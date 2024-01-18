
using CrudCandidatos.Domain.Models;
using CrudCandidatos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Verifique se o banco de dados existe; se não, crie um novo (crudCandidatos)
            dbContext.Database.EnsureCreated();

            // Verifique se já existem produtos no banco de dados
            if (!dbContext.Candidatos.Any())
            {
                // Se não existirem produtos, adicione 5 produtos iniciais
                dbContext.Candidatos.AddRange(new[]
                {
                new Candidato { Nome = "Candidato 1", Email = "candidato1@gmail.com", Cpf = "349.425.040-58" },
                    new Candidato { Nome = "Candidato 2", Email = "candidato2@gmail.com", Cpf = "875.579.920-59" },
                    new Candidato { Nome = "Candidato 3", Email = "candidato3@gmail.com", Cpf = "172.802.270-31" },
                    new Candidato { Nome = "Candidato 4", Email = "candidato4@gmail.com", Cpf = "598.964.330-62" },
                    new Candidato { Nome = "Candidato 5", Email = "candidato5@gmail.com", Cpf = "926.333.430-74" }
            });

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }


    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
