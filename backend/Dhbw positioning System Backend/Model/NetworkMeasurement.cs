using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dhbw_positioning_System_Backend
{
    public partial class NetworkMeasurement
    {
        public long NetworkMeasurementId { get; set; }
        public string NetworkSsid { get; set; }
        public double? MeasuredStrength { get; set; }
        public long MeasurementId { get; set; }
        public string MacAddress { get; set; }

        public virtual Measurement Measurement { get; set; }
    }
}
