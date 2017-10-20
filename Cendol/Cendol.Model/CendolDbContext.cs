using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cendol.Model
{
    public class CendolDbContext : DbContext
    {
        public DbSet<InputItem> InputItems { get; set; }

        public CendolDbContext(DbContextOptions<CendolDbContext> options)
            : base(options)
        {
        }

        public CendolDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(local);Database=Cendol;Trusted_Connection=True;");
        }
    }
}
