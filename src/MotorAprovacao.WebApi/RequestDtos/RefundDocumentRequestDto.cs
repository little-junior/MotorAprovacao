using System.ComponentModel.DataAnnotations;

namespace MotorAprovacao.WebApi.RequestDtos
{
    public class RefundDocumentRequestDto
    {
        [Required(ErrorMessage = "The field 'total' is required.")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "The field 'categoryId' is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The field 'description' is required.")]
        [StringLength(200, ErrorMessage ="The field must be under 200 characters.")]
        public string Description { get; set; }
    }
}
