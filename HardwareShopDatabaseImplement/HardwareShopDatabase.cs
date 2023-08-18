using HardwareShopDatabaseImplement.Models;
using HardwareShopDatabaseImplement.Models.ManyToMany;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDatabaseImplement.Models.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HardwareShopDatabaseImplement
{
    public class HardwareShopDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Computer_Hardware_Store5;Username=postgres;Password=1234");
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComponentBuild>().HasKey(x => new { x.ComponentId, x.BuildId });
            modelBuilder.Entity<PurchaseBuild>().HasKey(x => new { x.PurchaseId, x.BuildId });
            modelBuilder.Entity<PurchaseGood>().HasKey(x => new { x.PurchaseId, x.GoodId });
            modelBuilder.Entity<GoodComponent>().HasKey(x => new { x.GoodId, x.ComponentId });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Login).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }

        public virtual DbSet<Build> Builds { set; get; }

        public virtual DbSet<ComponentBuild> ComponentsBuilds { set; get; }

        public virtual DbSet<Comment> Comments { set; get; }

        public virtual DbSet<Component> Components { set; get; }

        public virtual DbSet<Good> Goods { set; get; }

        public virtual DbSet<GoodComponent> GoodsComponents { set; get; }

        public virtual DbSet<Order> Orders { set; get; }

        public virtual DbSet<Purchase> Purchases { set; get; }

        public virtual DbSet<PurchaseBuild> PurchasesBuilds { set; get; }

        public virtual DbSet<PurchaseGood> PurchasesGoods { set; get; }

        public virtual DbSet<User> Users { set; get; }
    }
}