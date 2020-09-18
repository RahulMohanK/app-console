using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using  AppConsoleApi.Models;
using DatabaseOperationLibrary;
using FileOperationLibrary;
using model = ModelLibrary;

namespace AppConsoleApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly AppConsoleApiContext context;

        public ApplicationController(AppConsoleApiContext context)
        {
           this.context = context;
        }


        [HttpGet]
        public  ActionResult<List<ModelLibrary.Application>> GetApplication(string projectName, string categoryName)
        {      
            DatabaseOperation db = new DatabaseOperation();
            return db.getApplications(projectName,categoryName);
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

        [HttpPost]
        public ActionResult<object> PostApplication(ModelLibrary.Application app)
        {
           
            DatabaseOperation db = new DatabaseOperation();
            db.AddApplication(app.ProjectName, app.CategoryName, app.FileName);

             ModelLibrary.Application application = new ModelLibrary.Application();
            DatabaseOperation db1 = new DatabaseOperation();
            application = db1.getSingleApplication(app.ProjectName, app.CategoryName, app.FileName);

            FileHierarchyCreation file = new FileHierarchyCreation();
            file.CreateApplicationFolder(application.AppId, app.CategoryName, app.ProjectName,app.FileName);

            return StatusCode(200);

        }

        
        [HttpDelete("{appId}")]
        public ActionResult<AppConsoleApi.Models.Application> DeleteApplication(int appId)
        {
            var application =  context.Application.FirstOrDefault(e=> e.AppId == appId);
            if(application == null)
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.deleteApplication(appId);

            return application;

        }


        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutApplication(int id, Application application)
        // {

        // }


        // private bool ApplicationExists(int id)
        // {
        //     return context.Application.Any(e => e.AppId == id);
        // }
    }
}
