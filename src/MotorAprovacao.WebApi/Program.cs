using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotorAprovacao.WebApi.AuthServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using MotorAprovacao.WebApi.Services;
using MotorAprovacao.WebApi.ErrorHandlers;
using MotorAprovacao.WebApi.Filters;
using System.Reflection;
using MotorAprovacao.WebApi.Middlewares;

namespace MotorAprovacao.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var servicesApplicator = new ServicesApplicator(builder);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationActionFilter>();
            });

            servicesApplicator.AddConfigure();

            servicesApplicator.AddValidatorsServices();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            servicesApplicator.AddSwaggerGen();

            var AllowedOrigins = "_allowedOrigins";
            servicesApplicator.AddCors(AllowedOrigins);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            servicesApplicator.AddApplicationServices();

            servicesApplicator.AddIdentity();

            servicesApplicator.AddAuthentication();
            servicesApplicator.AddAuthorization();

            var app = builder.Build();

            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseMiddleware<CustomHttpLoggingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(AllowedOrigins);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
