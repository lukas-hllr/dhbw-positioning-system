using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Dhbw_positioning_System_Backend.Model;

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
                optionsBuilder.UseSqlite("Filename=DhbwPositioningSystemDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessPoint>(entity =>
            {
                entity.HasKey(e => e.MacAddress);

                entity.ToTable("Access_Point");

                entity.Property(e => e.MacAddress)
                    .HasColumnType("text")
                    .HasColumnName("mac_address");

                entity.Property(e => e.Latitude)
                    .HasColumnType("real")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("real")
                    .HasColumnName("longitude");

                entity.Property(e => e.Room)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("room");

                entity.Property(e => e.RouterTypeId)
                    .HasColumnType("integer")
                    .HasColumnName("router_type_id");

                entity.HasOne(d => d.RouterType)
                    .WithMany(p => p.AccessPoint)
                    .HasForeignKey(d => d.RouterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.Property(e => e.MeasurementId)
                    .HasColumnType("integer")
                    .HasColumnName("measurement_id");

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("date");

                entity.Property(e => e.Device)
                    .IsRequired()
                    .HasColumnName("device");

                entity.Property(e => e.LatitudeHighAccuracy)
                    .HasColumnType("real")
                    .HasColumnName("latitudeHighAccuracy");

                entity.Property(e => e.LatitudeLowAccuracy)
                    .HasColumnType("real")
                    .HasColumnName("latitudeLowAccuracy");

                entity.Property(e => e.LongitudeHighAccuracy)
                    .HasColumnType("real")
                    .HasColumnName("longitudeHighAccuracy");

                entity.Property(e => e.LongitudeLowAccuracy)
                    .HasColumnType("real")
                    .HasColumnName("longitudeLowAccuracy");
            });

            modelBuilder.Entity<NetworkMeasurement>(entity =>
            {
                entity.ToTable("Network_Measurement");

                entity.Property(e => e.NetworkMeasurementId)
                    .HasColumnType("integer")
                    .HasColumnName("network_measurement_id");

                entity.Property(e => e.MacAddress)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("mac_address");

                entity.Property(e => e.MeasuredStrength)
                    .HasColumnType("real")
                    .HasColumnName("measured_strength");

                entity.Property(e => e.MeasurementId)
                    .HasColumnType("integer")
                    .HasColumnName("measurement_id");

                entity.Property(e => e.NetworkSsid)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("network_SSID");

                entity.HasOne(d => d.MacAddressNavigation)
                    .WithMany(p => p.NetworkMeasurement)
                    .HasForeignKey(d => d.MacAddress)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Measurement)
                    .WithMany(p => p.NetworkMeasurement)
                    .HasForeignKey(d => d.MeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RouterType>(entity =>
            {
                entity.ToTable("Router_Type");

                entity.Property(e => e.RouterTypeId)
                    .HasColumnType("integer")
                    .HasColumnName("router_type_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
