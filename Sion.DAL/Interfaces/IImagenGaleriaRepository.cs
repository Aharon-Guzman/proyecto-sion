using Sion.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.DAL.Interfaces
{
    public interface IImagenGaleriaRepository : IRepository<ImagenGaleria>
    {
        Task<IEnumerable<ImagenGaleria>> GetActivasAsync();
    }
}
