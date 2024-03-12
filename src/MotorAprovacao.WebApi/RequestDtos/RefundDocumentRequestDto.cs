using System.ComponentModel.DataAnnotations;

namespace MotorAprovacao.WebApi.RequestDtos
{
    public class RefundDocumentRequestDto
    {
        [Required]
        public decimal Total { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; }

    }
}
