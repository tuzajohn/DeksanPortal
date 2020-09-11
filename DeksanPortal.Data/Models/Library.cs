using DeksanPortal.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeksanPortal.Data.Models
{
    public class Library
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public LibraryType LibraryType { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ResourceUrl { get; set; }
        public Education EducationLevel { get; set; }
        public string ClassId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
