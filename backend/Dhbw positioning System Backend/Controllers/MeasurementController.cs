using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dhbw_positioning_System_Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly DhbwPositioningSystemDBContext _context;

        public MeasurementController(DhbwPositioningSystemDBContext context)
        {
            _context = context;
        }

        // GET: Measurement
        [HttpGet]
        public IEnumerable<Measurement> Get()
        {
            return _context.Measurement.ToList();
        }

        // POST api/<MeasurementController>
        [HttpPost]
        public ActionResult<Measurement> Post(Measurement measurement)
        {
            var e = _context.Measurement.Add(new Measurement()
            {
                Date = measurement.Date,
                Latitude = measurement.Latitude,
                Longitude = measurement.Longitude,
                NetworkMeasurement = measurement.NetworkMeasurement
            });

            _context.SaveChanges();
            return Ok();
        }

        // PUT api/<MeasurementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MeasurementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
