using Microsoft.EntityFrameworkCore;
using Sion.DAL.Context;
using Sion.DAL.Entities;
using Sion.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.DAL.Repositories
{

    public class DonacionRepository : Repository<Donacion>, IDonacionRepository
    {
        public DonacionRepository(SionDbContext context) : base(context) { }

        public async Task<IEnumerable<Donacion>> GetByFechasAsync(DateTime desde, DateTime hasta)
            => await _dbSet.Where(d => d.FechaRegistro >= desde && d.FechaRegistro <= hasta).ToListAsync();

        public async Task<Donacion?> GetByPaypalIdAsync(string transaccionPaypalId)
            => await _dbSet.FirstOrDefaultAsync(d => d.TransaccionPaypalId == transaccionPaypalId);
    }
}
