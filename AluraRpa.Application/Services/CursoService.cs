using AluraRpa.Domain.Entities;
using AluraRpa.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraRpa.Application.Services
{
    public class CursoService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task AdicionarCursoAsync(Curso curso)
        {
            // Adicione validações aqui se necessário
            await _cursoRepository.AddAsync(curso);
        }

        public async Task<IEnumerable<Curso>> ObterCursosAsync()
        {
            return await _cursoRepository.GetAllAsync();
        }
    }
}
