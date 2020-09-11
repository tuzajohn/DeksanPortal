using DeksanPortal.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeksanPortal.Web.Models
{
    public class BookViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public string ClassLevel { get; set; }

        [Required]
        public string Categories { get; set; }

        [Required]
        public Education educationLevel { get; set; }

        [Required]
        public IFormFile ThumbnailUrl { get; set; }

        [Required]
        public IFormFile Resource { get; set; }
    }
}
