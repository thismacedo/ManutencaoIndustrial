using Microsoft.EntityFrameworkCore;
using ManutencaoIndustrial.Models;

namespace ManutencaoIndustrial.Data
{
    public class AppDbContext : DbContext
    {
        // Construtor que recebe as opções de configuração de conexão
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets que representam as tabelas do banco de dados
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Maquina> Maquinas { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<StatusChamado> StatusChamados { get; set; }

        // Método que configura o modelo do banco de dados (se necessário)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais do modelo podem ser feitas aqui, caso necessário
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=ManutencaoIndustrial.db");
            }
        }
    }
}
