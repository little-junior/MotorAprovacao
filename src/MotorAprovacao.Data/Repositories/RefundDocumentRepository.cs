using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Data.Repositories
{
    public class RefundDocumentRepository : IRefundDocumentRepository
    {
        private readonly AppDbContext _context;

        public RefundDocumentRepository(AppDbContext context)
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
