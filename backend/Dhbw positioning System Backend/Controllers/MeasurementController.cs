using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Dhbw_positioning_System_Backend.Calculation;
using Dhbw_positioning_System_Backend.Model;
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
        public IEnumerable<Measurement> GetAllMeasurements()
        {
            return _context.Measurement.ToList();
        }
        
        // GET: Measurement
        [HttpGet("{MeasurementId:long}", Name = "GetMeasurement")]
        public ActionResult<Measurement> GetMeasurement(long MeasurementId)
        {
            var m = _context.Measurement.Find(MeasurementId);

            if (m == null) {
                return NotFound();
            } 

            return m;
        }

        //POST /Measurement
        [HttpPost(Name = "NewMeasurement")]
        public ActionResult NewMeasurement(DataSet ds)
        {
            var m = new Measurement(){
                MeasurementEntity = ds.Measurements,
                Device = ds.Device,
                Timestamp = ds.Timestamp,

                LatitudeGroundTruth = ds.PositionGroundTruth.Latitude,
                LongitudeGroundTruth = ds.PositionGroundTruth.Longitude,
                AltitudeGroundTruth = ds.PositionGroundTruth.Altitude,
                AccuracyGroundTruth = ds.PositionGroundTruth.Accuracy,

                LatitudeHighAccuracy = ds.PositionHighAccuracy.Latitude,
                LongitudeHighAccuracy = ds.PositionHighAccuracy.Longitude,
                AltitudeHighAccuracy = ds.PositionHighAccuracy.Altitude,
                AccuracyHighAccuracy = ds.PositionHighAccuracy.Accuracy,

                LatitudeLowAccuracy = ds.PositionLowAccuracy.Latitude,
                LongitudeLowAccuracy = ds.PositionLowAccuracy.Longitude,
                AltitudeLowAccuracy = ds.PositionLowAccuracy.Altitude,
                AccuracyLowAccuracy = ds.PositionLowAccuracy.Accuracy,

            };
            _context.Measurement.Add(m);

            _context.SaveChanges();

            return Ok();
        }
    }
}
