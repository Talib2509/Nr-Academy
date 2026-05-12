using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Entities.Identity;
using System.Reflection;

namespace NrAcademyDAL.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BlogPost> Blogs { get; set; }
        public DbSet<BlogCategory> Categories { get; set; }
        public DbSet<Enrollment> JobApplications { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; } 
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Teacher>()
                    .HasOne(t => t.User)
                    .WithOne()
                    .HasForeignKey<Teacher>(t => t.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Course>()
                    .HasOne(c => c.Teacher)
                    .WithMany()
                    .HasForeignKey(c => c.TeacherId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany()
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}