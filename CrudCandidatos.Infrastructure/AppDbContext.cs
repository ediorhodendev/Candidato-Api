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

            
            modelBuilder.Entity<Candidato>(entity =>
            {
                
                entity.ToTable("Candidatos");

                
                entity.HasKey(p => p.Id);

                
                entity.Property(p => p.Id).HasColumnName("Id");
                entity.Property(p => p.Nome).HasColumnName("Nome").HasMaxLength(100).IsRequired();
                entity.Property(p => p.Email).HasColumnName("Email").IsRequired();
                entity.Property(p => p.Cpf).HasColumnName("Cpf").IsRequired();

                
                entity.HasIndex(p => p.Nome).IsUnique();

                
            });
        }
    }
}
