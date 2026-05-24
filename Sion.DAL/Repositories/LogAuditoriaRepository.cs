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

    public class LogAuditoriaRepository : Repository<LogAuditoria>, ILogAuditoriaRepository
    {
        public LogAuditoriaRepository(SionDbContext context) : base(context) { }

        public async Task<IEnumerable<LogAuditoria>> GetByFechasAsync(DateTime desde, DateTime hasta)
            => await _dbSet.Where(l => l.FechaHora >= desde && l.FechaHora <= hasta).ToListAsync();
    }
}
