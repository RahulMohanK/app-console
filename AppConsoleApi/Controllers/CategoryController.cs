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
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppConsoleApiContext context;

        public CategoryController(AppConsoleApiContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppConsoleApi.Models.Category>>> GetCategory()
        {
            return await context.Category.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AppConsoleApi.Models.Category>> GetCategory(int id)
        {
            var category = await context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public ActionResult<object> PostCategory(ModelLibrary.Category category)
        {
            DatabaseOperation db = new DatabaseOperation();
            db.AddCategory(category.CategoryName);
            return StatusCode(200);
        }


        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutCategory(int id, Category category)
        // {

        // }




        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Category>> DeleteCategory(int id)
        // {

        // }

        // private bool CategoryExists(int id)
        // {
        //     return context.Category.Any(e => e.Id == id);
        // }
    }
}
