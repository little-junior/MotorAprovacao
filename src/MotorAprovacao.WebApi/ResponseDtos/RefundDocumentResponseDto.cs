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
            Total = document.Total.ToString("C", CultureInfo.InvariantCulture);
            Category = document.Category.Name;
            Description = document.Description;
            Status = Enum.GetName(document.Status)!;
            CreatedAt = document.CreatedAt.ToString("F", CultureInfo.InvariantCulture);
            StatusDeterminedAt = document.StatusDeterminedAt == null ? "N/A" : document.StatusDeterminedAt?.ToString("F", CultureInfo.InvariantCulture)!;
        }


        public Guid Id { get; set; }
        public string Total { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }
        public string StatusDeterminedAt { get; set; }
    }
}
