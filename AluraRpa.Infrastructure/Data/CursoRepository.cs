using AluraRpa.Domain.Entities;
using AluraRpa.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraRpa.Infrastructure.Data
{
    public class CursoRepository : ICursoRepository
    {
        private readonly List<Curso> _cursos = new List<Curso>();

        public Task AddAsync(Curso curso)
        {
            _cursos.Add(curso);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Curso>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Curso>>(_cursos);
        }
    }
}
