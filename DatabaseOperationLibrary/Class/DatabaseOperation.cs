﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using ModelLibrary;
using System.Collections.Specialized;



namespace DatabaseOperationLibrary
{
    public class DatabaseOperation
    {
        string connectionString;
        SqlConnection connection;

        public DatabaseOperation()
        {
            connectionString = ConfigurationManager.AppSettings["connectionstring"];
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public bool Authenticate(string username, string password)
        {
            bool flag;
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_Authenticate", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@User_Name", SqlDbType.NVarChar).Value = username;
            sqlCommand.Parameters.AddWithValue("@Password", SqlDbType.NVarChar).Value = password;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                flag = true;      
            }
            else{ flag = false;}
            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();
            return flag;
        }

        public void ChangePassword(string username,string oldPassword,string newPassword)
        {
            
            SqlCommand sqlCommand;
            sqlCommand = new SqlCommand("Proc_console_changePassword", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@User_Name", SqlDbType.NVarChar).Value = username;
            sqlCommand.Parameters.AddWithValue("@Old_Password", SqlDbType.NVarChar).Value = oldPassword;
            sqlCommand.Parameters.AddWithValue("@New_Password", SqlDbType.NVarChar).Value = newPassword;    
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();
        }

        public void AddProject(string projectName, string bundleIdentifier)
        {
            SqlCommand sqlCommand;
            sqlCommand = new SqlCommand("Proc_Console_addProject", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Project_Name", SqlDbType.NVarChar).Value = projectName;
            sqlCommand.Parameters.AddWithValue("@Bundle_Identifier", SqlDbType.NVarChar).Value = bundleIdentifier;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();
            //Console.WriteLine("yes");
        }

        public void AddCategory(string categoryName)
        {
            SqlCommand sqlCommand;
            sqlCommand = new SqlCommand("Proc_Console_addCategory", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Category_Name", SqlDbType.NVarChar).Value = categoryName;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();
          //  Console.WriteLine("yes");
        }

        public void AddApplication(string projectName, string categoryName, string fileName)
        {
            SqlCommand sqlCommand;
            sqlCommand = new SqlCommand("Proc_Console_addApplication", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Project_Name", SqlDbType.NVarChar).Value = projectName;
            sqlCommand.Parameters.AddWithValue("@Category_Name", SqlDbType.NVarChar).Value = categoryName;
            sqlCommand.Parameters.AddWithValue("@File_Name", SqlDbType.NVarChar).Value = fileName;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();
           // Console.WriteLine("yes");
        }

        public List<Project> GetProjects()
        {
            List<Project> projectList = new List<Project>();
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_getProjects", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Project project = new Project();
                    project.Id = (int)sqlDataReader.GetValue(0);
                    project.ProjectName = sqlDataReader.GetValue(1).ToString();
                    project.BundleIdentifier = sqlDataReader.GetValue(2).ToString();
                    projectList.Add(project);
                }
            }

            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();

            return projectList;
        }

        public List<Category> GetCategories()
        {
            List<Category> categoryList = new List<Category>();
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_getCategories", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Category category = new Category();
                    category.Id = (int)sqlDataReader.GetValue(0);
                    category.CategoryName = sqlDataReader.GetValue(1).ToString();
                    categoryList.Add(category);

                }
            }
            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();

            return categoryList;
        }

