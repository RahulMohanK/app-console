using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppConsoleApi.Models;
using DatabaseOperationLibrary;
using FileOperationLibrary;
using ModelLibrary;

namespace AppConsoleApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppConsoleApiContext context;

        public CategoryController(AppConsoleApiContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [ActionName("getCategory")]
        public  ActionResult<IEnumerable<ModelLibrary.Category>> GetCategory()
        {
            DatabaseOperation db = new DatabaseOperation();
            return db.getCategories();
        }


        [HttpGet]
         [ActionName("getSpecificCategory")]
        public ActionResult<IEnumerable<ModelLibrary.Category>> GetProjectSpecificCategory(string projectName)
        {
            DatabaseOperation db = new DatabaseOperation();
            return db.getProjectSpecificCategory(projectName);
        }

        [HttpPost]
        [ActionName("addCategory")]
        public ActionResult<ModelLibrary.Category> PostCategory(ModelLibrary.Category category)
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
        public ActionResult<AppConsoleApi.Models.Category> DeleteCategory(string categoryName)
        {   
            
            if(!CategoryExists(categoryName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.deleteCategory(categoryName);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.DeleteCategoryFolder(categoryName);
            return StatusCode(200,new { title = "Applicaton deleted successfully.", status = 200 });
        }

        [HttpDelete("{projectName}/{categoryName}")]
        [ActionName("deleteSpecificCategory")]
        public ActionResult<AppConsoleApi.Models.Category> DeleteCategory(string projectName,string categoryName)
        {   
           
            if(!projectSpecificCategoryExists(projectName,categoryName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.deleteProjectSpecificCategory(projectName,categoryName);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.DeleteSpecificCategoryFolder(projectName,categoryName);
            return StatusCode(200,new { title = "Applicaton deleted successfully.", status = 200 });
        }


        [HttpPut("{oldCategoryName}")]
        [ActionName("updateCategory")]
        public ActionResult<ModelLibrary.Category> PutCategory(string oldCategoryName, ModelLibrary.Category category)
        {
            if(!CategoryExists(oldCategoryName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.UpdateCategory(oldCategoryName,category.CategoryName);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.EditCategoryFolder(oldCategoryName,category.CategoryName);
            return StatusCode(200, new{ title = "Category updated successfully.", status = 200 });
        }
 
        private bool CategoryExists(string categoryName)
        {
            DatabaseOperation db = new DatabaseOperation();
            if(db.getSingleCategory(categoryName).Count == 0)
            {
                return false;
            }
            return true;
            
        }
    
        private bool projectSpecificCategoryExists(string projectName,string categoryName)
        {
             DatabaseOperation db = new DatabaseOperation();
             List<ModelLibrary.Category> categoryList = new List<ModelLibrary.Category>();
             categoryList = db.getProjectSpecificCategory(projectName);
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
