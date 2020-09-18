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
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
        public ActionResult<object> PostApplication([FromForm] ModelLibrary.ApplicationFile appFile,[FromForm]ModelLibrary.Application app)
        {
            Console.WriteLine("file : "+appFile.files.Length.ToString());
            Console.WriteLine("application : "+app.AppId.ToString());
           
            DatabaseOperation db = new DatabaseOperation();
            db.AddApplication(app.ProjectName, app.CategoryName, app.FileName);

            ModelLibrary.Application application = new ModelLibrary.Application();
            DatabaseOperation db1 = new DatabaseOperation();
            application = db1.getSingleApplication(app.ProjectName, app.CategoryName, app.FileName);

            var project =  context.Project.FirstOrDefault(e => e.ProjectName == app.ProjectName);

            FileHierarchyCreation file = new FileHierarchyCreation();
            bool uploadCheck = file.CreateApplicationFolder(application.AppId, app.CategoryName, app.ProjectName,app.FileName,project.BundleIdentifier,appFile);
            
            if(uploadCheck)
            {
            return StatusCode(200,new { title = "Applicaton uploaded successfully.", status = 200});
            }
            else
            {
                return StatusCode(500,new { title = "Applicaton upload error.", status = 500,message = "This app version already exists"});
            }

        }     
        
         
        [HttpDelete("{appId}")]
        public ActionResult<AppConsoleApi.Models.Application> DeleteApplication(int appId)
        {         
            if(!ApplicationExists(appId))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.deleteApplication(appId);

            return StatusCode(200,new { title = "Applicaton deleted successfully.", status = 200 });
        }


        private bool ApplicationExists(int id)
        {
            return context.Application.Any(e => e.AppId == id);
        }
    }
}
