using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Data.Repositories
{
    public interface ICategoryRepository
    {
        Task<bool> CheckExistenceById(int id);
    }
}
