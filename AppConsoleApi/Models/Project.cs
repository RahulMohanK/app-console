using System;
using System.Collections.Generic;

namespace AppConsoleApi.Models
{
    public partial class Project
    {
        public Project()
        {
            Application = new HashSet<Application>();
        }

        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string BundleIdentifier { get; set; }

        public virtual ICollection<Application> Application { get; set; }
    }
}