        public List<Category> GetProjectSpecificCategory(string projectName)
        {
            List<ModelLibrary.Category> categoryList = new List<ModelLibrary.Category>();
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_getProjectSpecificCategory", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Project_Name", SqlDbType.NVarChar).Value = projectName;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    ModelLibrary.Category category = new ModelLibrary.Category();
                    category.Id = (int)sqlDataReader.GetValue(0);
                    category.CategoryName = sqlDataReader.GetValue(1).ToString();
                    categoryList.Add(category);

                }
            }
            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();
            return categoryList;
        }

        public Application GetSingleApplication(string projectName, string categoryName, string fileName)
        {
            Application app = new Application();
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_getSingleApplication", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Project_Name", SqlDbType.NVarChar).Value = projectName;
            sqlCommand.Parameters.AddWithValue("@Category_Name", SqlDbType.NVarChar).Value = categoryName;
            sqlCommand.Parameters.AddWithValue("@File_Name", SqlDbType.NVarChar).Value = fileName;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    app.AppId = (int)sqlDataReader.GetValue(0);
                    // Console.WriteLine("get Application" + Convert.ToString(app.AppId));
                    app.ProjectName = sqlDataReader.GetValue(1).ToString();
                    app.CategoryName = sqlDataReader.GetValue(2).ToString();
                    app.FileName = sqlDataReader.GetValue(3).ToString();
                    app.UploadedDate = (DateTime)sqlDataReader.GetValue(4);
                }
            }

            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();

            return app;
        }

        public List<ModelLibrary.Application> GetSingleApp(int appId)
        {
            List<ModelLibrary.Application> appList = new List<ModelLibrary.Application>();
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_getSingleApp", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@App_Id", SqlDbType.Int).Value = appId;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    ModelLibrary.Application app = new ModelLibrary.Application();
                    app.AppId = (int)sqlDataReader.GetValue(0);
                    app.ProjectName = sqlDataReader.GetValue(1).ToString();
                    app.CategoryName = sqlDataReader.GetValue(2).ToString();
                    app.FileName = sqlDataReader.GetValue(3).ToString();
                    app.UploadedDate = (DateTime)sqlDataReader.GetValue(4);
                    appList.Add(app);
                }
            }

            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();

            return appList;
        }

        public List<ModelLibrary.Category> GetSingleCategory(string categoryName)
        {
            List<ModelLibrary.Category> categoryList = new List<ModelLibrary.Category>();
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_getSingleCategory", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Category_Name", SqlDbType.NVarChar).Value = categoryName;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    ModelLibrary.Category category = new ModelLibrary.Category();
                    category.Id = (int)sqlDataReader.GetValue(0);
                    category.CategoryName = sqlDataReader.GetValue(1).ToString();
                    categoryList.Add(category);

                }
            }
            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();
            return categoryList;
        }
        public List<ModelLibrary.Project> GetSingleProject(string projectName)
        {
            List<ModelLibrary.Project> projectList = new List<ModelLibrary.Project>();
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_getSingleProject", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@Project_Name", SqlDbType.NVarChar).Value = projectName;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    ModelLibrary.Project project = new ModelLibrary.Project();
                    project.Id = (int)sqlDataReader.GetValue(0);
                    project.ProjectName = sqlDataReader.GetValue(1).ToString();
                    project.BundleIdentifier = sqlDataReader.GetValue(2).ToString();
                    projectList.Add(project);
                }
            }

            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();

            return projectList;
        }

        public List<Application> GetApplications(string projectName, string categoryName)
        {
            List<Application> appList = new List<Application>();
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            sqlCommand = new SqlCommand("Proc_Console_getApplications", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Project_Name", SqlDbType.NVarChar).Value = projectName;
            sqlCommand.Parameters.AddWithValue("@Category_Name", SqlDbType.NVarChar).Value = categoryName;
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Application app = new Application();
                    app.AppId = (int)sqlDataReader.GetValue(0);
                    app.ProjectName = sqlDataReader.GetValue(1).ToString();
                    app.CategoryName = sqlDataReader.GetValue(2).ToString();
                    app.FileName = sqlDataReader.GetValue(3).ToString();
                    app.UploadedDate = (DateTime)sqlDataReader.GetValue(4);
                    appList.Add(app);
                }
            }

            sqlDataReader.Close();
            sqlCommand.Dispose();
            connection.Close();

            return appList;
        }

        public void DeleteProject(string projectName)
        {

            SqlCommand sqlCommand;
            SqlDataAdapter adapter = new SqlDataAdapter();
            sqlCommand = new SqlCommand("Proc_Console_deleteProject", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Project_Name", SqlDbType.NVarChar).Value = projectName;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();


        }
        public void DeleteCategory(string categoryName)
        {

            SqlCommand sqlCommand;
            SqlDataAdapter adapter = new SqlDataAdapter();
            sqlCommand = new SqlCommand("Proc_Console_deleteCategory", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Category_Name", SqlDbType.NVarChar).Value = categoryName;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();

        }

        public void DeleteProjectSpecificCategory(string projectName, string categoryName)
        {

            SqlCommand sqlCommand;
            SqlDataAdapter adapter = new SqlDataAdapter();
            sqlCommand = new SqlCommand("Proc_Console_deleteProjectSpecificCategory", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Project_Name", SqlDbType.NVarChar).Value = projectName;
            sqlCommand.Parameters.AddWithValue("@Category_Name", SqlDbType.NVarChar).Value = categoryName;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();

        }
        public void DeleteApplication(int appId)
        {

            SqlCommand sqlCommand;
            SqlDataAdapter adapter = new SqlDataAdapter();
            sqlCommand = new SqlCommand("Proc_Console_deleteApplication", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@App_Id", SqlDbType.Int).Value = appId;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();

        }

        public void UpdateProject(string oldProjectName, string newProjectName, string newBundleIdentifier)
        {


            SqlCommand sqlCommand;
            sqlCommand = new SqlCommand("Proc_console_updateProject", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@OldProject_Name", SqlDbType.NVarChar).Value = oldProjectName;
            sqlCommand.Parameters.AddWithValue("@NewProject_Name", SqlDbType.NVarChar).Value = newProjectName;
            sqlCommand.Parameters.AddWithValue("@NewBundle_Identifier", SqlDbType.NVarChar).Value = newBundleIdentifier;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();

        }

        public void UpdateCategory(string oldCategoryName, string newCategoryName)
        {

            SqlCommand sqlCommand;
            sqlCommand = new SqlCommand("Proc_console_updateCategory", connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@OldCategory_Name", SqlDbType.NVarChar).Value = oldCategoryName;
            sqlCommand.Parameters.AddWithValue("@NewCategory_Name", SqlDbType.NVarChar).Value = newCategoryName;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();

        }


    }
}
