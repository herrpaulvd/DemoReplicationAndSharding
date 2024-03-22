using Microsoft.EntityFrameworkCore;

namespace DemoReplicationAndSharding.Data
{
    public class ShardContext : DbContext
    {
        private static string[] Connections =
        {
            "Host=172.21.0.5; Database=db; Username=postgres; Password=228cat228",
            "Host=172.21.0.6; Database=db; Username=postgres; Password=228cat228",
            "Host=172.21.0.7; Database=db; Username=postgres; Password=228cat228",
        };

        private static string[] ShardBorders =
        {
            "i", "s"
        };

        public static int GetShardIndex(string s)
        {
            for(int i = 0; i < ShardBorders.Length; i++)
                if(s.CompareTo(ShardBorders[i]) == -1)
                    return i;
            return ShardBorders.Length;
        }

        private int index;
        public ShardContext(int index) => this.index = index;

        public virtual DbSet<Dict> Dict { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Connections[index]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dict>(e =>
            {
                e.HasKey(e => e.Key).HasName("dict_pkey");
                e.Property(e => e.Key).HasColumnName("key").HasMaxLength(100).IsUnicode(false);
                e.Property(e => e.Value).HasColumnName("value").HasMaxLength(100).IsUnicode(false);
            });
        }
    }
}
