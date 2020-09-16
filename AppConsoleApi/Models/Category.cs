using System;
using System.Collections.Generic;

namespace AppConsoleApi.Models
{
    public partial class Category
    {
        public Category()
        {
            Application = new HashSet<Application>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Application> Application { get; set; }
    }
}
