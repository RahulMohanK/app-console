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
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace AppConsoleApi.Controllers     
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {

        [HttpGet]
        public  ActionResult<List<Application>> GetApplication(string projectName, string categoryName)
        {      
            DatabaseOperation db = new DatabaseOperation();
            return db.GetApplications(projectName,categoryName);
        }

        [HttpPost]
        public ActionResult<Application> PostApplication([FromForm] ApplicationFile appFile,[FromForm]Application app)
        {   
            //appending date with filename to make dbo.Application.File_Name unique

            string tempFileName = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")+"/"+app.FileName;
            DatabaseOperation db = new DatabaseOperation();
            db.AddApplication(app.ProjectName, app.CategoryName, tempFileName);

            Application application = new Application();
            DatabaseOperation db1 = new DatabaseOperation();
            application = db1.GetSingleApplication(app.ProjectName, app.CategoryName, tempFileName);

           
             DatabaseOperation db2 = new DatabaseOperation();
             Project project = db2.GetSingleProject(app.ProjectName)[0];
          

            FileHierarchyCreation file = new FileHierarchyCreation();
            bool uploadCheck = file.CreateApplicationFolder(application.AppId, app.CategoryName, app.ProjectName,tempFileName,project.BundleIdentifier,appFile);
            
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
        public ActionResult<Application> DeleteApplication(int appId)
        {         
            if(!applicationExists(appId))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.DeleteApplication(appId);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.DeleteApplicationFolder(appId);
            return StatusCode(200,new { title = "Applicaton deleted successfully.", status = 200 });
        }


        private bool applicationExists(int id)
        {
             DatabaseOperation db = new DatabaseOperation();
            if(db.GetSingleApp(id).Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
