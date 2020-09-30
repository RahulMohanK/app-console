using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace ModelLibrary
{
    public class Application
    {
        private int appId;
        private string projectName;
        private string categoryName;
        private string fileName;
        private DateTime uploadedDate;
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        [Required(ErrorMessage = "Project Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "ProjectName should not contain special symbols(only (.) allowed)")]
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }

        }

        [Required(ErrorMessage = "Category Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "CategoryName should not contain special symbols(only (.) allowed)")]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        [Required(ErrorMessage = "FileName must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "FileName not allowed. Filename can contain((a-z),(A-Z),(.),(_),(-))")]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public DateTime UploadedDate
        {
            get { return uploadedDate; }
            set { uploadedDate = value; }
        }


    }

    public class ApplicationFile
    {
        private IFormFile file;

        [Required(ErrorMessage = "File must not be Empty")]
        public IFormFile File
        {
            get { return file; }
            set { file = value; }
        }


    }
}