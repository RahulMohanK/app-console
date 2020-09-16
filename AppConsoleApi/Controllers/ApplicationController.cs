using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using  AppConsoleApi.Models;
using DatabaseOperationLibrary;
using FileOpearionLibrary;
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
        public async Task<ActionResult<IEnumerable<AppConsoleApi.Models.Application>>> GetApplication()
        {
            return await context.Application.ToListAsync();
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
            ModelLibrary.Application application = new ModelLibrary.Application();
            DatabaseOperation db = new DatabaseOperation();
            DatabaseOperation db1 = new DatabaseOperation();

            db.AddApplication(app.ProjectName, app.CategoryName, app.FileName);
            application = db1.getSingleApplication(app.ProjectName, app.CategoryName, app.FileName);

            FileHierarchyCreation file = new FileHierarchyCreation();
            file.CreateApplicationFolder(application.AppId, app.CategoryName, app.ProjectName);

            return StatusCode(200);

        }


        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutApplication(int id, Application application)
        // {

        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Application>> DeleteApplication(int id)
        // {

        // }

        // private bool ApplicationExists(int id)
        // {
        //     return context.Application.Any(e => e.AppId == id);
        // }
    }
}
