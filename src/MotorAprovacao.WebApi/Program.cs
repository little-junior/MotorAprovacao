
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.WebApi.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MotorAprovacao.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            builder.Services.AddScoped<IRefundDocumentRepository, RefundDocumentRepository>();
            builder.Services.AddScoped<IRefundDocumentService,  RefundDocumentService>();
            builder.Services.AddScoped<IApprovalEngine,  ApprovalEngine>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
