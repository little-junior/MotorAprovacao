using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Models.Entities;

namespace MotorAprovacao.Data.EF;

public class AppDbContext : DbContext
{
    const int MaxCharsByDocumentoDescription = 200;
    const int MaxCharsByCategory = 70;
    const int MaxIntByAprovacao = 10000;
    const int MinIntByReprovacao = 10000;


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<RefundDocument> RefundDocuments { get; set; }

    public DbSet<CategoryRules> Rules { get; set; }
    public DbSet<Category> Categories { get; set; }



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
            builder.Property(s => s.StatusDeterminedAt).IsRequired().ValueGeneratedOnUpdate();
        });

        modelBuilder.Entity<CategoryRules>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.HasOne(s => s.Category)
                .WithOne(r => r.CategoryRules)
                .IsRequired()
                .HasForeignKey<CategoryRules>(s => s.CategoryId);
            builder.Property(s => s.MaximumToApprove).HasMaxLength(MaxIntByAprovacao);
            builder.Property(s => s.MinimumToDisapprove).HasMaxLength(MinIntByReprovacao);
            builder.HasData(new List<CategoryRules>()
            {
                new CategoryRules(1, 1, 100m, 1000m),
                new CategoryRules(2, 2, 500m, 1000m),
                new CategoryRules(3, 3, 500m, 1000m)
            });
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.HasOne(s => s.CategoryRules)
                .WithOne(r => r.Category)
                .IsRequired();
            builder.Property(s => s.Name).HasMaxLength(MaxCharsByCategory);
            builder.HasData(new List<Category>()
                {
                    new Category(1, "Outros"),
                    new Category(2, "Alimentação"),
                    new Category(3, "Transporte")
                });
        });
    }

}