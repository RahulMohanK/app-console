using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseOperationLibrary;
using FileOperationLibrary;
using ModelLibrary;

namespace AppConsoleApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        
        [HttpGet]
        [ActionName("getCategory")]
        public  ActionResult<IEnumerable<Category>> GetCategory()
        {
            DatabaseOperation db = new DatabaseOperation();
            return db.GetCategories();
        }


        [HttpGet]
         [ActionName("getSpecificCategory")]
        public ActionResult<IEnumerable<Category>> GetProjectSpecificCategory(string projectName)
        {
            DatabaseOperation db = new DatabaseOperation();
            return db.GetProjectSpecificCategory(projectName);
        }

        [HttpPost]
        [ActionName("addCategory")]
        public ActionResult<Category> PostCategory(Category category)
        {   
            try{
            DatabaseOperation db = new DatabaseOperation();
            db.AddCategory(category.CategoryName);
           
            }
            catch(System.Data.SqlClient.SqlException)
            {
                return StatusCode(500,new{ title = "Category addition error", status = 500, message="Cannot have duplicate category" });
            }
             return StatusCode(200, new{ title = "Category added successfully.", status = 200 });
        }

        [HttpDelete("{categoryName}")]
        [ActionName("deleteCategory")]
        public ActionResult<Category> DeleteCategory(string categoryName)
        {   
            if(String.IsNullOrEmpty(categoryName))
            {
                 return StatusCode(500,new { title = "CategoryName must not be empty", status = 500 });
            }
            if(!categoryExists(categoryName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.DeleteCategory(categoryName);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.DeleteCategoryFolder(categoryName);
            return StatusCode(200,new { title = "Applicaton deleted successfully.", status = 200 });
        }

        [HttpDelete("{projectName}/{categoryName}")]
        [ActionName("deleteSpecificCategory")]
        public ActionResult<Category> DeleteCategory(string projectName,string categoryName)
        {   
           
            if(!projectSpecificCategoryExists(projectName,categoryName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.DeleteProjectSpecificCategory(projectName,categoryName);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.DeleteSpecificCategoryFolder(projectName,categoryName);
            return StatusCode(200,new { title = "Applicaton deleted successfully.", status = 200 });
        }


        [HttpPut("{oldCategoryName}")]
        [ActionName("updateCategory")]
        public ActionResult<Category> PutCategory(string oldCategoryName, Category category)
        {
            if(!categoryExists(oldCategoryName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.UpdateCategory(oldCategoryName,category.CategoryName);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.EditCategoryFolder(oldCategoryName,category.CategoryName);
            return StatusCode(200, new{ title = "Category updated successfully.", status = 200 });
        }
 
        private bool categoryExists(string categoryName)
        {
            DatabaseOperation db = new DatabaseOperation();
            if(db.GetSingleCategory(categoryName).Count == 0)
            {
                return false;
            }
            return true;
            
        }
    
        private bool projectSpecificCategoryExists(string projectName,string categoryName)
        {
             DatabaseOperation db = new DatabaseOperation();
             List<Category> categoryList = new List<Category>();
             categoryList = db.GetProjectSpecificCategory(projectName);
            foreach(var val in categoryList)
            {
                if(val.CategoryName == categoryName)
                {     
                    return true;
                }
            }   
            return false;
        }
    }
}
