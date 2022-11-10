﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dhbw_positioning_System_Backend
{
    public partial class Measurement
    {
        public Measurement()
        {
            NetworkMeasurement = new HashSet<NetworkMeasurement>();
        }

        public long MeasurementId { get; set; }
        public string Date { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public virtual ICollection<NetworkMeasurement> NetworkMeasurement { get; set; }
    }
}