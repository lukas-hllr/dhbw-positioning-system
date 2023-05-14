using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        private readonly RayCastingAlgorithm _rayCastingAlgorithm;

        public LocationController(DhbwPositioningSystemDBContext context, RayCastingAlgorithm rayCastingAlgorithm)
        {
            _context = context;
            _rayCastingAlgorithm = rayCastingAlgorithm;
        }

        // POST: getLocation
        [HttpPost]
        public ActionResult<LocationDto> GetLocation(IEnumerable<MeasurementEntityDto> aps)
        {
            List<MeasurementEntityDto> apsFiltered = ExcludeDuplicates(aps);
            List<double> distances = new List<double>();
            List<GeoCoordinate> coordinates = new List<GeoCoordinate>();

            foreach (MeasurementEntityDto ap in apsFiltered)
            {
                AccessPoint correspondingAp = _context.AccessPoint.Find(
                    ap.Mac.Remove(16, 1).ToLower() + "0"
                );

                if (correspondingAp == null) continue;
                distances.Add(ap.Ssid.Equals("DHBW-KA5")
                    ? RSSItoDistanceConverter.ConvertWithFormula5G(ap.Rssi)
                    : RSSItoDistanceConverter.ConvertWithFormula2G(ap.Rssi));


                coordinates.Add(new GeoCoordinate(correspondingAp.Latitude, correspondingAp.Longitude));
            }

            if (distances.Count < 3)
            {
                return BadRequest("Trilateration requires at least 3 distinct and registered data points.");
            }

            Multilateration calculator = new Multilateration(coordinates.ToArray(), distances.ToArray());
            GeoCoordinate result = calculator.FindOptimalLocationLBFGS();
            double accuracy = calculator.Error(result);
            string room = _rayCastingAlgorithm.GetRoom(result);
            string closestDoor = _rayCastingAlgorithm.GetClosestDoor(result);

            return new LocationDto(result.Latitude, result.Longitude, -1, accuracy, room, closestDoor);
        }

        /*
            Prioritize 5GHz Networks and filter out
            redundant 2.4Ghz Networks
        */
        private List<MeasurementEntityDto> ExcludeDuplicates(IEnumerable<MeasurementEntityDto> aps)
        {
            aps = aps.OrderByDescending(ap => ap.Ssid);

            List<MeasurementEntityDto> filtered = new List<MeasurementEntityDto>();
            List<string> macs = new List<string>();

            foreach (MeasurementEntityDto ap in aps)
            {
                string currentMac = ap.Mac.Remove(16, 1).ToLower();

                if (!macs.Contains(currentMac))
                {
                    filtered.Add(ap);
                    macs.Add(currentMac);
                }
            }

            return filtered;
        }
    }
}