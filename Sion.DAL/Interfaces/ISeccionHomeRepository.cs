using Sion.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.DAL.Interfaces
{
    public interface ISeccionHomeRepository : IRepository<SeccionHome>
    {
        Task<IEnumerable<SeccionHome>> GetActivasAsync();
    }
}
