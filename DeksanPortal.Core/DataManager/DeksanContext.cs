using DeksanPortal.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeksanPortal.Core.DataManager
{
    public class DeksanContext : DbContext
    {
        public DeksanContext(DbContextOptions<DeksanContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<CategoryLibrary> CategoryLibraries { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
