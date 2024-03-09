using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Models.Entities
{
    public class Category
    {
        public Category() { }

        public int Id { get; set; }
        public string Name { get; private set; }
        public decimal MaximumApproval { get; private set; }
        public decimal MinimalDisapproval { get; private set; }
    }
}
