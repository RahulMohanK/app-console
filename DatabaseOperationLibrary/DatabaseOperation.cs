using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using ModelLibrary;
namespace DatabaseOperationLibrary
{
    public class DatabaseOperation
    {
        string connectionString;
        SqlConnection connection;

        public DatabaseOperation()
        {
            connectionString = "Server=DESKTOP-ROBHQ7Q;Database=Console Database;Trusted_Connection=True";

            connection = new SqlConnection(connectionString);
            connection.Open();
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
            Console.WriteLine("yes");
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
            Console.WriteLine("yes");
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
            Console.WriteLine("yes");
        }

        public Application getSingleApplication(string projectName, string categoryName, string fileName)
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
                    Console.WriteLine("get Application" + Convert.ToString(app.AppId));
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


    }
}
