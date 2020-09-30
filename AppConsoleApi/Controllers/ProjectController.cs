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


namespace AppConsoleApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        [HttpGet]
        public  ActionResult<IEnumerable<Project>> GetProject()
        {
            DatabaseOperation db = new DatabaseOperation();
            return db.GetProjects();
        }
     
        [HttpPost]
        public ActionResult<Project> PostProject(Project project)
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
        public  ActionResult<Project> DeleteProject(string projectName)
        {
            if(String.IsNullOrEmpty(projectName))
            {
                return StatusCode(500,new { title = "ProjectName must not be empty", status = 500 });
            }
            
            if(!projectExists(projectName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.DeleteProject(projectName);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.DeleteProjectFolder(projectName);
            return StatusCode(200,new { title = "Deletion successfull.", status = 200 });
        }


        [HttpPut("{oldProjectName}")]
        public ActionResult<Project> PutProject(string oldProjectName, Project project)
        {
            if(!projectExists(oldProjectName))
            {
                return NotFound();
            }
            DatabaseOperation db = new DatabaseOperation();
            db.UpdateProject(oldProjectName,project.ProjectName,project.BundleIdentifier);
            FileHierarchyCreation file = new FileHierarchyCreation();
            file.EditProjectFolder(oldProjectName,project.ProjectName,project.BundleIdentifier);

            return StatusCode(200, new{ title = "Project updated successfully.", status = 200 });
        }
     
        private bool projectExists(string projectName)
        {
           DatabaseOperation db = new DatabaseOperation();
            if(db.GetSingleProject(projectName).Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
