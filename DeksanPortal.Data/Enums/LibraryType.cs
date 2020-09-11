using System;
using System.Collections.Generic;
using System.Text;

namespace DeksanPortal.Data.Enums
{
    public enum LibraryType
    {
        EPUB = 1,
        PDF = 2,
        VIDEO = 4,

        BOOKS = EPUB | PDF
    }
}
