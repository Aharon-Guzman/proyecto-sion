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

    public class SeccionHomeRepository : Repository<SeccionHome>, ISeccionHomeRepository
    {
        public SeccionHomeRepository(SionDbContext context) : base(context) { }

        public async Task<IEnumerable<SeccionHome>> GetActivasAsync()
            => await _dbSet.Where(s => s.EstaActiva).OrderBy(s => s.Orden).ToListAsync();
    }
}
