using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.Models.Entities;

namespace MotorAprovacao.WebApi.Services
{
    public class ApprovalEngine : IApprovalEngine
    {
        private readonly ICategoryRulesRepository _repository;

        public ApprovalEngine(ICategoryRulesRepository repository)
        {
            _repository = repository;
        }

        const int OutrosCategoryId = 1;

        public async Task ProcessDocument(RefundDocument document)
        {
            decimal maxApprovalCategory, minDisapprovalCategory;

            var categoryRules = await _repository.GetById(document.CategoryId);

            if (categoryRules != null)
            {
                maxApprovalCategory = categoryRules.MaximumToApprove;
                minDisapprovalCategory = categoryRules.MinimumToDisapprove;
            }
            else
            {
                var defaultCategoryRules = await _repository.GetById(OutrosCategoryId);
                maxApprovalCategory = defaultCategoryRules.MaximumToApprove;
                minDisapprovalCategory = defaultCategoryRules.MinimumToDisapprove;
            }

            decimal refundTotal = document.Total;

            if (refundTotal <= maxApprovalCategory)
            {
                document.Approve();
            }
            else if (refundTotal > minDisapprovalCategory)
            {
                document.Disapprove();
            }
            else
            {
                document.PutOnApproval();
            }
        }
    }
}