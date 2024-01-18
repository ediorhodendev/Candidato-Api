using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrudCandidatos.Application.Interfaces;
using CrudCandidatos.Application.Services;
using CrudCandidatos.Domain.Models;
using CrudCandidatos.Infrastructure.Data;
using CrudCandidatos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

public class CandidatoServiceTestsDocker : IDisposable
{
    private readonly ICandidatoService _CandidatoService;
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public CandidatoServiceTestsDocker()
    {
        // Configurar as opções do contexto do banco de dados a partir do appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .Options;

        // Criar um contexto de banco de dados com as opções configuradas
        var dbContext = new AppDbContext(_dbContextOptions);

        // Inicializar o serviço de Candidato com o contexto do banco de dados configurado
        _CandidatoService = new CandidatoService(new CandidatoRepository(dbContext));

        // Inicializar o banco de dados com dados de teste
        SeedTestData();
    }

    private void SeedTestData()
    {
        using (var dbContext = new AppDbContext(_dbContextOptions))
        {
            // Limpar os dados existentes
            dbContext.Candidatos.RemoveRange(dbContext.Candidatos);
            dbContext.SaveChanges();

            // Adicionar alguns Candidatos de teste ao banco de dados
            dbContext.Candidatos.AddRange(new[]
            {
                    new Candidato { Nome = "Candidato 1", Email = "candidato1@gmail.com", Cpf = "349.425.040-58" },
                    new Candidato { Nome = "Candidato 2", Email = "candidato2@gmail.com", Cpf = "875.579.920-59" },
                    new Candidato { Nome = "Candidato 3", Email = "candidato3@gmail.com", Cpf = "172.802.270-31" },
                    new Candidato { Nome = "Candidato 4", Email = "candidato4@gmail.com", Cpf = "598.964.330-62" },
                    new Candidato { Nome = "Candidato 5", Email = "candidato5@gmail.com", Cpf = "926.333.430-74" }
            });

            dbContext.SaveChanges();
        }
    }

    [Fact]
    public async Task DeveListarCandidatosCorretamente()
    {
        // Act
        var Candidatos = await _CandidatoService.ListarCandidatosAsync();

        // Assert
        Assert.NotNull(Candidatos);
        Assert.Equal(3, Candidatos.Count());
    }

    [Fact]
    public async Task DeveObterCandidatoPorIdCorretamente()
    {
        // Arrange
        var CandidatoExistenteId = 1;

        // Act
        var Candidato = await _CandidatoService.ObterCandidatoPorIdAsync(CandidatoExistenteId);

        // Assert
        Assert.NotNull(Candidato);
        Assert.Equal(CandidatoExistenteId, Candidato.Id);
    }

    [Fact]
    public async Task DeveCriarCandidatoCorretamente()
    {
        // Arrange
        var novoCandidato = new Candidato { Nome = "Novo Candidato", Email = "novocandidato@gmail.com", Cpf = "692.093.420-54" };

        // Act
        var novoCandidatoId = await _CandidatoService.CriarCandidatoAsync(novoCandidato);

        // Assert
        Assert.NotEqual(0, novoCandidatoId);
    }

    [Fact]
    public async Task DeveAtualizarCandidatoCorretamente()
    {
        // Arrange
        var CandidatoExistenteId = 1;
        var CandidatoAtualizado = new Candidato { Id = CandidatoExistenteId, Nome = "Candidato Atualizado", Email ="candidato1@gmail.com", Cpf = ""};

        // Act
        await _CandidatoService.AtualizarCandidatoAsync(CandidatoExistenteId, CandidatoAtualizado);

        // Assert
        var CandidatoAtualizadoNovamente = await _CandidatoService.ObterCandidatoPorIdAsync(CandidatoExistenteId);
        Assert.Equal(CandidatoAtualizado.Nome, CandidatoAtualizadoNovamente.Nome);
        Assert.Equal(CandidatoAtualizado.Email, CandidatoAtualizadoNovamente.Email);
        Assert.Equal(CandidatoAtualizado.Cpf, CandidatoAtualizadoNovamente.Cpf);
    }

    [Fact]
    public async Task DeveDeletarCandidatoCorretamente()
    {
        // Arrange
        var CandidatoExistenteId = 1;

        // Act
        await _CandidatoService.DeletarCandidatoAsync(CandidatoExistenteId);

        // Assert
        var CandidatoDeletado = await _CandidatoService.ObterCandidatoPorIdAsync(CandidatoExistenteId);
        Assert.Null(CandidatoDeletado);
    }

    public void Dispose()
    {
        // Dispose do contexto do banco de dados
        using (var dbContext = new AppDbContext(_dbContextOptions))
        {
            dbContext.Database.EnsureDeleted();
        }
    }
}
