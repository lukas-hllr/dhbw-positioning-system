using System;
using System.Collections.Generic;

namespace Dhbw_positioning_System_Backend.Model
{
    public partial class MeasurementEntity
    {
        public long MeasurementEntityId { get; set; }
        public long MeasurementId { get; set; }
        public string Ssid { get; set; }
        public string Mac { get; set; }
        public long Rssi { get; set; }

        public virtual Measurement Measurement { get; set; }
    }
}
