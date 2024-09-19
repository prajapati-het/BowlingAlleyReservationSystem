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

        //public DbSet<RejectedSlotsViewModel> RejectedSlots { get; set; }

        public DbSet<RejectedSlots> RejectedSlots { get; set; }

        public DbSet<ReservationRejections> ReservationRejections { get; set; }

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

                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(10)
                      .HasDefaultValue("1478523690");

            }
            );

            modelBuilder.Entity<ReservationRejections>()
        .HasKey(rr => new { rr.ReservationId, rr.EmpId });

            modelBuilder.Entity<ReservationRejections>()
                .HasOne(rr => rr.Reservation)
                .WithMany(r => r.ReservationRejections)
                .HasForeignKey(rr => rr.ReservationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservationRejections_Reservations");

            modelBuilder.Entity<ReservationRejections>()
                .HasOne(rr => rr.Role)
                .WithMany(e => e.ReservationRejections)
                .HasForeignKey(rr => rr.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservationRejections_Roles");
        }

    }

    
}
