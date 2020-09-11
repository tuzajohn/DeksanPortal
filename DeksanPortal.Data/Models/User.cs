using DeksanPortal.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DeksanPortal.Data.Models
{
    public class User
    {
        public string Id { get; set; }

        public string AccountId { get; set; }

        public string ClassId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        [StringLength(100)]
        public string School { get; set; }

        [StringLength(50)]
        public string Contact { get; set; }

        [StringLength(300)]
        public string Address { get; set; }

        public Education Education { get; set; }

        public DateTime CreatedOn { get; set; }

        [NotMapped]
        public int Age => DateTime.UtcNow.Year - DateOfBirth.Year;

        [NotMapped]
        public string Name => FirstName + " " + LastName;

    }
}
