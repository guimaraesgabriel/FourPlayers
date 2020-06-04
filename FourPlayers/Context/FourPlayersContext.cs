using FourPlayers.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FourPlayers.Context
{
    public class FourPlayersContext : DbContext
    {
        public FourPlayersContext() : base("Name=conn")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>().Configure(a => a.HasColumnType("varchar"));
        }

        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<ClientesAlugueis> ClientesAlugueis { get; set; }
        public DbSet<ClientesHistoricos> ClientesHistoricos { get; set; }
        public DbSet<ClientesPlanos> ClientesPlanos { get; set; }
        public DbSet<Consoles> Consoles { get; set; }
        public DbSet<GruposUsuarios> GruposUsuarios { get; set; }
        public DbSet<Jogos> Jogos { get; set; }
        public DbSet<JogosCategorias> JogosCategorias { get; set; }
        public DbSet<JogosContas> JogosContas { get; set; }
        public DbSet<JogosImagens> JogosImagens { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MovimentacoesFinanceiras> Despesas { get; set; }
        public DbSet<Planos> Planos { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<StatusJogos> StatusJogos { get; set; }
        public DbSet<TiposContas> TiposContas { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<UsuariosConsoles> UsuariosConsoles { get; set; }

    }
}