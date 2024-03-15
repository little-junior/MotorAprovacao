
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotorAprovacao.WebApi.AuthServices;
using System.Net;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.WebApi.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MotorAprovacao.Models.Entities;
using Serilog;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MotorAprovacao.WebApi;
using MotorAprovacao.WebApi.Logging;

namespace MotorAprovacao.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //implementação logging com dB
            var config = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();

            builder.Host.UseSerilog((context, configuration) => configuration.
                                                                ReadFrom.
                                                                Configuration(context.Configuration));



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
            builder.Services.AddScoped<ICategoryRulesRepository, CategoryRulesRepository>();


            //default Swagger configuration for JWT utilization
            builder.Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "motorAprovação", Version = "v1" });

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Bearer JWT"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //Injection suggestion required for partial delivery day 12 corrigir implementação dos using
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
                            AddEntityFrameworkStores<AppDbContext>
                            ().AddDefaultTokenProviders();


            //Configuração de autenticação 
            var secretKey = builder.Configuration.GetSection("JWT:SecretKey").Value;
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentException("Invalid secret key!");
            }


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                                            Encoding.UTF8.GetBytes(secretKey))

                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Gerente"));
                options.AddPolicy("TraineeOnly", policy => policy.RequireRole("Estagiário(a)"));
                //options.AddPolicy("AdminOnly", policy=> policy.RequireRole("Administrador").RequireClaim("id" "ME"))
            });
            builder.Services.AddScoped<ITokenService, TokenService>();
            
            var app = builder.Build();

            var loggerFactory = builder.Services?.BuildServiceProvider().GetRequiredService<ILoggerFactory>();

            app.UseCustomLog(loggerFactory, builder.Configuration);

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
