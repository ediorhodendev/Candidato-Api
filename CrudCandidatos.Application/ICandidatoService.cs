using System.Collections.Generic;
using System.Threading.Tasks;
using CrudCandidatos.Domain.Models;

namespace CrudCandidatos.Application.Interfaces
{
    public interface ICandidatoService
    {
        Task<IEnumerable<Candidato>> ListarCandidatosAsync();
        Task<Candidato> ObterCandidatoPorIdAsync(int id);
        Task<int> CriarCandidatoAsync(Candidato Candidato);
        Task AtualizarCandidatoAsync(int id, Candidato Candidato);
        Task<IEnumerable<Candidato>> BuscarCandidatosPorNomeAsync(string nome); // Novo método
        Task DeletarCandidatoAsync(int id);
    }
}
