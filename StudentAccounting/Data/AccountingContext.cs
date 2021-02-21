using Microsoft.EntityFrameworkCore;
using StudentAccounting.Models;

namespace StudentAccounting.Data
{
    public class AccountingContext : DbContext
    {
        public AccountingContext(DbContextOptions<AccountingContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<CourseSelection> CourseSelections { get; set; }
        public DbSet<CourseSelectionOwing> CourseSelectionOwings { get; set; }

        /// <summary>
        /// Override the default behaviour to create table name in singular form
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<CourseSelection>().ToTable("CourseSelection");
            modelBuilder.Entity<CourseSelectionOwing>().ToTable("CourseSelectionOwing");
        }
    }
}
