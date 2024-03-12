using MotorAprovacao.Data.Repositories;
using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using MotorAprovacao.WebApi.RequestDtos;

namespace MotorAprovacao.WebApi.Services
{
    public class RefundDocumentService : IRefundDocumentService
    {
        private readonly IApprovalEngine _approvalEngine;
        private readonly IRefundDocumentRepository _repository;

        public RefundDocumentService(IApprovalEngine approvalEngine, IRefundDocumentRepository repository)
        {
            _approvalEngine = approvalEngine;
            _repository = repository;
        }

        public async Task ApproveDocument(Guid id)
        {
            var document = await _repository.GetById(id);

            document.Approve();

            await _repository.Update(document);
            await _repository.SaveChanges();
        }

        public async Task<RefundDocument> CreateDocument(RefundDocumentRequestDto documentDto)
        {
            var generatedDocument = GenerateRequestDocument(documentDto);

            await _approvalEngine.ProcessDocument(generatedDocument);

            await _repository.Add(generatedDocument);
            await _repository.SaveChanges();

            return generatedDocument;
        }

        public async Task DisapproveDocument(Guid id)
        {
            var document = await _repository.GetById(id);

            document.Disapprove();

            await _repository.Update(document);
            await _repository.SaveChanges();
        }

        public async Task<IEnumerable<RefundDocument>> GetDocumentsByStatus(Status status)
        {
            var documents = await _repository.GetByStatus(status);

            return documents;
        }

        private RefundDocument GenerateRequestDocument(RefundDocumentRequestDto documentDto)
        {
            return new RefundDocument(documentDto.Total, documentDto.CategoryId, documentDto.Description);
        }
    }
}
