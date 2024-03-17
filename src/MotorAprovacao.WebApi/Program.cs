using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotorAprovacao.WebApi.AuthServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using MotorAprovacao.WebApi.Services;
using MotorAprovacao.Models.Entities;
using MotorAprovacao.WebApi.ErrorHandlers;
using MotorAprovacao.WebApi.Filters;
using System.Reflection;
using Microsoft.AspNetCore.HttpLogging;

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

            builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.All;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            servicesApplicator.AddApplicationServices();

            //default Swagger configuration for JWT utilization
            builder.Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MotorAprovacao API",
                    Description = "API para o Desafio \"Motor de Aprovação - DiverseDev\"",
                    Contact = new OpenApiContact
                    {
                        Name = "Repository",
                        Url = new Uri("https://github.com/little-junior/MotorAprovacao")
                    }
                });

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

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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

            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseHttpLogging();

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
