using System.Collections.Generic;
using GeoCoordinatePortable;
using Newtonsoft.Json.Linq;

namespace Dhbw_positioning_System_Backend.Calculation;

public class RayCastingAlgorithm
{
    public static readonly List<GeoCoordinate>Audimax = new List<GeoCoordinate>()
    {
        new GeoCoordinate(longitude:8.385570490303623, latitude:49.02685019084306),
        new GeoCoordinate(longitude:8.385575703581326, latitude:49.02687383312321),
        new GeoCoordinate(longitude:8.385647104316815, latitude:49.026866917465505),
        new GeoCoordinate(longitude:8.385704184695946, latitude:49.02687890785823),
        new GeoCoordinate(longitude:8.38574095401178, latitude:49.027045656180164),
        new GeoCoordinate(longitude:8.385691563768605, latitude:49.02706854018315),
        new GeoCoordinate(longitude:8.385643447456738, latitude:49.02707308909208),
        new GeoCoordinate(longitude:8.385618280449936, latitude:49.02707546832719),
        new GeoCoordinate(longitude:8.385623461200174, latitude:49.02709896269424),
        new GeoCoordinate(longitude:8.385604471434805, latitude:49.02709549178795),
        new GeoCoordinate(longitude:8.385599144829467, latitude:49.027071335804365),
        new GeoCoordinate(longitude:8.385534093388914, latitude:49.02706131996549),
        new GeoCoordinate(longitude:8.385499115470612, latitude:49.026902695656915),
        new GeoCoordinate(longitude:8.385558735967296, latitude:49.02688048255547),
        new GeoCoordinate(longitude:8.385553468534818, latitude:49.02685659489871),
        new GeoCoordinate(longitude:8.385570490303623, latitude:49.02685019084306)
    };



    public static bool CheckIfInside(List<GeoCoordinate> room, GeoCoordinate point)
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