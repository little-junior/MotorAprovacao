using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MotorAprovacao.WebApi.AuthContextMock
{
    public class AppDbContextMock : IdentityDbContext
    {
        public AppDbContextMock(DbContextOptions<AppDbContextMock> options) : base(options) { } 
    }
}
