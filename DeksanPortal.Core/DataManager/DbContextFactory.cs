using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeksanPortal.Core.DataManager
{
    public class DbContextFactory
    {
        readonly IConfiguration configuration;
        public DbContextFactory(IConfiguration appConfiguration)
        {
            configuration = appConfiguration;
        }
        public DeksanContext GetContext(string connectionStringName = "db_connection")
        {
            var builder = new DbContextOptionsBuilder<DeksanContext>().UseMySQL(configuration.GetConnectionString(connectionStringName));

            var _context = new DeksanContext(builder.Options);

            var dbConnextion = _context.Database.GetDbConnection();

            if (dbConnextion.State == System.Data.ConnectionState.Closed)
                dbConnextion.Open();

            return _context;
        }
    }
}
