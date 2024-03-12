using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Data.Repositories
{
    public class CategoryRulesRepository : ICategoryRulesRepository
    {
        private readonly AppDbContext _context;

        public CategoryRulesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryRules> GetById(int id)
        {
            var rule = await _context.Rules.FirstOrDefaultAsync(x => x.Id == id);

            return rule;
        }
    }
}
