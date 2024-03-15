using System.ComponentModel.DataAnnotations;

namespace MotorAprovacao.WebApi.RequestDtos
{
    public class RefundDocumentRequestDto
    {
        public decimal Total { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }
    }
}
