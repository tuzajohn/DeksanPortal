using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeksanPortal.Data.Models
{
    public class Category
    {
        public string Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
