using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ModelLibrary
{
    public class Project
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "ProjectName must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "ProjectName should not contain special symbols(only (.) allowed)")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "BundleIdentifier must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "BundleIdentifier should not contain special symbols(only (.) allowed)")]
        public string BundleIdentifier { get; set; }
    }
}