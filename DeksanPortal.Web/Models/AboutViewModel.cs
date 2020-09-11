using DeksanPortal.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeksanPortal.Web.Models
{
    public class AboutViewModel
    {

        public string Occupation { get; set; }
        [Required]
        public Education EducationLevel { get; set; }
        public string EducationClass { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
    }
}
