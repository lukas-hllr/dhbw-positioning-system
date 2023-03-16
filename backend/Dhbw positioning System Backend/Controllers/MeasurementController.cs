using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Dhbw_positioning_System_Backend.Calculation;
using Dhbw_positioning_System_Backend.Model;
using Dhbw_positioning_System_Backend.Model.dto;
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

        // GET: /Measurement (All Measurements)
        [HttpGet]
        public IEnumerable<MeasurementDto> GetAllMeasurements()
        {
            return _context.Measurement.ToList().ConvertAll(m => new MeasurementDto(m));
        }
        
        // GET: /Measurement (Measurement by ID)
        [HttpGet("{MeasurementId:long}", Name = "GetMeasurement")]
        public ActionResult<MeasurementDto> GetMeasurement(long MeasurementId)
        {
            var m = _context.Measurement.Find(MeasurementId);

            if (m == null) {
                return NotFound();
            } 

            return new MeasurementDto(m);
        }

        //POST /Measurement (new Measurement)
        [HttpPost(Name = "NewMeasurement")]
        public ActionResult NewMeasurement(MeasurementDto mDto)
        {
            var m = mDto.toMeasurement();

            _context.Measurement.Add(m);

            _context.SaveChanges();

            return CreatedAtRoute("GetMeasurement", new {m.MeasurementId}, new MeasurementDto(m));
        }
    }
}
