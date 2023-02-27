using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Dhbw_positioning_System_Backend.Model;
using Dhbw_positioning_System_Backend.Calculation;
using GeoCoordinatePortable;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dhbw_positioning_System_Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly DhbwPositioningSystemDBContext _context;

        public LocationController(DhbwPositioningSystemDBContext context)
        {
            _context = context;
        }

        // POST: getLocation
        [HttpPost]

        public ActionResult<Position> getLocation(IEnumerable<DataPoint> aps)
        {
            List<double> distances = new List<double>();
            List<GeoCoordinate> coordinates = new List<GeoCoordinate>();

            foreach (var ap in aps)
            {
                AccessPoint correspondingAp = _context.AccessPoint.Find(
                    ap.MAC.Remove(16, 1).ToLower() + "0"
                );

                if (correspondingAp != null)
                {
                    distances.Add(RSSItoDistanceConverter.Convert(ap.Level));
                    coordinates.Add(new GeoCoordinate(correspondingAp.Latitude, correspondingAp.Longitude));
                }
            }

            if (distances.Count<double>() < 3)
            {
                return BadRequest("Trilateration requires at least 3 registered datapoints.");
            }


            GeoCoordinate result = Trilateration.IterativeIntersection(
                coordinates.ToArray<GeoCoordinate>(),
                distances.ToArray<double>(),
                5
            );
            
            return new Position(result.Latitude, result.Longitude, -1, 1);
        }
    }
}
