using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoCoordinatePortable;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace Dhbw_positioning_System_Backend.Calculation;

public class RayCastingAlgorithm
{
    List<Feature> geoData;
    public RayCastingAlgorithm()
    {
        string fileName = Path.Join(Directory.GetCurrentDirectory(), "2og_cal.json");
        string jsonString = File.ReadAllText(fileName);
        var featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(jsonString)!;
        geoData = featureCollection.Features
            .Where(e => e.Geometry is Polygon)
            .Where(e => e.Properties.ContainsKey("room"))
            .ToList();
    }

    public string GetClosestDoor(GeoCoordinate point)
    {
        return "tbd";
    }

    public string GetRoom(GeoCoordinate point)
    {
        foreach (Feature feature in geoData)
        {
            var c = (feature.Geometry as Polygon)
                .Coordinates[0]
                .Coordinates
                .ToList();
            if (CheckIfInside(c, point))
            {
                return feature.Properties["room"] as string;
            }
        }
        return null;
    }

    public static bool CheckIfInside(List<IPosition> room, GeoCoordinate point)
    {
        int count = room.Count;

        double x = point.Longitude;
        double y = point.Latitude;

        bool inside = false;
        for (int i = 0, j = count - 1; i < count; j = i++)
        {
            double xi = room[i].Longitude;
            double yi = room[i].Latitude;
            double xj = room[j].Longitude;
            double yj = room[j].Latitude;

            bool intersect = ((yi > y) != (yj > y)) && (x < (xj - xi) * (y - yi) / (yj - yi) + xi);
            if (intersect)
            {
                inside = !inside;
            }
        }

        return inside;
    }
}