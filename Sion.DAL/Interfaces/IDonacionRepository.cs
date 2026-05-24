using Sion.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.DAL.Interfaces
{
    public interface IDonacionRepository : IRepository<Donacion>
    {
        Task<IEnumerable<Donacion>> GetByFechasAsync(DateTime desde, DateTime hasta);
    }
}
