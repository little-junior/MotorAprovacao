using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Data.Models
{
    public interface IRefundRepository
    {
        Task<RefundDocument> GetById(Guid id);
        Task<IEnumerable<RefundDocument>> GetByStatus(Status status);
        Task Add(RefundDocument document);
        Task Update(RefundDocument document);
        Task<bool> SaveChanges();
    }
}
