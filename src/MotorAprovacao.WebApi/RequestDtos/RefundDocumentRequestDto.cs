using System.ComponentModel.DataAnnotations;

namespace MotorAprovacao.WebApi.RequestDtos
{
    public class RefundDocumentRequestDto
    {
        /// <summary>
        /// The total value of refund
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Id of a Category
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Description of the document
        /// </summary>
        public string Description { get; set; }
    }
}
