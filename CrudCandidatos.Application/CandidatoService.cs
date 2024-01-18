using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrudCandidatos.Application.Interfaces;
using CrudCandidatos.Domain.Models;
using CrudCandidatos.Infrastructure.Interfaces;
using OpenQA.Selenium;

namespace CrudCandidatos.Application.Services
{
    public class CandidatoService : ICandidatoService
    {
        private readonly ICandidatoRepository _CandidatoRepository;

        public CandidatoService(ICandidatoRepository CandidatoRepository)
        {
            _CandidatoRepository = CandidatoRepository;
        }

        public async Task<IEnumerable<Candidato>> ListarCandidatosAsync()
        {
            return await _CandidatoRepository.ListarCandidatosAsync();
        }

        public async Task<Candidato> ObterCandidatoPorIdAsync(int id)
        {
            return await _CandidatoRepository.ObterCandidatoPorIdAsync(id);
        }

        public async Task<int> CriarCandidatoAsync(Candidato Candidato)
        {
            if (!IsValidCPF(Candidato.Cpf))
            {
                throw new ArgumentException("CPF inválido.");
            }

            if (!IsValidEmail(Candidato.Email))
            {
                throw new ArgumentException("Email inválido.");
            }

            return await _CandidatoRepository.CriarCandidatoAsync(Candidato);
        }

        public async Task AtualizarCandidatoAsync(int id, Candidato Candidato)
        {
            var CandidatoExistente = await _CandidatoRepository.ObterCandidatoPorIdAsync(id);

            if (CandidatoExistente == null)
            {
                throw new NotFoundException("Candidato não encontrado.");
            }

            if (!IsValidCPF(Candidato.Cpf))
            {
                throw new ArgumentException("CPF inválido.");
            }

            if (!IsValidEmail(Candidato.Email))
            {
                throw new ArgumentException("Email inválido.");
            }

            CandidatoExistente.Nome = Candidato.Nome;
            CandidatoExistente.Email = Candidato.Email;
            CandidatoExistente.Cpf = Candidato.Cpf;

            await _CandidatoRepository.AtualizarCandidatoAsync(id, CandidatoExistente);
        }

        public async Task<IEnumerable<Candidato>> BuscarCandidatosPorNomeAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome do Candidato não pode ser vazio ou nulo.");
            }

            var Candidatos = await _CandidatoRepository.BuscarCandidatosPorNomeAsync(nome);
            return Candidatos;
        }

        public async Task DeletarCandidatoAsync(int id)
        {
            var CandidatoExistente = await _CandidatoRepository.ObterCandidatoPorIdAsync(id);

            if (CandidatoExistente == null)
            {
                throw new NotFoundException("Candidato não encontrado.");
            }

            await _CandidatoRepository.DeletarCandidatoAsync(CandidatoExistente.Id);
        }

        private bool IsValidCPF(string cpf)
        {
            // Remova caracteres não numéricos do CPF
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
            {
                return false;
            }

            // Implemente a lógica de validação do CPF de acordo com as regras.
            // Esta é uma validação básica, e é altamente recomendável usar uma biblioteca de validação de CPF confiável.
            // Aqui está um exemplo simples:
            int[] weights = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * weights[i];
            }
            int remainder = sum % 11;
            if (remainder < 2 && int.Parse(cpf[9].ToString()) != 0)
            {
                return false;
            }
            else if (remainder >= 2 && int.Parse(cpf[9].ToString()) != 11 - remainder)
            {
                return false;
            }

            // Continue a validação para os dígitos 10 e 11, se necessário.

            return true;
        }

        private bool IsValidEmail(string email)
        {
            // Implemente a validação do email usando expressão regular.
            // Esta é uma validação básica, e existem expressões regulares mais complexas para validar emails.
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
