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
        public virtual DbSet<MeasurementEntity> MeasurementEntity { get; set; }
        public virtual DbSet<RouterType> RouterType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

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

                entity.Property(e => e.AccuracyGroundTruth).HasColumnName("accuracyGroundTruth");

                entity.Property(e => e.AccuracyHighAccuracy).HasColumnName("accuracyHighAccuracy");

                entity.Property(e => e.AccuracyLowAccuracy).HasColumnName("accuracyLowAccuracy");

                entity.Property(e => e.AltitudeGroundTruth).HasColumnName("altitudeGroundTruth");

                entity.Property(e => e.AltitudeHighAccuracy).HasColumnName("altitudeHighAccuracy");

                entity.Property(e => e.AltitudeLowAccuracy).HasColumnName("altitudeLowAccuracy");

                entity.Property(e => e.Device)
                    .IsRequired()
                    .HasColumnName("device");

                entity.Property(e => e.LatitudeGroundTruth).HasColumnName("latitudeGroundTruth");

                entity.Property(e => e.LatitudeHighAccuracy)
                    .HasColumnType("real")
                    .HasColumnName("latitudeHighAccuracy");

                entity.Property(e => e.LatitudeLowAccuracy)
                    .HasColumnType("real")
                    .HasColumnName("latitudeLowAccuracy");

                entity.Property(e => e.LongitudeGroundTruth).HasColumnName("longitudeGroundTruth");

                entity.Property(e => e.LongitudeHighAccuracy)
                    .HasColumnType("real")
                    .HasColumnName("longitudeHighAccuracy");

                entity.Property(e => e.LongitudeLowAccuracy)
                    .HasColumnType("real")
                    .HasColumnName("longitudeLowAccuracy");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("timestamp");
            });

            modelBuilder.Entity<MeasurementEntity>(entity =>
            {
                entity.ToTable("Measurement_Entity");

                entity.Property(e => e.MeasurementEntityId)
                    .HasColumnType("integer")
                    .HasColumnName("measurement_entity_id");

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("mac");

                entity.Property(e => e.MeasurementId)
                    .HasColumnType("integer")
                    .HasColumnName("measurement_id");

                entity.Property(e => e.Rssi).HasColumnName("rssi");

                entity.Property(e => e.Ssid)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("ssid");

                entity.HasOne(d => d.Measurement)
                    .WithMany(p => p.MeasurementEntity)
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
