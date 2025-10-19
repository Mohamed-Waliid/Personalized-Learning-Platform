using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Graduation_Project.Models;

namespace Graduation_Project.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<CourseTrack> CourseTracks { get; set; }
        public DbSet<CourseMaterial> CourseMaterials { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<LearningMaterial> LearningMaterials { get; set; }
        public DbSet<CompletedMaterial> CompletedMaterials { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Enrollment Relationships (Many-to-Many between Student & Course)

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Cascade);



            //CourseMaterials Relationships(Many-to - Many between LearningMaterial &Course)

            modelBuilder.Entity<CourseMaterial>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<CourseMaterial>()
                .HasOne(cm => cm.Course)
                .WithMany(s => s.CourseMaterials)
                .HasForeignKey(cm => cm.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseMaterial>()
                .HasOne(cm => cm.LearningMaterial)
                .WithMany(c => c.CourseMaterials)
                .HasForeignKey(cm => cm.MaterialID)
                .OnDelete(DeleteBehavior.Cascade);



            //CourseTracks Relationships(Many-to - Many between Course & Track)

            modelBuilder.Entity<CourseTrack>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<CourseTrack>()
                .HasIndex(ct => new { ct.CourseID, ct.TrackID })
                .IsUnique();

            modelBuilder.Entity<CourseTrack>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTracks)
                .HasForeignKey(ct => ct.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseTrack>()
                .HasOne(ct => ct.Track)
                .WithMany(t => t.CourseTracks)
                .HasForeignKey(ct => ct.TrackID)
                .OnDelete(DeleteBehavior.Cascade);


            // Instructor - Course Relationship
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.InstructorID)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Course>()
            //    .HasMany(c => c.CourseTracks)
            //    .WithMany(t => t.CourseTracks);

            //modelBuilder.Entity<Course>()
            //    .HasMany(c => c.CourseMaterials)
            //    .WithMany(m => m.CourseTracks);


            // Course - Quiz Relationship
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Course)
                .WithMany(c => c.Quizzes)
                .HasForeignKey(q => q.CourseID)
                .OnDelete(DeleteBehavior.Cascade);



            // Quiz - Question Relationship
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(quiz => quiz.Questions)
                .HasForeignKey(q => q.QuizID)
                .OnDelete(DeleteBehavior.Cascade);



            // Student - QuizResult Relationship
            modelBuilder.Entity<QuizResult>()
                .HasOne(qr => qr.Student)
                .WithMany(s => s.QuizResults)
                .HasForeignKey(qr => qr.StudentID)
                .OnDelete(DeleteBehavior.Cascade);



            // Quiz - QuizResult Relationship
            modelBuilder.Entity<QuizResult>()
                .HasOne(qr => qr.Quiz)
                .WithMany(q => q.QuizResults)
                .HasForeignKey(qr => qr.QuizID)
                .OnDelete(DeleteBehavior.Cascade);



            // Course - Project Relationship
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CourseID)
                .OnDelete(DeleteBehavior.Cascade);



            // Student - Recommendation Relationship
            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Recommendations)
                .HasForeignKey(r => r.StudentID)
                .OnDelete(DeleteBehavior.Cascade);


            // LearningMaterial - CompletedMaterial Relationship
            modelBuilder.Entity<CompletedMaterial>()
                .HasOne(cm => cm.Material)
                .WithMany(lm => lm.CompletedMaterials)
                .HasForeignKey(cm => cm.MaterialID)
                .OnDelete(DeleteBehavior.Cascade); // Changed from Cascade to Restrict


            // Fix for CompletedMaterial relationships:
            modelBuilder.Entity<CompletedMaterial>()
                .HasOne(cm => cm.Enrollment)
                .WithMany(e => e.CompletedMaterials)
                .HasForeignKey(cm => cm.EnrollmentID)
                .OnDelete(DeleteBehavior.Cascade); // Changed from Cascade to Restrict
        }
    }
}