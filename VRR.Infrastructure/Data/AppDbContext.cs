using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VRR.Domain.Entities;

namespace VRR.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Resident> Residents => Set<Resident>();
    public DbSet<EmergencyEvent> EmergencyEvents => Set<EmergencyEvent>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Resident>()
            .Property(r => r.RiskScore)
            .HasDefaultValue(0);

        modelBuilder.Entity<CheckIn>()
            .HasOne(c => c.Resident)
            .WithMany(r => r.CheckIns)
            .HasForeignKey(c => c.ResidentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CheckIn>()
            .HasOne(c => c.EmergencyEvent)
            .WithMany(e => e.CheckIns)
            .HasForeignKey(c => c.EmergencyEventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}