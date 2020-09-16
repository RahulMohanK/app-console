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
    public class AppConsoleController : ControllerBase
    {
        private AppConsoleApiContext context;

        public AppConsoleController(AppConsoleApiContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> Get(string value)
        {

            if (value == "application")
            {

                // DatabaseOperation db = new DatabaseOperation();
                // db.AddApplication("aap", "test1", "aapfile.ipf");
                return await context.Application.ToListAsync();
            }
            else if (value == "project")
            {

                return await context.Project.ToListAsync();
            }
            else if (value == "category")
            {
                // DatabaseOperation db = new DatabaseOperation();
                // db.AddCategory("test1");
                return await context.Category.ToListAsync();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppConsoleApi.Models.Application>> GetApplication(int id)
        {
            var application = await context.Application.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        // [HttpPost]
        // public ActionResult<object> PostProject(ModelLibrary.Project project)
        // {
        //     DatabaseOperation db = new DatabaseOperation();
        //     db.AddProject(project.ProjectName, project.BundleIdentifier);

        //     FileHierarchyCreation file = new FileHierarchyCreation();
        //     file.CreateProjectFolder(project.ProjectName);

        //     return StatusCode(200);

        // }
        [HttpPost]
        public ActionResult<object> PostApplication(ModelLibrary.Application app)
        {
            ModelLibrary.Application application = new ModelLibrary.Application();
            DatabaseOperation db = new DatabaseOperation();
            DatabaseOperation db1 = new DatabaseOperation();

            db.AddApplication(app.ProjectName, app.CategoryName, app.FileName);
            application = db1.getSingleApplication(app.ProjectName, app.CategoryName, app.FileName);

            // FileHierarchyCreation file = new FileHierarchyCreation();
            // file.CreateApplicationFolder(application.AppId, app.CategoryName, app.ProjectName);

            return StatusCode(200);

        }
    }

}
