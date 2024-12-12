using HealthNoter.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthNoter.DataAccess.Postgresql;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PressureNoteEntity> PressureNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("uuid");

            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.RegisteredAt).IsRequired();

            builder.HasIndex(x => x.Username).IsUnique();

            builder
                .HasMany(x => x.PressureNotes)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        });
        
        modelBuilder.Entity<PressureNoteEntity>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("uuid");

            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.Sys).IsRequired();
            builder.Property(x => x.Dia).IsRequired();
            builder.Property(x => x.Pulse).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.PressureNotes)
                .HasForeignKey(x => x.UserId);
        });
    }
}