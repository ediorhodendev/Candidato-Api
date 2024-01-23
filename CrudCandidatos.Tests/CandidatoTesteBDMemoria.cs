using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudCandidatos.Application.Interfaces;
using CrudCandidatos.Application.Services;
using CrudCandidatos.Domain.Models;
using CrudCandidatos.Infrastructure.Data;
using CrudCandidatos.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class CandidatoServiceTestsMemoria : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    private readonly AppDbContext _dbContext;

    public CandidatoServiceTestsMemoria()
    {
        // Configuração do DbContext em memória para os testes
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new AppDbContext(_dbContextOptions);
        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task CriarCandidato_DeveRetornarIdDoNovoCandidato()
    {
        // Arrange
        var CandidatoRepositoryMock = new Mock<ICandidatoRepository>();
        var CandidatoService = new CandidatoService(CandidatoRepositoryMock.Object);

        var novoCandidato = new Candidato { Nome = "Novo Candidato", Email = "candidato1", Cpf = "" };

        // Act
        var novoCandidatoId = await CandidatoService.CriarCandidatoAsync(novoCandidato);

        // Assert
        Assert.NotEqual(0, novoCandidatoId); // O ID deve ser diferente de zero
    }

    [Fact]
    public async Task ObterCandidatoPorId_QuandoCandidatoExiste_DeveRetornarCandidato()
    {
        // Arrange
        var CandidatoRepositoryMock = new Mock<ICandidatoRepository>();
        var CandidatoService = new CandidatoService(CandidatoRepositoryMock.Object);

        var CandidatoExistente = new Candidato { Nome = "Candidato Existente", Email = "candidato1", Cpf = "" };

        
        _dbContext.Candidatos.Add(CandidatoExistente);
        _dbContext.SaveChanges();

        // Act
        var Candidato = await CandidatoService.ObterCandidatoPorIdAsync(CandidatoExistente.Id);

        // Assert
        Assert.NotNull(Candidato);
        Assert.Equal(CandidatoExistente.Id, Candidato.Id);
        Assert.Equal(CandidatoExistente.Nome, Candidato.Nome);
        Assert.Equal(CandidatoExistente.Email, Candidato.Email);
        Assert.Equal(CandidatoExistente.Cpf, Candidato.Cpf);
    }

    [Fact]
    public async Task DeletarCandidato_QuandoCandidatoExiste_DeveDeletarCandidato()
    {
        // Arrange
        var CandidatoRepositoryMock = new Mock<ICandidatoRepository>();
        var CandidatoService = new CandidatoService(CandidatoRepositoryMock.Object);

        var CandidatoExistente = new Candidato { Nome = "Candidato Existente", Email = "candidato1@gmail.com", Cpf = "692.093.420-54" };

       
        _dbContext.Candidatos.Add(CandidatoExistente);
        _dbContext.SaveChanges();

        // Act
        await CandidatoService.DeletarCandidatoAsync(CandidatoExistente.Id);

        // Assert
        var CandidatoDeletado = await _dbContext.Candidatos.FindAsync(CandidatoExistente.Id);
        Assert.Null(CandidatoDeletado); // O Candidato deve ser nulo após a exclusão
    }

    public void Dispose()
    {
        
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
