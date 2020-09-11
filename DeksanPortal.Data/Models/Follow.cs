using System;
using System.Collections.Generic;
using System.Text;

namespace DeksanPortal.Data.Models
{
    public class Follow
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string LibraryId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
