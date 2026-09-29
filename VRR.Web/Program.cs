using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VRR.Application.Interfaces;
using VRR.Application.Services;
using VRR.Infrastructure.Data;
using VRR.Domain.Entities;
using VRR.Infrastructure.Services;

namespace VulnerableResidentRegistry
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IAuditService, AuditService>();
            builder.Services.AddScoped<IEmergencyDeclarationService, EmergencyDeclarationService>();
            builder.Services.AddScoped<IRiskScoringService, RiskScoringService>();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            var app = builder.Build();

            // Seed roles and default admin account
            using (var scope = app.Services.CreateScope())
            {
                await VRR.Infrastructure.Data.Seed.IdentitySeeder.SeedAsync(
                    scope.ServiceProvider, builder.Configuration);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages();

            app.Run();
        }
    }
}