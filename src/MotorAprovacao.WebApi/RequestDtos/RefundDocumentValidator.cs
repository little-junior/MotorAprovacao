using FluentValidation;

namespace MotorAprovacao.WebApi.RequestDtos
{
    public class RefundDocumentValidator : AbstractValidator<RefundDocumentRequestDto>
    {
        public RefundDocumentValidator() 
        {
            RuleFor(model => model.CategoryId)
                .NotEmpty()
                .GreaterThan(0);
                

            RuleFor(model => model.Total)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(model => model.Description)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
