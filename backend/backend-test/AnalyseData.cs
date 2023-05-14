using System.Diagnostics;
using System.Text;
using Dhbw_positioning_System_Backend;
using Dhbw_positioning_System_Backend.Calculation;
using Dhbw_positioning_System_Backend.Model;
using Dhbw_positioning_System_Backend.Model.dto;
using Dhbw_positioning_System_Backend.Controllers;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;

namespace backend_test.TestData;

public class AnalyseData
{
    private DhbwPositioningSystemDBContext _context;
    private RayCastingAlgorithm _rayCastingAlgorithm;
    private LocationController _locationController;

    [SetUp]
    public void SetUp()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DhbwPositioningSystemDBContext>()
            .UseSqlite(
                "Data Source=..\\..\\..\\..\\Dhbw positioning System Backend\\DhbwPositioningSystemDB.db;Mode=ReadOnly") //Standart Path is in dhbw-positioning-system\backend\backend-test\bin\Debug\net6.0 which is fucked, Idk how to fix so thats why weird navigation.
            .UseLazyLoadingProxies()
            .Options;
        _context = new DhbwPositioningSystemDBContext(optionsBuilder);
        _context.Database.OpenConnection();

        _rayCastingAlgorithm = new RayCastingAlgorithm();
        _locationController = new LocationController(_context, _rayCastingAlgorithm);
    }

    [Test]
    public void testAlgorithmPerformance()
    {
        Random random = new Random();
        int randomMeasurement = random.Next(_context.Measurement.Count());
        ExtractDistancesAndAps(1, out var distances, out var coordinates);
        Multilateration multilateration = new Multilateration(coordinates.ToArray(), distances.ToArray());

        Benchmark(() => { multilateration.FindOptimalLocationLBFGS(); }, 100000);
        Benchmark(() => { multilateration.FindOptimalLocationLM(); }, 100000);
    }
    private static void Benchmark(Action act, int iterations)
    {
        GC.Collect();
        act.Invoke(); // run once outside of loop to avoid initialization costs
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            act.Invoke();
        }
        sw.Stop();
        Console.WriteLine((sw.ElapsedMilliseconds / (iterations+0.0)).ToString());
    }

    [Test]
    public void testAlgorithmsAccuracy()
    {
        var measurements = _context.Measurement.ToList();
        List<GeoCoordinate> gTList = new List<GeoCoordinate>();
        List<GeoCoordinate> LMList = new List<GeoCoordinate>();
        List<GeoCoordinate> BFSGList = new List<GeoCoordinate>();
        
        foreach (var m in measurements)
        {
            ExtractDistancesAndAps(m.MeasurementId, out var distances, out var coordinates);

            if (coordinates.Count < 3) continue;
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

    private void ExtractDistancesAndAps(long measurementId, out List<double> distances, out List<GeoCoordinate> coordinates)
    {
        List<MeasurementEntity> dataPoints =
            ExcludeDuplicates(_context.MeasurementEntity.Where(mE => mE.MeasurementId == measurementId));
        distances = new List<double>();
        coordinates = new List<GeoCoordinate>();
        foreach (MeasurementEntity ap in dataPoints)
        {
            AccessPoint? correspondingAp = _context.AccessPoint.Find(
                ap.Mac.Remove(16, 1).ToLower() + "0"
            );

            if (correspondingAp == null) continue;
            distances.Add(RSSItoDistanceConverter.ConvertWithRegression(ap.Rssi)); //Konvertierung von RSSI zu Distanz
            coordinates.Add(new GeoCoordinate(correspondingAp.Latitude, correspondingAp.Longitude));
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

    [Test]
    public void testDistancesToGroundTruthAndLocationServices(){

        var res = calcDbMeasurements();

        Assert.AreEqual(0, 0);
    }

    private Dictionary<string, dynamic> calcDbMeasurements(bool writeCsv = true){
        var result = new Dictionary<string, dynamic>();

        var groundTruthList = new List<GeoCoordinate>();
        var calculatedList = new List<LocationDto>();
        var locationServicesList = new List<GeoCoordinate>();
        var distanceToCalculatedList = new List<double>();
        var distanceToLocationServicesList = new List<double>();

        foreach (var m in _context.Measurement)
        {
            var measurementEntities = new List<MeasurementEntityDto>();

            foreach(var me in m.MeasurementEntity)
            {
                measurementEntities.Add(new MeasurementEntityDto(me));
            }

            var groundTruth = new GeoCoordinate(m.LatitudeGroundTruth, m.LongitudeGroundTruth);
            var calculated = _locationController.GetLocation(measurementEntities).Value!;
            var locationServices = new GeoCoordinate((double)m.LatitudeHighAccuracy, (double)m.LongitudeHighAccuracy) { VerticalAccuracy = (double)m.AccuracyHighAccuracy };

            groundTruthList.Add(groundTruth);
            locationServicesList.Add(locationServices);
            calculatedList.Add(calculated);
            distanceToCalculatedList.Add(new GeoCoordinate(calculated.Latitude, calculated.Longitude).GetDistanceTo(groundTruth));
            distanceToLocationServicesList.Add(new GeoCoordinate(calculated.Latitude, calculated.Longitude).GetDistanceTo(locationServices));

        }

        result.Add("groundTruth", groundTruthList);
        result.Add("calculated", calculatedList);
        result.Add("locationServices", locationServicesList);
        result.Add("distanceToCalculated", distanceToCalculatedList);
        result.Add("distanceToLocationServices", distanceToLocationServicesList);

        if (writeCsv)
        {
            writeResultsToCsv(result);
        }

        return result;
    }

    private void writeResultsToCsv(Dictionary<string, dynamic> res)
    {
        var filePath = Path.Join(Directory.GetParent(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)).Parent.FullName, "results.csv");
        var csv = new StringBuilder();
        var csvHeader = "lat;lng;latCalculated;lngCalculated;accCalculated;distanceToCalculated;latLocationServices;lngLocationServices;accCalculated;distanceToLocationServices";
        csv.AppendLine(csvHeader);

        var groundTruthList = res["groundTruth"]! as List<GeoCoordinate>;
        var calculatedList = res["calculated"]! as List<LocationDto>;
        var locationServicesList = res["locationServices"] as List<GeoCoordinate>;
        var distanceToCalculatedList = res["distanceToCalculated"] as List<double>;
        var distanceToLocationServicesList = res["distanceToLocationServices"] as List<double>;

        for (int i = 0; i < groundTruthList.Count; i++)
        {
            var newLine = string.Format(
                "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                groundTruthList.ElementAt(i).Latitude,
                groundTruthList.ElementAt(i).Longitude,
                calculatedList.ElementAt(i).Latitude,
                calculatedList.ElementAt(i).Longitude,
                calculatedList.ElementAt(i).Accuracy,
                distanceToCalculatedList.ElementAt(i),
                locationServicesList.ElementAt(i).Latitude,
                locationServicesList.ElementAt(i).Longitude,
                locationServicesList.ElementAt(i).VerticalAccuracy,
                distanceToLocationServicesList.ElementAt(i)
            );
            csv.AppendLine(newLine);
        }

        File.WriteAllText(filePath, csv.ToString());
    }
}