using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudCandidatos.Application.Interfaces;
using CrudCandidatos.Application.Services;
using CrudCandidatos.Domain.Models;
using CrudCandidatos.Infrastructure.Interfaces;
using Moq;
using Xunit;

public class CandidatoServiceTests
{
    [Fact]
    public async Task CriarCandidato_DeveRetornarIdDoNovoCandidato()
    {
        // Arrange
        var CandidatoRepositoryMock = new Mock<ICandidatoRepository>();
        CandidatoRepositoryMock.Setup(repo => repo.CriarCandidatoAsync(It.IsAny<Candidato>()))
                            .ReturnsAsync(1);

        var CandidatoService = new CandidatoService(CandidatoRepositoryMock.Object);

        var novoCandidato = new Candidato { Nome = "Novo Candidato", Email = "", Cpf = "" };

        // Act
        var novoCandidatoId = await CandidatoService.CriarCandidatoAsync(novoCandidato);

        // Assert
        Assert.Equal(1, novoCandidatoId);
    }

    [Fact]
    public async Task ObterCandidatoPorId_QuandoCandidatoExiste_DeveRetornarCandidato()
    {
        // Arrange
        var CandidatoRepositoryMock = new Mock<ICandidatoRepository>();
        CandidatoRepositoryMock.Setup(repo => repo.ObterCandidatoPorIdAsync(1))
                            .ReturnsAsync(new Candidato { Id = 1, Nome = "Candidato Existente", Email = "", Cpf = "" });

        var CandidatoService = new CandidatoService(CandidatoRepositoryMock.Object);

        // Act
        var Candidato = await CandidatoService.ObterCandidatoPorIdAsync(1);

        // Assert
        Assert.NotNull(Candidato);
        Assert.Equal(1, Candidato.Id);
        Assert.Equal("Candidato Existente", Candidato.Nome);
        Assert.Equal("candidato1@gamail.com", Candidato.Email);
        Assert.Equal("692.093.420-54", Candidato.Cpf);
    }

    [Fact]
    public async Task AtualizarCandidato_QuandoCandidatoExiste_DeveAtualizarCandidato()
    {
        // Arrange
        var CandidatoRepositoryMock = new Mock<ICandidatoRepository>();
        CandidatoRepositoryMock.Setup(repo => repo.AtualizarCandidatoAsync(1, It.IsAny<Candidato>()))
                            .Returns(Task.CompletedTask);

        var CandidatoService = new CandidatoService(CandidatoRepositoryMock.Object);

        var CandidatoAtualizado = new Candidato { Id = 1, Nome = "Candidato Atualizado", Email = "", Cpf = "" };

        // Act
        await CandidatoService.AtualizarCandidatoAsync(1, CandidatoAtualizado);

        // Assert - Verifique se o método de atualização do repositório foi chamado com os parâmetros corretos.
         CandidatoRepositoryMock.Verify(repo => repo.AtualizarCandidatoAsync(1, CandidatoAtualizado), Times.Once);
    }

    [Fact]
    public async Task DeletarCandidato_QuandoCandidatoExiste_DeveDeletarCandidato()
    {
        // Arrange
        var CandidatoRepositoryMock = new Mock<ICandidatoRepository>();
        CandidatoRepositoryMock.Setup(repo => repo.DeletarCandidatoAsync(1))
                            .Returns(Task.CompletedTask);

        var CandidatoService = new CandidatoService(CandidatoRepositoryMock.Object);

        // Act
        await CandidatoService.DeletarCandidatoAsync(1);

        // Assert - Verifique se o método de exclusão do repositório foi chamado com o ID correto.
        CandidatoRepositoryMock.Verify(repo => repo.DeletarCandidatoAsync(1), Times.Once);
    }

    
}
