using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ModelLibrary
{
    public class Application
    {
        public int AppId { get; set; }

        [Required(ErrorMessage = "Project Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "ProjectName should not contain special symbols(only (.) allowed)")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Category Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "CategoryName should not contain special symbols(only (.) allowed)")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "FileName must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "FileName not allowed. Filename can contain((a-z),(A-Z),(.),(_),(-))")]
        public string FileName { get; set; }

        public DateTime UploadedDate {get; set;}


    }
}