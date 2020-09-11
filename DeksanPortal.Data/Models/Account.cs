using DeksanPortal.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeksanPortal.Data.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountStatus Type { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
