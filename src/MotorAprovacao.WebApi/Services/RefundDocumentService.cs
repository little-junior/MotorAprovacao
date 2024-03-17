using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using MotorAprovacao.WebApi.ErrorHandlers;
using MotorAprovacao.WebApi.RequestDtos;

namespace MotorAprovacao.WebApi.Services
{
    public class RefundDocumentService : IRefundDocumentService
    {
        private readonly IApprovalEngine _approvalEngine;
        private readonly IRefundDocumentRepository _refundDocumentrepository;
        private readonly ICategoryRepository _categoryRepository;

        public RefundDocumentService(IApprovalEngine approvalEngine, IRefundDocumentRepository repository, ICategoryRepository categoryRepository)
        {
            _approvalEngine = approvalEngine;
            _refundDocumentrepository = repository;
            _categoryRepository = categoryRepository;
        }

        public async Task<DefaultResult> ApproveDocument(Guid id)
        {
            var document = await _refundDocumentrepository.GetById(id);

            if (document == null)
            {
                var errorResponse = new ErrorResponse(404, "Not Found", "Document not found.");
                return new DefaultResult(new NotFoundObjectResult(errorResponse));
            }

            if (document.Status != Status.OnApproval)
            {
                var errorResponse = new ErrorResponse(409, "Conflict", "Document can only be approved while in the 'OnApproval' state.");
                return new DefaultResult(new ConflictObjectResult(errorResponse));
            }

            document.Approve();

            await _refundDocumentrepository.Update(document);
            await _refundDocumentrepository.SaveChanges();

            return new DefaultResult();
        }

        public async Task<DefaultResult<RefundDocument>> CreateDocument(RefundDocumentRequestDto documentDto)
        {
            var categoryExistence = await _categoryRepository.CheckExistenceById(documentDto.CategoryId);

            if (!categoryExistence)
            {
                var errorResponse = new ErrorResponse(400, "Bad Request", $"The value {documentDto.CategoryId} of field 'categoryId' is invalid.");
                return new DefaultResult<RefundDocument>(new BadRequestObjectResult(errorResponse));
            }

            var generatedDocument = GenerateRequestDocument(documentDto);

            await _approvalEngine.ProcessDocument(generatedDocument);

            await _refundDocumentrepository.Add(generatedDocument);
            await _refundDocumentrepository.SaveChanges();

            return new DefaultResult<RefundDocument>(generatedDocument);
        }

        public async Task<DefaultResult> DisapproveDocument(Guid id)
        {
            var document = await _refundDocumentrepository.GetById(id);

            if (document == null)
            {
                var errorResponse = new ErrorResponse(404, "Not Found", "Document not found.");
                return new DefaultResult(new NotFoundObjectResult(errorResponse));
            }

            if (document.Status != Status.OnApproval)
            {
                var errorResponse = new ErrorResponse(409, "Conflict", "Document can only be disapproved while in the 'OnApproval' state.");
                return new DefaultResult(new ConflictObjectResult(errorResponse));
            }

            document.Disapprove();

            await _refundDocumentrepository.Update(document);
            await _refundDocumentrepository.SaveChanges();

            return new DefaultResult();
        }

        public async Task<DefaultResult<RefundDocument>> GetDocumentById(Guid id)
        {
            var document = await _refundDocumentrepository.GetById(id);

            if (document == null)
            {
                var errorResponse = new ErrorResponse(404, "Not Found", "Document not found.");
                return new DefaultResult<RefundDocument>(new NotFoundObjectResult(errorResponse));
            }

            return new DefaultResult<RefundDocument>(document);
        }

        public async Task<IEnumerable<RefundDocument>> GetDocumentsByStatus(Status status)
        {
            var documents = await _refundDocumentrepository.GetByStatus(status);

            return documents;
        }

        private RefundDocument GenerateRequestDocument(RefundDocumentRequestDto documentDto)
        {
            return new RefundDocument(documentDto.Total, documentDto.CategoryId, documentDto.Description);
        }
    }
}
