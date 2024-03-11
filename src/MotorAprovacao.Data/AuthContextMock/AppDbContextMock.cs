using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MotorAprovacao.WebApi.AuthContextMock
{
    
    public class AppDbContextMock : IdentityDbContext<ApplicationUser>
    {
        public AppDbContextMock(DbContextOptions<AppDbContextMock> options) : base(options) { } 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
   
}
