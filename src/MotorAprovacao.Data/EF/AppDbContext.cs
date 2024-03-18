using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Models.Entities;

using System.Reflection.Emit;

namespace MotorAprovacao.Data.EF;

public class AppDbContext : IdentityDbContext 
{
    const int MaxCharsByDocumentoDescription = 200;
    const int MaxCharsByCategory = 70;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<RefundDocument> RefundDocuments { get; set; }
    public DbSet<CategoryRules> Rules { get; set; }
    public DbSet<Category> Categories { get; set; }
    //public DbSet<IdentityUser> User { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefundDocument>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Description).HasMaxLength(MaxCharsByDocumentoDescription);
            builder.HasOne(s => s.Category);
            builder.Property(s => s.Status).IsRequired();
            builder.Property(s => s.Total).IsRequired().HasPrecision(5);
            builder.Property(s => s.CreatedAt).IsRequired().ValueGeneratedOnAdd();
            builder.Property(s => s.StatusDeterminedAt);
        });

        modelBuilder.Entity<IdentityUser>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.UserName);
            builder.Property(s => s.Email);
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.HasOne(s => s.CategoryRules)
                .WithOne(r => r.Category);
            builder.Property(s => s.Name).HasMaxLength(MaxCharsByCategory);
            builder.HasData(new List<Category>()
            {
                new Category(1, "Outros"),
                new Category(2, "Alimentacao"),
                new Category(3, "Transporte"),
                new Category(4, "Hospedagem"),
                new Category(5, "Viagem"),
            });
        });

        modelBuilder.Entity<CategoryRules>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.HasOne(s => s.Category)
                .WithOne(r => r.CategoryRules)
                .IsRequired()
                .HasForeignKey<CategoryRules>(s => s.CategoryId);
            builder.Property(s => s.MaximumToApprove);
            builder.Property(s => s.MinimumToDisapprove);

            builder.HasData(new List<CategoryRules>()
            {
                new CategoryRules(1, 1, 100m, 1000m),
                new CategoryRules(2, 2, 500m, 1000m),
                new CategoryRules(3, 3, 500m, 1000m),
            });
        });

        modelBuilder.Entity<IdentityUser>(builder =>
        {
            builder.Property<string>("RefreshToken");
            builder.Property<DateTime>("RefreshTokenExpiryTime");
        }
        );
         


        base.OnModelCreating(modelBuilder);

    }
};