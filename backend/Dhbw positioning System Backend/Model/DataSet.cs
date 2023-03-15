using System.Collections.Generic;

namespace Dhbw_positioning_System_Backend.Model
{
    public class DataSet
    {
        public List<MeasurementEntity> Measurements { get; set; }
        public Position PositionGroundTruth { get; set; }
        public Position PositionHighAccuracy { get; set; }
        public Position PositionLowAccuracy { get; set; }
        public string Device { get; set; }
        public string Timestamp { get; set; }


    }
}