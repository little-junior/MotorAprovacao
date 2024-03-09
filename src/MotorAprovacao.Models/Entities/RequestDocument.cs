using MotorAprovacao.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Models.Entities
{
    public class RequestDocument
    {
        public RequestDocument() { }

        public RequestDocument(decimal total, int categoryId, string description) 
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Total = total;
            CategoryId = categoryId;
            Description = description;
        }

        //To do: implementação do responsável pela criação e determinação do status
        public Guid Id { get; private set; }
        public decimal Total { get; private set; }
        public int CategoryId { get; private set; }
        public Category? Category { get; private set; }
        public string Description { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime StatusDeterminedAt { get; private set; }

        public void Approve()
        {
            Status = Status.Approved;
            SetStatusDeterminedAt();
        }

        public void Disapprove()
        {
            Status = Status.Disapproved;
            SetStatusDeterminedAt();
        }

        public void PutOnApproval()
        {
            Status = Status.OnApproval;
            SetStatusDeterminedAt();
        }

        private void SetStatusDeterminedAt()
        {
            StatusDeterminedAt = DateTime.Now;
        }
    }
}
