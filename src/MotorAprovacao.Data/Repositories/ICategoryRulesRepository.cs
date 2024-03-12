using MotorAprovacao.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Data.Repositories
{
    public interface ICategoryRulesRepository
    {
        Task<CategoryRules> GetById(int id);
    }
}
