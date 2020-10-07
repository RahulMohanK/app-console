using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace ModelLibrary
{
    public class Project
    {

        
        public int Id
        {
            get;
            set;
        }

        [Required(ErrorMessage = "ProjectName must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z _.-]+$", ErrorMessage = "ProjectName should not contain special symbols(only (.) allowed)")]
        public string ProjectName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "BundleIdentifier must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "BundleIdentifier should not contain special symbols(only (.) allowed)")]
        public string BundleIdentifier
        {
            get;
            set;
        }
    }
    public class ProjectIcon
    {
       
        [Required(ErrorMessage = "Project Icon must be uploaded")]
        public IFormFile Icon
        {
            get;
            set;
        }
    }
    public class EditProjectIcon
    {
        

        public IFormFile Icon
        {
            get;
            set;
        }
    }
}