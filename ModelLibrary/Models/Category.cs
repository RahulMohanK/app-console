using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ModelLibrary
{
    public class Category
    {
        private int id;
        private string categoryName;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Required(ErrorMessage = "Category Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "CategoryName should not contain special symbols(only (.) allowed)")]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
    }
}