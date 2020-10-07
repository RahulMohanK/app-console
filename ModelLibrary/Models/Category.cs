using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ModelLibrary
{
    public class Category
    {
      
        public int Id
        {
            get ;
            set;
        }

        [Required(ErrorMessage = "Category Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z _.-]+$", ErrorMessage = "CategoryName should not contain special symbols(only (.,-,_) allowed)")]
        public string CategoryName
        {
            get ;
            set;
        }
    }
}