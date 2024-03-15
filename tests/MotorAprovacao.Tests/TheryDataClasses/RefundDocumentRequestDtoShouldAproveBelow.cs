using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Models.Entities.TheryDataClasses
{
    internal class RefundDocumentRequestDtoShouldAproveBelow : TheoryData<decimal, int, string>
    {
        public RefundDocumentRequestDtoShouldAproveBelow()
        {
            // Contem somente aqueles que devem ser aprovados
            Add(50m, 1, "Deve ser aprovado");
            Add(100m, 2, "Deve ser aprovado");
            Add(25.50m, 3, "Deve ser aprovado");
        }
    }

    internal class RefundDocumentRequestDtoShouldAproveAbove : TheoryData<decimal, int, string>
    {
        public RefundDocumentRequestDtoShouldAproveAbove()
        {
            // Contem somente aqueles que devem ser aprovados
            Add(700m, 1, "Deve ser aprovado pelo document service");
            Add(700m, 2, "Deve ser aprovado pelo document service");
            Add(700m, 3, "Deve ser aprovado pelo document service");
        }
    }


    internal class RefundDocumentRequestDtoShouldNotAproveAbove : TheoryData<decimal, int, string>
    {
        public RefundDocumentRequestDtoShouldNotAproveAbove()
        {
            // Contem somente aqueles que devem ser aprovados
            Add(1001m, 1, "Não ser aprovado");
            Add(1002m, 2, "Não ser aprovado");
            Add(1003m, 3, "Não ser aprovado");
        }
    }
    internal class RefundDocumentRequestDtoShouldNotAproveLaw : TheoryData<decimal, int, string>
    {
        public RefundDocumentRequestDtoShouldNotAproveLaw()
        {
            // Contem somente aqueles que devem ser aprovados
            Add(1001m, 1, "Não ser aprovado");
            Add(1002m, 2, "Não ser aprovado");
            Add(1003m, 3, "Não ser aprovado");
        }
    }
}
