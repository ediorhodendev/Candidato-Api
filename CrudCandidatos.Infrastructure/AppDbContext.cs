using CrudCandidatos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudCandidatos.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Candidato> Candidatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de mapeamento para a entidade Candidato
            modelBuilder.Entity<Candidato>(entity =>
            {
                // Define o nome da tabela
                entity.ToTable("Candidatos");

                // Define a chave primária
                entity.HasKey(p => p.Id);

                // Configurações de colunas
                entity.Property(p => p.Id).HasColumnName("Id");
                entity.Property(p => p.Nome).HasColumnName("Nome").HasMaxLength(100).IsRequired();
                entity.Property(p => p.Email).HasColumnName("Email").IsRequired();
                entity.Property(p => p.Cpf).HasColumnName("Cpf").IsRequired();

                // Exemplo de configuração de índice
                entity.HasIndex(p => p.Nome).IsUnique();

                
            });
        }
    }
}
