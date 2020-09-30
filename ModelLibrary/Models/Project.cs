using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ModelLibrary
{
    public class Project
    {

        private int id;
        private string projectName;
        private string bundleIdentifier;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Required(ErrorMessage = "ProjectName must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "ProjectName should not contain special symbols(only (.) allowed)")]
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        [Required(ErrorMessage = "BundleIdentifier must not be null")]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z. ]+$", ErrorMessage = "BundleIdentifier should not contain special symbols(only (.) allowed)")]
        public string BundleIdentifier
        {
            get { return bundleIdentifier; }
            set { bundleIdentifier = value; }
        }
    }
}