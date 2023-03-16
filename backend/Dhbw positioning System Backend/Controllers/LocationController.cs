using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Dhbw_positioning_System_Backend.Model;
using Dhbw_positioning_System_Backend.Model.dto;
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

        public ActionResult<PositionDto> getLocation(IEnumerable<MeasurementEntityDto> aps)
        {

            List<MeasurementEntityDto> aps_filtered = this.excludeDuplicates(aps);

            List<double> distances = new List<double>();
            List<GeoCoordinate> coordinates = new List<GeoCoordinate>();

            foreach (MeasurementEntityDto ap in aps_filtered)
            {
                AccessPoint correspondingAp = _context.AccessPoint.Find(
                    ap.Mac.Remove(16, 1).ToLower() + "0"
                );

                if (correspondingAp != null)
                {
                    distances.Add(RSSItoDistanceConverter.Convert(ap));
                    coordinates.Add(new GeoCoordinate(correspondingAp.Latitude, correspondingAp.Longitude));
                }
            }

            if (distances.Count<double>() < 3)
            {
                return BadRequest("Trilateration requires at least 3 distinct and registered datapoints.");
            }


            GeoCoordinate result = Trilateration.IterativeIntersection(
                coordinates.ToArray<GeoCoordinate>(),
                distances.ToArray<double>(),
                5
            );
            
            return new PositionDto(result.Latitude, result.Longitude, -1, 1);
        }

        /*
            Priorise 5GHz Networks and filter out
            duplicate 2.4Ghz Networks
        */
        private List<MeasurementEntityDto> excludeDuplicates(IEnumerable<MeasurementEntityDto> aps){
            aps = aps.OrderByDescending(ap => ap.Ssid);

            List<MeasurementEntityDto> filtered = new List<MeasurementEntityDto>();
            List<string> macs = new List<string>();
            
            foreach (MeasurementEntityDto ap in aps)
            {
                string current_mac = ap.Mac.Remove(16, 1).ToLower();

                if (!macs.Contains(current_mac)){
                    filtered.Add(ap);
                    macs.Add(current_mac);
                }
            }

            return filtered;
        }
    }
}
