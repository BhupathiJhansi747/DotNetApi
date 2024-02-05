using EmployeeAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EmployeeAPI.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(e => e.Emp_id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
