using AluraRpa.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraRpa.Domain.Interfaces
{
    public interface ICursoRepository
    {
        Task AddAsync(Curso curso);
        Task<IEnumerable<Curso>> GetAllAsync();
    }
}