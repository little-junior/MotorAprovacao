using MotorAprovacao.Data.EF;
using MotorAprovacao.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Tests
{
    public class TestDataSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            context.Categories.AddAsync(new Category(1, "Outros"));
            context.Categories.AddAsync(new Category(2, "Hospedagem"));
            context.Categories.AddAsync(new Category(3, "Transporte"));

            context.Rules.AddAsync(new CategoryRules(1, 1, 100m, 1000m));
            context.Rules.AddAsync(new CategoryRules(2, 2, 500m, 1000m));
            context.Rules.AddAsync(new CategoryRules(3, 3, 500m, 1000m));


            context.SaveChanges();
        }

    }
}
