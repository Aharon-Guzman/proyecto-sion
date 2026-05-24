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

    public class ImagenGaleriaRepository : Repository<ImagenGaleria>, IImagenGaleriaRepository
    {
        public ImagenGaleriaRepository(SionDbContext context) : base(context) { }

        public async Task<IEnumerable<ImagenGaleria>> GetActivasAsync()
            => await _dbSet.Where(i => i.EstaActiva).ToListAsync();
    }
}
