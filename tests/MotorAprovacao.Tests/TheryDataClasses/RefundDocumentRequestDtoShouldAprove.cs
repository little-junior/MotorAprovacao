using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Models.Entities.TheryDataClasses
{
    internal class RefundDocumentRequestDtoShouldAprove : TheoryData<decimal, int, string>
    {
        public RefundDocumentRequestDtoShouldAprove()
        {
            // Contem somente aqueles que devem ser aprovados
            Add(50m, 1, "Deve ser aprovado");
            Add(100m, 2, "Deve ser aprovado");
            Add(25.50m, 3, "Deve ser aprovado");
        }
    }
    internal class RefundDocumentRequestDtoShouldNotAprove : TheoryData<decimal, int, string>
    {
        public RefundDocumentRequestDtoShouldNotAprove()
        {
            // Contem somente aqueles que devem ser aprovados
            Add(1001m, 1, "Não ser aprovado");
            Add(1002m, 2, "Não ser aprovado");
            Add(1003m, 3, "Não ser aprovado");
        }
    }
}
