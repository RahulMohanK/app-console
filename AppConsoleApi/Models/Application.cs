using System;
using System.Collections.Generic;

namespace AppConsoleApi.Models
{
    public partial class Application
    {
        public int AppId { get; set; }
        public int ProjectId { get; set; }
        public int CategoryId { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual Project Project { get; set; }
    }
}
