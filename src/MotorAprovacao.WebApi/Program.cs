
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotorAprovacao.Models.AuthModels;
using MotorAprovacao.WebApi.AuthContextMock;
using MotorAprovacao.WebApi.AuthServices;
using System.Net;
using System.Text;

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
                            AddEntityFrameworkStores<AppDbContextMock>
                            ().AddDefaultTokenProviders();


            //Configuração de autenticação 
            var secretKey = builder.Configuration["JWT:SecretKey"]
                            ?? throw new ArgumentException("Invalid secret key!");

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
