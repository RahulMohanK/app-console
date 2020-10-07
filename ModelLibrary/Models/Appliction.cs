using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace ModelLibrary
{
    public class Application
    {
        
        public int AppId
        {
            get ;
            set ;
        }

        [Required(ErrorMessage = "Project Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "ProjectName should not contain special symbols(only (.) allowed)")]
        public string ProjectName
        {
            get ;
            set;

        }

        [Required(ErrorMessage = "Category Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "CategoryName should not contain special symbols(only (.) allowed)")]
        public string CategoryName
        {
            get ;
            set ;
        }

        [Required(ErrorMessage = "FileName must not be null")]
        [DataType(DataType.Text)]
       // [RegularExpression("^[a-zA-Z0-9_\-. ]+$", ErrorMessage = "FileName not allowed. Filename can contain((a-z),(A-Z),(.),(_),(-))")]
        public string FileName
        {
            get ;
            set ;
        }

        public DateTime UploadedDate
        {
            get ;
            set ;
        }


    }

    public class ApplicationFile
    {
       

        [Required(ErrorMessage = "File must not be Empty")]
        public IFormFile File
        {
            get ;
            set ;
        }


    }
}