using System;
using System.Collections.Generic;

namespace Dhbw_positioning_System_Backend.Model
{
    public partial class Measurement
    {
        public Measurement()
        {
            MeasurementEntity = new HashSet<MeasurementEntity>();
        }

        public long MeasurementId { get; set; }
        public double LatitudeGroundTruth { get; set; }
        public double LongitudeGroundTruth { get; set; }
        public double AltitudeGroundTruth { get; set; }
        public double AccuracyGroundTruth { get; set; }
        public double? LatitudeHighAccuracy { get; set; }
        public double? LongitudeHighAccuracy { get; set; }
        public double? AltitudeHighAccuracy { get; set; }
        public double? AccuracyHighAccuracy { get; set; }
        public double? LatitudeLowAccuracy { get; set; }
        public double? LongitudeLowAccuracy { get; set; }
        public double? AltitudeLowAccuracy { get; set; }
        public double? AccuracyLowAccuracy { get; set; }
        public string Device { get; set; }
        public string Timestamp { get; set; }

        public virtual ICollection<MeasurementEntity> MeasurementEntity { get; set; }
    }
}
