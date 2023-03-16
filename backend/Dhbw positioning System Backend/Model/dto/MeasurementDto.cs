using System.Collections.Generic;

#nullable enable

namespace Dhbw_positioning_System_Backend.Model.dto
{
    public class MeasurementDto
    {
        public int? MeasurementId { get; set; }
        public List<MeasurementEntityDto> Measurements { get; set; }
        public PositionDto PositionGroundTruth { get; set; }
        public PositionDto? PositionHighAccuracy { get; set; }
        public PositionDto? PositionLowAccuracy { get; set; }
        public string Device { get; set; }
        public string Timestamp { get; set; }

        public MeasurementDto(){}

        public MeasurementDto(Measurement m){
                this.MeasurementId = (int) m.MeasurementId;

                this.Measurements = new List<MeasurementEntity>(m.MeasurementEntity).ConvertAll(me => new MeasurementEntityDto(me));

                this.Device = m.Device;
                this.Timestamp = m.Timestamp;

                this.PositionGroundTruth = new PositionDto(
                    m.LatitudeGroundTruth,
                    m.LongitudeGroundTruth,
                    m.AltitudeGroundTruth,
                    m.AccuracyGroundTruth
                );

                if (m.LatitudeHighAccuracy.HasValue && m.LongitudeHighAccuracy.HasValue && m.AltitudeHighAccuracy.HasValue && m.AccuracyHighAccuracy.HasValue){
                    this.PositionHighAccuracy = new PositionDto(
                        (double) m.LatitudeHighAccuracy,
                        (double) m.LongitudeHighAccuracy,
                        (double) m.AltitudeHighAccuracy,
                        (double) m.AccuracyHighAccuracy
                    );
                }

                if (m.LatitudeHighAccuracy.HasValue && m.LongitudeHighAccuracy.HasValue && m.AltitudeHighAccuracy.HasValue && m.AccuracyHighAccuracy.HasValue){
                    this.PositionLowAccuracy = new PositionDto(
                        (double) m.LatitudeLowAccuracy,
                        (double) m.LongitudeLowAccuracy,
                        (double) m.AltitudeLowAccuracy,
                        (double) m.AccuracyLowAccuracy
                    );
                }
                
        }

        public Measurement toMeasurement() {
            var m = new Measurement(){
                MeasurementEntity = this.Measurements.ConvertAll(m => m.toMeasurementEntity()),

                Device = this.Device,
                Timestamp = this.Timestamp,

                LatitudeGroundTruth = this.PositionGroundTruth.Latitude,
                LongitudeGroundTruth = this.PositionGroundTruth.Longitude,
                AltitudeGroundTruth = this.PositionGroundTruth.Altitude,
                AccuracyGroundTruth = this.PositionGroundTruth.Accuracy,

                LatitudeHighAccuracy = this.PositionHighAccuracy.Latitude,
                LongitudeHighAccuracy = this.PositionHighAccuracy.Longitude,
                AltitudeHighAccuracy = this.PositionHighAccuracy.Altitude,
                AccuracyHighAccuracy = this.PositionHighAccuracy.Accuracy,

                LatitudeLowAccuracy = this.PositionLowAccuracy.Latitude,
                LongitudeLowAccuracy = this.PositionLowAccuracy.Longitude,
                AltitudeLowAccuracy = this.PositionLowAccuracy.Altitude,
                AccuracyLowAccuracy = this.PositionLowAccuracy.Accuracy,
            };

            return m;
        }

    }

    internal class List : List<MeasurementEntityDto>
    {
        private ICollection<MeasurementEntity> measurementEntity;

        public List(ICollection<MeasurementEntity> measurementEntity)
        {
            this.measurementEntity = measurementEntity;
        }
    }
}