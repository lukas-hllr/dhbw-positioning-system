﻿namespace Dhbw_positioning_System_Backend.Model.dto
{
    public class PositionDto
    {
        public PositionDto(){}
        public PositionDto(double lat, double lon, double alt, double acc){
            this.Latitude = lat;
            this.Longitude = lon;
            this.Altitude = alt;
            this.Accuracy = acc;

        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Accuracy { get; set; }

    }
}