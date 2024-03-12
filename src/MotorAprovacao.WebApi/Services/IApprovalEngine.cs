using MotorAprovacao.Models.Entities;

namespace MotorAprovacao.WebApi.Services
{
    public interface IApprovalEngine
    {
        Task ProcessDocument(RefundDocument document);
    }
}