using Dhbw_positioning_System_Backend;
using Dhbw_positioning_System_Backend.Calculation;
using Dhbw_positioning_System_Backend.Model;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;

namespace backend_test.TestData;

public class AnalyseData
{
    private DhbwPositioningSystemDBContext _context;

    [SetUp]
    public void SetUp()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DhbwPositioningSystemDBContext>()
            .UseSqlite(
                "Data Source=..\\..\\..\\..\\Dhbw positioning System Backend\\DhbwPositioningSystemDB.db;Mode=ReadOnly") //Standart Path is in dhbw-positioning-system\backend\backend-test\bin\Debug\net6.0 which is fucked, Idk how to fix so thats why weird navigation.
            .Options;
        _context = new DhbwPositioningSystemDBContext(optionsBuilder);
        _context.Database.OpenConnection();
    }

    [Test]
    public void testAlgorithms()
    {
        var measurements = _context.Measurement.ToList();
        List<GeoCoordinate> gTList = new List<GeoCoordinate>();
        List<GeoCoordinate> LMList = new List<GeoCoordinate>();
        List<GeoCoordinate> BFSGList = new List<GeoCoordinate>();


        foreach (var m in measurements)
        {
            List<MeasurementEntity> dataPoints = ExcludeDuplicates(_context.MeasurementEntity.Where(mE=>mE.MeasurementId == m.MeasurementId));
            List<double> distances = new List<double>();
            List<GeoCoordinate> coordinates = new List<GeoCoordinate>();
            foreach (MeasurementEntity ap in dataPoints)
            {
                AccessPoint? correspondingAp = _context.AccessPoint.Find(
                    ap.Mac.Remove(16, 1).ToLower() + "0"
                );

                if (correspondingAp == null) continue;
                distances.Add(0.00001 * Math.Pow(ap.Rssi * -1, 3.5537)); //Konvertierung von RSSI zu Distanz
                coordinates.Add(new GeoCoordinate(correspondingAp.Latitude, correspondingAp.Longitude));
            }

            if (dataPoints.Count < 3) continue;
            Multilateration multilateration = new Multilateration(coordinates.ToArray(), distances.ToArray());
            var resultLM = multilateration.FindOptimalLocationLM();
            var resultBFGS = multilateration.FindOptimalLocationLBFGS();
            GeoCoordinate groundTruth = new GeoCoordinate(m.LatitudeGroundTruth, m.LongitudeGroundTruth);

            gTList.Add(groundTruth);
            BFSGList.Add(resultBFGS);
            LMList.Add(resultLM);
        }
        
        for (int i = 0; i < BFSGList.Count; i++)
        {
            Console.WriteLine(BFSGList[i].GetDistanceTo(gTList[i]));
        }
        Console.WriteLine();
        for (int i = 0; i < LMList.Count; i++)
        {
            Console.WriteLine(LMList[i].GetDistanceTo(gTList[i]));
        }
    }

    private List<MeasurementEntity> ExcludeDuplicates(IEnumerable<MeasurementEntity> aps)
    {
        aps = aps.OrderByDescending(ap => ap.Ssid);

        List<MeasurementEntity> filtered = new List<MeasurementEntity>();
        List<string> macs = new List<string>();

        foreach (MeasurementEntity ap in aps)
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