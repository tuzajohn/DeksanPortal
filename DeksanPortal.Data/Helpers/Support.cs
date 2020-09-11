using System;
using System.Collections.Generic;
using System.Text;

namespace DeksanPortal.Data.Helpers
{
    public class Support
    {
        public static string Id => Guid.NewGuid().ToString("N");
    }
}
