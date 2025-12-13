using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiTest.Models;

namespace ApiTest.Data
{
    public class ApiTestContext : DbContext
    {
        public ApiTestContext(DbContextOptions<ApiTestContext> options):base(options)
        {
            
        }
        //Add ObSets (Tables)
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(c => c.Year)
                    .IsRequired();

                entity.ToTable(tb => tb.HasCheckConstraint(
                    "CK_Course_Year", 
                    "\"Year\" >= 1 AND \"Year\" <= 4"
                ));
            });

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "Relationship",
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
                    j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId")
            );
        }
    }
}


//dotnet ef migrations add [nombre]
//dotnet ef database update