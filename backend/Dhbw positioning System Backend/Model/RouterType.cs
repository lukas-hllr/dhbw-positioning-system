using System;
using System.Collections.Generic;

namespace Dhbw_positioning_System_Backend.Model
{
    public partial class RouterType
    {
        public RouterType()
        {
            AccessPoint = new HashSet<AccessPoint>();
        }

        public long RouterTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccessPoint> AccessPoint { get; set; }
    }
}
