using DeksanPortal.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeksanPortal.Data.Models
{
    public class Class
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Education EducationLevel { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
