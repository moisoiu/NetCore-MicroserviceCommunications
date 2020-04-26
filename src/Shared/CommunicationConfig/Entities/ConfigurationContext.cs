using Microsoft.EntityFrameworkCore;

namespace CommunicationConfig.Entities
{
    internal partial class ConfigurationContext : DbContext
    {
        public ConfigurationContext()
        {
        }

        public ConfigurationContext(DbContextOptions<ConfigurationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Communication> Communication { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Communication>(entity =>
            {
                entity.Property(e => e.CommunicationModeName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
