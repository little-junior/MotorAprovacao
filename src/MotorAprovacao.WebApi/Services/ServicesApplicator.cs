using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.WebApi.RequestDtos;

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
        }
    }
}
