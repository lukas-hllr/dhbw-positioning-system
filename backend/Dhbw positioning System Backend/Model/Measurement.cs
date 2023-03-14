using System;
using System.Collections.Generic;

namespace Dhbw_positioning_System_Backend.Model
{
    public partial class Measurement
    {
        public Measurement()
        {
            NetworkMeasurement = new HashSet<NetworkMeasurement>();
        }

        public long MeasurementId { get; set; }
        public string Date { get; set; }
        public double LatitudeGroundTruth { get; set; }
        public double LongitudeGroundTruth { get; set; }
        public double? LatitudeHighAccuracy { get; set; }
        public double? LongitudeHighAccuracy { get; set; }
        public double? LatitudeLowAccuracy { get; set; }
        public double? LongitudeLowAccuracy { get; set; }
        public string Device { get; set; }

        public virtual ICollection<NetworkMeasurement> NetworkMeasurement { get; set; }
    }
}
