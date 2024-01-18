
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudCandidatos.Domain.Models;


namespace CrudCandidatos.Infrastructure.Interfaces
{
    public interface ICandidatoRepository
    {
        /// <summary>
        /// Obtém uma lista de todos os Candidatos de forma assíncrona.
        /// </summary>
        /// <returns>Uma coleção de Candidatos.</returns>
        Task<IEnumerable<Candidato>> ListarCandidatosAsync();

        /// <summary>
        /// Obtém um Candidato por ID de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID do Candidato a ser obtido.</param>
        /// <returns>O Candidato correspondente ao ID especificado.</returns>
        Task<Candidato> ObterCandidatoPorIdAsync(int id);

        /// <summary>
        /// Cria um novo Candidato de forma assíncrona.
        /// </summary>
        /// <param name="Candidato">O Candidato a ser criado.</param>
        /// <returns>O ID do Candidato criado.</returns>
        Task<int> CriarCandidatoAsync(Candidato Candidato);

        /// <summary>
        /// Atualiza um Candidato existente de forma assíncrona.
        /// </summary>
        /// <param name="Candidato">O Candidato atualizado.</param>
        Task AtualizarCandidatoAsync(int v, Candidato Candidato);

        /// <summary>
        /// Atualiza um Candidato existente de forma assíncrona.
        /// </summary>
        /// <param name="nome">O Candidato atualizado.</param>
        Task<IEnumerable<Candidato>> BuscarCandidatosPorNomeAsync(string nome);

        /// <summary>
        /// Exclui um Candidato de forma assíncrona.
        /// </summary>
        /// <param name="Candidato">O Candidato a ser excluído.</param>
        Task DeletarCandidatoAsync(int id);
    }
}



