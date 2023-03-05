

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dhbw_positioning_System_Backend.Model
{
    public partial class AccessPoint
    {
        public string MacAddress { get; set; }
        // TODO: Coordinates NOT NULL
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Room { get; set; }
        public long RouterTypeId { get; set; }

        public virtual RouterType RouterType { get; set; }
    }
}
