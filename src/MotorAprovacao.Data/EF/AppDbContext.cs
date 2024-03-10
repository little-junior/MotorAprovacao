using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Models.Entities;

namespace MotorAprovacao.Data.EF;

public class AppDbContext : DbContext
{
    const int MaxCharsByDocumentoDescription = 200;
    const int MaxCharsByCategory = 70;
    const int MaxCharsByAprovadorName = 40;
    const int MaxIntByAprovacao = 10000;
    const int MinIntByReprovacao = 10000;


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<RequestDocument> ReturnDocuments { get; set; }

    public DbSet<CategoryRules> Rules { get; set; }

    //public DbSet<Approver> Approvers { get; set; }
    public DbSet<Category> Categories { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequestDocument>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Description).HasMaxLength(MaxCharsByDocumentoDescription);
            builder.Property(s => s.Category).HasMaxLength(MaxCharsByCategory);
            builder.Property(s => s.Status).IsRequired();
            builder.Property(s => s.Total).IsRequired().HasPrecision(5);
            builder.Property(s => s.CreatedAt).IsRequired().ValueGeneratedOnAdd();
            builder.Property(s => s.StatusDeterminedAt).IsRequired().ValueGeneratedOnUpdate();


            //builder.HasOne(s => Approvers);
        });

        modelBuilder.Entity<CategoryRules>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Category).IsRequired();
            builder.Property(s => s.CategoryId).IsRequired();
            builder.Property(s => s.MaximumToApprove).HasMaxLength(MaxIntByAprovacao);
            builder.Property(s => s.MinimumToDisapprove).HasMaxLength(MinIntByReprovacao);

        });

        //modelBuilder.Entity<Approver>(builder =>
        //{
        //    builder.HasKey(s => s.ApproverId);
        //    builder.Property(s => s.ApproverName).HasMaxLength(MaxCharsByAprovadorName);
        //    builder.Property(s => s.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        //});

        modelBuilder.Entity<Category>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).HasMaxLength(MaxCharsByCategory);
            builder.Property(s => s.CategoryRules).IsRequired().ValueGeneratedOnAddOrUpdate();
        });
    }

}