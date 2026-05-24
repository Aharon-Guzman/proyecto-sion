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

    public class ConfiguracionSitioRepository : Repository<ConfiguracionSitio>, IConfiguracionSitioRepository
    {
        public ConfiguracionSitioRepository(SionDbContext context) : base(context) { }

        public async Task<ConfiguracionSitio?> GetByClaveAsync(string clave)
            => await _dbSet.FirstOrDefaultAsync(c => c.Clave == clave);
    }
}
