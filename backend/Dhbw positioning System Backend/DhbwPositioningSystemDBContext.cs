using System;
using Dhbw_positioning_System_Backend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dhbw_positioning_System_Backend
{
    public partial class DhbwPositioningSystemDBContext : DbContext
    {
        public DhbwPositioningSystemDBContext()
        {
        }

        public DhbwPositioningSystemDBContext(DbContextOptions<DhbwPositioningSystemDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessPoint> AccessPoint { get; set; }
        public virtual DbSet<Measurement> Measurement { get; set; }
        public virtual DbSet<NetworkMeasurement> NetworkMeasurement { get; set; }
        public virtual DbSet<RouterType> RouterType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Filename=DhbwPositioningSystemDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessPoint>(entity =>
            {
                entity.HasKey(e => e.MacAddress);

                entity.ToTable("Access_Point");

                entity.Property(e => e.MacAddress).HasColumnName("mac_address");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

                entity.Property(e => e.Room).HasColumnName("room");

                entity.Property(e => e.RouterTypeId).HasColumnName("router_type_id");

                entity.HasOne(d => d.RouterType)
                    .WithMany(p => p.AccessPoint)
                    .HasForeignKey(d => d.RouterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.Property(e => e.MeasurementId)
                    .HasColumnName("measurement_id").ValueGeneratedOnAdd();

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.LatitudeHighAccuracy).HasColumnName("latitudeHighAccuracy");

                entity.Property(e => e.LatitudeLowAccuracy).HasColumnName("latitudeLowAccuracy");

                entity.Property(e => e.LongitudeHighAccuracy).HasColumnName("longitudeHighAccuracy");

                entity.Property(e => e.LongitudeLowAccuracy).HasColumnName("longitudeLowAccuracy");
            });

            modelBuilder.Entity<NetworkMeasurement>(entity =>
            {
                entity.ToTable("Network_Measurement");

                entity.Property(e => e.NetworkMeasurementId)
                    .HasColumnName("network_measurement_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.MacAddress)
                    .IsRequired()
                    .HasColumnName("mac_address");

                entity.Property(e => e.MeasuredStrength).HasColumnName("measured_strength");

                entity.Property(e => e.MeasurementId).HasColumnName("measurement_id");

                entity.Property(e => e.NetworkSsid).HasColumnName("network_SSID");

                entity.HasOne(d => d.Measurement)
                    .WithMany(p => p.NetworkMeasurement)
                    .HasForeignKey(d => d.MeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RouterType>(entity =>
            {
                entity.ToTable("Router_Type");

                entity.Property(e => e.RouterTypeId)
                    .HasColumnName("router_type_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Range).HasColumnName("range");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
