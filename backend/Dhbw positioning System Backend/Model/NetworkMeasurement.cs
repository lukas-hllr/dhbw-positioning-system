using System;
using System.Collections.Generic;

namespace Dhbw_positioning_System_Backend.Model
{
    public partial class NetworkMeasurement
    {
        public long NetworkMeasurementId { get; set; }
        public string NetworkSsid { get; set; }
        public double MeasuredStrength { get; set; }
        public long MeasurementId { get; set; }
        public string MacAddress { get; set; }

        public virtual AccessPoint MacAddressNavigation { get; set; }
        public virtual Measurement Measurement { get; set; }
    }
}
