using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fitotron5000.Models
{
    public partial class fitotron_devContext : DbContext
    {
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Weights> Weights { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=Eris;Database=fitotron_dev;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.CurrentWeight)
                    .HasColumnName("currentWeight")
                    .HasColumnType("float");

                entity.Property(e => e.DiscordId).HasColumnName("_discordID");

                entity.Property(e => e.Goal)
                    .HasColumnName("goal")
                    .HasColumnType("float");
                entity.Ignore(e => e.discordID);
            });

            modelBuilder.Entity<Weights>(entity =>
            {
                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TimeStamp)
                    .HasColumnName("timeStamp")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.UserWeight)
                    .HasColumnName("userWeight")
                    .HasColumnType("float");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Weights)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_weights_users");
            });
        }
    }
}
