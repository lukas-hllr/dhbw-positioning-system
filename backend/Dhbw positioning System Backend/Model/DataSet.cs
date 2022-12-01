using System.Collections.Generic;

namespace Dhbw_positioning_System_Backend.Model
{
    public class DataSet
    {
        public List<DataPoint> Measurements { get; set; }
        public string Timestamp { get; set; }
        public Position PositionHighAccuracy { get; set; }
        public Position PositionLowAccuracy { get; set; }


    }
}