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
        public ActionResult<object> PostCategory(ModelLibrary.Category category)
        {
            DatabaseOperation db = new DatabaseOperation();
            db.AddCategory(category.CategoryName);
            return StatusCode(200);
        }

        [HttpDelete("{categoryName}")]
        [ActionName("deleteCategory")]
        public ActionResult<AppConsoleApi.Models.Category> DeleteCategory(string categoryName)
        {
            var category = context.Category.FirstOrDefault(e=> e.CategoryName == categoryName);
            if(category == null)
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.deleteCategory(categoryName);
            return category;
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

            return StatusCode(200);
        }
 
        private bool CategoryExists(string categoryName)
        {
            return context.Category.Any(e => e.CategoryName == categoryName);
        }
    }
}
