using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudCandidatos.Domain.Models;
using CrudCandidatos.Infrastructure.Data;
using CrudCandidatos.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudCandidatos.Infrastructure.Repositories
{
    public class CandidatoRepository : ICandidatoRepository
    {
        private readonly AppDbContext _dbContext;

        public CandidatoRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Candidato>> ListarCandidatosAsync()
        {
            return await _dbContext.Candidatos.ToListAsync();
        }

        public async Task<Candidato> ObterCandidatoPorIdAsync(int id)
        {
            return await _dbContext.Candidatos.FindAsync(id);
        }

        public async Task<IEnumerable<Candidato>> BuscarCandidatosPorNomeAsync(string nome)
        {
            return await _dbContext.Candidatos
                .Where(p => p.Nome.Contains(nome))
                .ToListAsync();
        }

        public async Task<int> CriarCandidatoAsync(Candidato Candidato)
        {
            if (Candidato == null)
            {
                throw new ArgumentNullException(nameof(Candidato));
            }

            _dbContext.Candidatos.Add(Candidato);
            await _dbContext.SaveChangesAsync();
            return Candidato.Id;
        }

        public async Task AtualizarCandidatoAsync(int v, Candidato Candidato)
        {
            if (Candidato == null)
            {
                throw new ArgumentNullException(nameof(Candidato));
            }

            if (!_dbContext.Candidatos.Local.Contains(Candidato))
            {
                _dbContext.Entry(Candidato).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletarCandidatoAsync(int id)
        {
            var Candidato = await _dbContext.Candidatos.FindAsync(id);

            if (Candidato == null)
            {
                throw new KeyNotFoundException("Candidato não encontrado.");
            }

            _dbContext.Candidatos.Remove(Candidato);
            await _dbContext.SaveChangesAsync();
        }
    }
}
