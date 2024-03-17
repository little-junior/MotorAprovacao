using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using MotorAprovacao.WebApi.RequestDtos;

namespace MotorAprovacao.WebApi.Services
{
    public interface IRefundDocumentService
    {
        Task<DefaultResult<RefundDocument>> GetDocumentById(Guid id);
        Task<IEnumerable<RefundDocument>> GetDocumentsByStatus(Status status);
        Task<DefaultResult<RefundDocument>> CreateDocument(RefundDocumentRequestDto documentDto);
        Task<DefaultResult> ApproveDocument(Guid id);
        Task<DefaultResult> DisapproveDocument(Guid id);
    }
}
