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
            return db.getProjects();
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
        public ActionResult<ModelLibrary.Project> PostProject(ModelLibrary.Project project)
        {
            try{
            DatabaseOperation db = new DatabaseOperation();
            db.AddProject(project.ProjectName, project.BundleIdentifier);

            FileHierarchyCreation file = new FileHierarchyCreation();
            file.CreateProjectFolder(project.ProjectName);
            }
            catch(System.Data.SqlClient.SqlException)
            {
                return StatusCode(500,new { title = "Project addition error", status = 500, message="Cannot have duplicate project name" });
            }

            return StatusCode(200,new { title = "Project added successfully.", status = 200 });

        }

        [HttpDelete("{projectName}")]
        public  ActionResult<AppConsoleApi.Models.Project> DeleteProject(string projectName)
        {
            
            if(!ProjectExists(projectName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.deleteProject(projectName);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.DeleteProjectFolder(projectName);
            return StatusCode(200,new { title = "Deletion successfull.", status = 200 });
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
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.EditProjectFolder(oldProjectName,project.ProjectName,project.BundleIdentifier);

            return StatusCode(200, new{ title = "Project updated successfully.", status = 200 });
        }
     
        private bool ProjectExists(string projectName)
        {
           DatabaseOperation db = new DatabaseOperation();
            if(db.getSingleProject(projectName).Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
