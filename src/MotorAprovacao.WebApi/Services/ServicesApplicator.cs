using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.WebApi.AuthServices;
using MotorAprovacao.WebApi.RequestDtos;
using System.Reflection;
using System.Text;

namespace MotorAprovacao.WebApi.Services
{
    public class ServicesApplicator
    {
        private readonly WebApplicationBuilder _builder;

        public ServicesApplicator(WebApplicationBuilder builder)
        {
            _builder = builder;
        }

        public void AddConfigure()
        {
            _builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });
        }

        public void AddValidatorsServices()
        {
            _builder.Services.AddScoped<IValidator<RefundDocumentRequestDto>, RefundDocumentValidator>();
            _builder.Services.AddFluentValidationAutoValidation();
        }

        public void AddApplicationServices()
        {
            _builder.Services.AddScoped<IRefundDocumentRepository, RefundDocumentRepository>();
            _builder.Services.AddScoped<IRefundDocumentService, RefundDocumentService>();
            _builder.Services.AddScoped<IApprovalEngine, ApprovalEngine>();
            _builder.Services.AddScoped<ICategoryRulesRepository, CategoryRulesRepository>();
            _builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            _builder.Services.AddScoped<ITokenService, TokenService>();
        }

        public void AddAuthorization()
        {
            _builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Gestor"));
                options.AddPolicy("TraineeOnly", policy => policy.RequireRole("Estagiario"));
                options.AddPolicy("AllRoles", policy =>
                {
                    policy.RequireRole("Gestor", "Estagiario");
                });
            });
        }

        public void AddAuthentication()
        {
            var secretKey = _builder.Configuration.GetSection("JWT:SecretKey").Value;
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentException("Invalid secret key!");
            }

            _builder.Services.AddAuthentication(options =>
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
                    ValidAudience = _builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = _builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                                            Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public void AddCors(string origin)
        {
            _builder.Services.AddCors(options =>
                options.AddPolicy(name: origin,
                policy =>
                {
                    policy.WithOrigins("https://www.me.com.br/").
                                                AllowAnyMethod();
                })
            );
        }

        public void AddSwaggerGen()
        {
            _builder.Services.AddSwaggerGen(x =>
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
                    Type = SecuritySchemeType.Http,
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
                        Array.Empty<string>()
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        public void AddIdentity()
        {
            _builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();
        }
    }
}
