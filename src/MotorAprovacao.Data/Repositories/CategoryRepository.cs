using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistenceById(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
    }
}
