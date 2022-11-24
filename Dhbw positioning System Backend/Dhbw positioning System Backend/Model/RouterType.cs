using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dhbw_positioning_System_Backend
{
    public partial class RouterType
    {
        public RouterType()
        {
            AccessPoint = new HashSet<AccessPoint>();
        }

        public long RouterTypeId { get; set; }
        public string Name { get; set; }
        public double? Range { get; set; }

        public virtual ICollection<AccessPoint> AccessPoint { get; set; }
    }
}
