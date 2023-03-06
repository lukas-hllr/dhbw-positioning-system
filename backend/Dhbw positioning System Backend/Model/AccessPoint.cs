using System;
using System.Collections.Generic;

namespace Dhbw_positioning_System_Backend.Model
{
    public partial class AccessPoint
    {
        public AccessPoint()
        {
            NetworkMeasurement = new HashSet<NetworkMeasurement>();
        }

        public string MacAddress { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Room { get; set; }
        public long RouterTypeId { get; set; }

        public virtual RouterType RouterType { get; set; }
        public virtual ICollection<NetworkMeasurement> NetworkMeasurement { get; set; }
    }
}
