using MotorAprovacao.Models.Entities;

namespace MotorAprovacao.WebApi.Services
{
    public class ApprovalEngine : IApprovalEngine
    {
        private readonly AppDbContext _context;

        public ApprovalEngine(AppDbContext context)
        {
            _context = context;
        }

        public async Task ProcessDocument(RefundDocument document)
        {
            decimal maxApprovalCategory, minDisapprovalCategory;

            var categoryRules = await _context.Rules.FirstOrDefaultAsync(rule => rule.CategoryId == document.CategoryId);

            if (categoryRules != null)
            {
                maxApprovalCategory = categoryRules.MaximumApproval;
                minDisapprovalCategory = categoryRules.MinimalDisapproval;
            }
            else
            {
                var defaultCategoryRules = await _context.Rules.FirstOrDefaultAsync(rule => rule.CategoryId == 1);
                maxApprovalCategory = defaultCategoryRules.MaximumApproval;
                minDisapprovalCategory = defaultCategoryRules.MinimalDisapproval;
            }

            decimal requestTotal = document.Total;

            if (requestTotal <= maxApprovalCategory)
            {
                document.Approve();
                return;
            }

            if (requestTotal > minDisapprovalCategory)
            {
                document.Disapprove();
                return;
            }
        }
    }
}