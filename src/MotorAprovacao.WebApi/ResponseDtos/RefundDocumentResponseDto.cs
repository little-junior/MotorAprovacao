using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using System.Globalization;

namespace MotorAprovacao.WebApi.ResponseDtos
{
    public class RefundDocumentResponseDto
    {
        public RefundDocumentResponseDto(RefundDocument document) 
        {
            Id = document.Id;
            //To-do: pegar culture do app.settings
            Total = document.Total.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            //To-do: implementar o recebimento do nome da categoria atraves do id
            Category = document.CategoryId;
            Description = document.Description;
            Status = Enum.GetName(document.Status);
            CreatedAt = document.CreatedAt.ToString("F", CultureInfo.CreateSpecificCulture("pt-BR"));
            StatusDeterminedAt = 
                document.StatusDeterminedAt == DateTime.MinValue ? 
                "N/A" : 
                document.StatusDeterminedAt.ToString("F", CultureInfo.CreateSpecificCulture("pt-BR"));
        }


        public Guid Id { get; set; }
        public string Total { get; set; }
        public int Category { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }
        public string StatusDeterminedAt { get; set; }
    }
}
