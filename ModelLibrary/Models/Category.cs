using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ModelLibrary
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "CategoryName should not contain special symbols(only (.) allowed)")]
        public string CategoryName { get; set; }
    }
}