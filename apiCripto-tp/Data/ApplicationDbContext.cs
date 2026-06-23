using apiCripto_tp.Models;
using Microsoft.EntityFrameworkCore;

namespace apiCripto_tp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.CryptoAmount).HasPrecision(28, 12);
            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Money).HasPrecision(28, 2);

            modelBuilder.Entity<Usuarios>()
                .HasIndex(u => u.Nombre_Usuario).IsUnique();

            modelBuilder.Entity<Crypto>().HasData(
                new Crypto { Id = 1, Codigo = "btc", Nombre = "Bitcoin", Simbolo = "BTC" },
                new Crypto { Id = 2, Codigo = "eth", Nombre = "Ethereum", Simbolo = "ETH" },
                new Crypto { Id = 3, Codigo = "usdc", Nombre = "USD Coin", Simbolo = "USDC" },
                new Crypto { Id = 4, Codigo = "usdt", Nombre = "Tether", Simbolo = "USDT" }
            );
        }
    }
}
