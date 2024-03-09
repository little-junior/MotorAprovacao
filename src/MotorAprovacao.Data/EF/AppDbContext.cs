using Microsoft.EntityFrameworkCore;

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

    public DbSet<DocumentoReembolso> ReturnDocuments { get; set; }

    public DbSet<RegrasCategoria> Rules { get; set; }
    public DbSet<Approver> Approvers { get; set; }
    public DbSet<Category> Categories { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentoReembolso>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Descricao).HasMaxLength(MaxCharsByDocumentoDescription);
            builder.Property(s => s.Categoria).HasMaxLength(MaxCharsByCategory);
            builder.Property(s => s.Status).IsRequired();
            builder.Property(s => s.Total).IsRequired().HasPrecision(5);

            builder.HasOne(s => Approvers);
        });

        modelBuilder.Entity<RegrasCategoria>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Categoria).HasMaxLength(MaxCharsByCategory);
            builder.Property(s => s.MaximoAprovacao).HasMaxLength(MaxIntByAprovacao);
            builder.Property(s => s.MinimoReprovacao).HasMaxLength(MinIntByReprovacao);

        });

        modelBuilder.Entity<Approver>(builder =>
        {
            builder.HasKey(s => s.ApproverId);
            builder.Property(s => s.ApproverName).HasMaxLength(MaxCharsByAprovadorName);
            builder.Property(s => s.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder.HasKey(s => s.CategoryId);
            builder.Property(s => s.CategoryName).HasMaxLength(MaxCharsByAprovadorName);
            builder.Property(s => s.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        });
    }

}