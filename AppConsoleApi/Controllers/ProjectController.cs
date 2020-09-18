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
        public  ActionResult<IEnumerable<ModelLibrary.Project>> GetProject()
        {
            DatabaseOperation db = new DatabaseOperation();
            return  db.getProjects();
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

        [HttpDelete("{projectName}")]
        public  ActionResult<AppConsoleApi.Models.Project> DeleteProject(string projectName)
        {
            var project = context.Project.FirstOrDefault(e => e.ProjectName == projectName);
            if(project == null)
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.deleteProject(projectName);
            return project;
        }


        [HttpPut("{oldProjectName}")]
        public ActionResult<ModelLibrary.Project> PutProject(string oldProjectName, ModelLibrary.Project project)
        {
            if(!ProjectExists(oldProjectName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.UpdateProject(oldProjectName,project.ProjectName,project.BundleIdentifier);

            return StatusCode(200);
        }
     
        private bool ProjectExists(string projectName)
        {
            return context.Project.Any(e => e.ProjectName == projectName);
        }
    }
}
