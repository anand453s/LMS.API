using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LMS.Repository.Entities;

namespace LMS.Repository.EF.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<RoleType> roleTypes { get; set; }
        public DbSet<UserLogin> userLogins { get; set; }
        public DbSet<StudentDetails> students { get; set; }
        public DbSet<InstructorDetails> instructors { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<CourseMaterial> courseMaterials { get; set; }
        public DbSet<StudentCourse> studentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=SCLIENT-68;Initial Catalog=LmsProDB;Integrated Security=True;TrustServerCertificate=True;");
        }
    }
}
