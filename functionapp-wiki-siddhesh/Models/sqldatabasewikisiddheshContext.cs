using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace functionapp_wiki_siddhesh.Models
{
    public partial class sqldatabasewikisiddheshContext : DbContext
    {
        public sqldatabasewikisiddheshContext()
        {
        }

        public sqldatabasewikisiddheshContext(DbContextOptions<sqldatabasewikisiddheshContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Gate> Gates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=sqlserver-wiki-siddhesh.database.windows.net;Database=sqldatabase-wiki-siddhesh;user id=samrale;password=Saibabas123!;Encrypt=True;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>(entity =>
            {
                entity.Property(e => e.AirportId)
                    .HasColumnName("AirportID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AirportName).HasMaxLength(50);
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.Property(e => e.FlightId)
                    .HasColumnName("FlightID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.FlightDate).HasColumnType("datetime");

                entity.Property(e => e.FlightNumber).HasMaxLength(50);

                entity.HasOne(d => d.GateNavigation)
                    .WithMany(p => p.Flights)
                    .HasForeignKey(d => d.Gate)
                    .HasConstraintName("FK__Flights__Gate__778AC167");

                entity.HasOne(d => d.PartnerAirportNavigation)
                    .WithMany(p => p.Flights)
                    .HasForeignKey(d => d.PartnerAirport)
                    .HasConstraintName("FK__Flights__Partner__76969D2E");
            });

            modelBuilder.Entity<Gate>(entity =>
            {
                entity.Property(e => e.GateId)
                    .HasColumnName("GateID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.GateName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
