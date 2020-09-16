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
using ModelLibrary;


namespace AppConsoleApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly AppConsoleApiContext context;

        public ProjectController(AppConsoleApiContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppConsoleApi.Models.Project>>> GetProject()
        {
            return await context.Project.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AppConsoleApi.Models.Project>> GetProject(int id)
        {
            var project = await context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpPost]
        public ActionResult<object> PostProject(ModelLibrary.Project project)
        {
            DatabaseOperation db = new DatabaseOperation();
            db.AddProject(project.ProjectName, project.BundleIdentifier);

            FileHierarchyCreation file = new FileHierarchyCreation();
            file.CreateProjectFolder(project.ProjectName);

            return StatusCode(200);

        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutProject(int id, Project project)
        // {

        // }
        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Project>> DeleteProject(int id)
        // {

        // }

        // private bool ProjectExists(int id)
        // {
        //     return context.Project.Any(e => e.Id == id);
        // }
    }
}
