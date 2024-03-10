using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;

namespace MotorAprovacao.Data.Models
{

    public class RefundRepository : IRefundRepository
    {
        private readonly AppDbContext _context;

        public RefundRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(RefundDocument document)
        {
            await _context.RefundDocuments.AddAsync(document);
        }

        public async Task<RefundDocument> GetById(Guid id)
        {
            var document = await _context.RefundDocuments.SingleOrDefaultAsync(document => document.Id == id);

            return document;
        }

        public async Task<IEnumerable<RefundDocument>> GetByStatus(Status status)
        {
            var documents = await Task.FromResult(_context.RefundDocuments.Where(document => document.Status == status));

            return documents;
        }

        public async Task Update(RefundDocument document)
        {
            await Task.Run(() => _context.RefundDocuments.Update(document));
        }

        public async Task<bool> SaveChanges()
        {
            var saveChangesResult = await _context.SaveChangesAsync();

            return saveChangesResult > 0;
        }
    }
}