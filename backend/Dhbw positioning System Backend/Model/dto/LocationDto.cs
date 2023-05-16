namespace Dhbw_positioning_System_Backend.Model.dto
{
    public class LocationDto : PositionDto
    {
        public LocationDto(double lat, double lon, double alt, double acc, string room, string closestDoor){
            this.Latitude = lat;
            this.Longitude = lon;
            this.Altitude = alt;
            this.Accuracy = acc;
            this.Room = room;
            this.ClosestDoor = closestDoor;
        }
        public string Room { get; set; }
        public string ClosestDoor { get; set; }

    }
}