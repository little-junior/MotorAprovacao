using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MotorAprovacao.Data.Models.RefundRepository;

namespace MotorAprovacao.Data.Models
{

    public class RefundRepository : IRequestDocumentRepository
    {
        private readonly MyContext _context;

        public RequestDocumentRepository(MyContext context)
        {
            _context = context;
        }

        public async Task Add(RequestDocument document)
        {
            await _context.RequestDocuments.AddAsync(document);
        }

        public async Task<RequestDocument> GetById(Guid id)
        {
            var document = await _context.RequestDocuments.SingleOrDefaultAsync(document => document.Id == id);

            return document;
        }

        public async Task<IEnumerable<RequestDocument>> GetByStatus(Status status)
        {
            var documents = await Task.FromResult(_context.RequestDocuments.Where(document => document.Status == status));

            return documents;
        }

        public async Task Update(RequestDocument document)
        {
            await Task.Run(() => _context.RequestDocuments.Update(document));
        }

        public async Task<bool> SaveChanges()
        {
            var saveChangesResult = await _context.SaveChangesAsync();

            return saveChangesResult > 0;
        }
    }
