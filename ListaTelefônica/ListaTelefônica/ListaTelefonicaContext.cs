using Microsoft.EntityFrameworkCore;

namespace ListaTelefônica
{
    public class ListaTelefonicaContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Telefone> Telefones { get; set; }

        public ListaTelefonicaContext(string connectionString) : base(new DbContextOptionsBuilder<ListaTelefonicaContext>().UseSqlServer(connectionString).Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>().ToTable("tb_pessoas");
            modelBuilder.Entity<Pessoa>().HasKey(x => x.Id);
            modelBuilder.Entity<Pessoa>().Property(x => x.Id).HasColumnName("id_pessoa");
            modelBuilder.Entity<Pessoa>().Property(x => x.Nome).HasColumnName("nome").HasMaxLength(255);
            modelBuilder.Entity<Pessoa>().HasMany(x => x.Telefones).WithOne().HasForeignKey("id_pessoa").IsRequired();

            modelBuilder.Entity<Telefone>().ToTable("tb_telefones");
            modelBuilder.Entity<Telefone>().HasKey(x => x.Id);
            modelBuilder.Entity<Telefone>().Property(x => x.Id).HasColumnName("id_telefone");
            modelBuilder.Entity<Telefone>().Property(x => x.Tipo).HasColumnName("tipo_telefone").HasColumnType("tinyint");
            modelBuilder.Entity<Telefone>().Property(x => x.Numero).HasColumnName("numero").HasMaxLength(11);
        }
    }
}
