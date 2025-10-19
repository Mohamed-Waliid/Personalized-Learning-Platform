
using Graduation_Project.Models;
using Graduation_Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Graduation_Project.Repositories;
using Graduation_Project.Repositories.Interfaces;

namespace Graduation_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>();

            builder.Services.AddScoped<IQuizRepo, QuizRepo>();
            builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
            builder.Services.AddScoped<ITrackRepo, TrackRepo>();    
            builder.Services.AddScoped<ICourseRepo, CourseRepo>();
            builder.Services.AddScoped<IQuestionRepo, QuestionRepo>();
            builder.Services.AddScoped<IEnrollmentRepo, EnrollmentRepo>();
            builder.Services.AddScoped<ICourseTrackRepo, CourseTrackRepo>();
            builder.Services.AddScoped<IRecommendationRepo, RecommendationRepo>();
            builder.Services.AddScoped<ILearningMaterialRepo, LearningMaterialRepo>();
            builder.Services.AddScoped<ICourseMaterialRepo, CourseMaterialRepo>();
            builder.Services.AddScoped<ICompletedMaterialRepo, CompletedMaterialRepo>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
