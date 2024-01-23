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

        public async Task<bool> VerificarCpfExistenteAsync(string cpf)
        {
            var candidato = await _dbContext.Candidatos.FirstOrDefaultAsync(c => c.Cpf == cpf);
            return candidato != null;
        }

        public async Task<IEnumerable<Candidato>> BuscarCandidatosPorNomeAsync(string nome)
        {
            return await _dbContext.Candidatos
                .Where(p => p.Nome.Contains(nome))
                .ToListAsync();
        }

        public async Task<int> CriarCandidatoAsync(Candidato candidato)
        {
            if (candidato == null)
            {
                throw new ArgumentNullException(nameof(candidato));
            }

            // Verificar se o CPF já existe
            var cpfExistente = await VerificarCpfExistenteAsync(candidato.Cpf);

            if (cpfExistente)
            {
                throw new ArgumentException("CPF já cadastrado.");
            }

            _dbContext.Candidatos.Add(candidato);
            await _dbContext.SaveChangesAsync();
            return candidato.Id;
        }

        public async Task AtualizarCandidatoAsync(int id, Candidato candidato)
        {
            if (candidato == null)
            {
                throw new ArgumentNullException(nameof(candidato));
            }

            if (!_dbContext.Candidatos.Local.Contains(candidato))
            {
                _dbContext.Entry(candidato).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletarCandidatoAsync(int id)
        {
            var candidato = await _dbContext.Candidatos.FindAsync(id);

            if (candidato == null)
            {
                throw new KeyNotFoundException("Candidato não encontrado.");
            }

            _dbContext.Candidatos.Remove(candidato);
            await _dbContext.SaveChangesAsync();
        }
    }
}
