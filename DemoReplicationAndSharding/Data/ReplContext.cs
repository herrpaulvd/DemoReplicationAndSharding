using Microsoft.EntityFrameworkCore;

namespace DemoReplicationAndSharding.Data
{
    public class ReplContext : DbContext
    {
        private static string[] Connections =
        {
            "Host=172.21.0.2; Database=db; Username=postgres; Password=228cat228",
            "Host=172.21.0.3; Database=db; Username=postgres; Password=228cat228",
        };

        public const int Master = 0;
        public const int Slave = 1;
        private int index;

        public ReplContext(int index) => this.index = index;

        public virtual DbSet<Arr> Arr { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Connections[index]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arr>(e =>
            {
                e.HasKey(e => e.Index).HasName("arr_pkey");
                e.Property(e => e.Index).HasColumnName("index");
                e.Property(e => e.Element).IsUnicode(false)
                .HasMaxLength(100).HasColumnName("element");
            });
        }
    }
}
