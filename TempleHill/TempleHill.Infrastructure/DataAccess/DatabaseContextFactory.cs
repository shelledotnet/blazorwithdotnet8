using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleHill.Infrastructure.DataAccess
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<THDbContext>
    {
        public THDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<THDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(localDb)\\MSSQLLocalDb;Initial Catalog=TemphillDb;");
            return new THDbContext(optionsBuilder.Options);
        }
    }
}
