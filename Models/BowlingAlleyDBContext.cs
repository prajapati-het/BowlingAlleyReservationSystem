using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BowlingAlley.Models
{
    public partial class BowlingAlleyDBContext : DbContext
    {
        public BowlingAlleyDBContext()
        {
        }

        public BowlingAlleyDBContext(DbContextOptions<BowlingAlleyDBContext> options)
            : base(options)
        {
        }

        public  DbSet<BookingSlots> BookingSlots { get; set; }
        public  DbSet<Reservations> Reservations { get; set; }
        public  DbSet<Roles> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                var connection = builder.Build();
                string connectionString = connection.GetConnectionString("BowlingAlleyDBConnectionString");

                optionsBuilder.UseSqlServer(connectionString);
            }*/
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingSlots>(entity =>
            {
                entity.HasKey(e => e.SlotId);

                entity.HasMany(e => e.Reservations)
                      .WithOne(r => r.Slot)
                      .HasForeignKey(r => r.SlotId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Reservations_BookingSlots");
            });

            modelBuilder.Entity<Reservations>(entity =>
            {
                entity.HasKey(e => e.ReservationId);

                entity.Property(e => e.ReservedOn)
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))");

                entity.HasOne(r => r.Role)
                      .WithMany(e => e.Reservations)
                      .HasForeignKey(r => r.ReservedBy)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Reservations_Roles");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.Property(e => e.EmpName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasMany(e => e.Reservations)
                      .WithOne(r => r.Role)
                      .HasForeignKey(r => r.ReservedBy)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Reservations_Roles");
            });
        }

    }
}
