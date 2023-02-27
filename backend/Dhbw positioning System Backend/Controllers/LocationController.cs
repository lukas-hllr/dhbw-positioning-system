using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Dhbw_positioning_System_Backend.Model;

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
            return new Position(49.02721308, 8.38568593, 116, 1);
        }
    }
}
