using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using MotorAprovacao.WebApi.RequestDtos;

namespace MotorAprovacao.WebApi.Services
{
    public interface IRefundDocumentService
    {
        Task<RefundDocument> GetDocumentById(Guid id);
        Task<IEnumerable<RefundDocument>> GetDocumentsByStatus(Status status);
        Task<RefundDocument> CreateDocument(RefundDocumentRequestDto documentDto);
        Task ApproveDocument(Guid id);
        Task DisapproveDocument(Guid id);
    }
}
