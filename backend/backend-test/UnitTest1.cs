using System.Runtime.Intrinsics.Arm;
using Dhbw_positioning_System_Backend.Calculation;
using Dhbw_positioning_System_Backend.Model;
using Dhbw_positioning_System_Backend.Model.dto;
using GeoCoordinatePortable;

namespace backend_test;

using NUnit.Framework;

public class Tests
{
    [Test]
    public void TestLateration()
    {
        GeoCoordinate[] knownRoutersGeoCord =
        {
            new GeoCoordinate(49.02732553, 8.38536608),
            new GeoCoordinate(49.02719075, 8.38552274),
            new GeoCoordinate(49.0272082, 8.38570786)
        };

        double[] distances = new double[] { 15.7, 4.8, 13.2 };

        var result = Trilateration.IterativeIntersection(knownRoutersGeoCord, distances, 2);
        Assert.That(result.Latitude, Is.EqualTo(49.02724149333333).Within(0.001));
        Assert.That(result.Longitude, Is.EqualTo(8.385532226666667).Within(0.001));
    }

    [Test]
    public void TestLateration2()
    {
        GeoCoordinate[] knownRoutersGeoCord =
        {
            new GeoCoordinate(49.0272880184736, 8.38570350114179),
            new GeoCoordinate(49.0273064641865, 8.38550658162981),
            new GeoCoordinate(49.0271643231129, 8.38584581316988)
        };

        double[] distances = new double[] { 5, 10, 18 };
        Multilateration multilateration = new Multilateration(knownRoutersGeoCord, distances);
        var result = multilateration.FindOptimalLocation();
        Console.WriteLine(result);
        Assert.That(result.Latitude, Is.EqualTo(49.027217178260145).Within(0.0001));
        Assert.That(result.Longitude, Is.EqualTo(8.38570374903793).Within(0.0001));
    }

    [Test]
    public void TestLateration3()
    {
        GeoCoordinate[] knownRoutersGeoCord =
        {
            new GeoCoordinate(49.0272880184736, 8.38570350114179),
            new GeoCoordinate(49.0273064641865, 8.38550658162981),
            new GeoCoordinate(49.0271643231129, 8.38584581316988),
            new GeoCoordinate(49.0273249104467, 8.38533117269256),
            new GeoCoordinate(49.0271523880134, 8.38526332626088),
            new GeoCoordinate(49.0271380365763, 8.38539400717443),
        };

        double[] distances = new double[]
        {
            5.843414133735177, 58.434141337351754, 58.434141337351754, 107.97751623277094, 116.59144011798323,
            125.89254117941675
        };
        Multilateration multilateration = new Multilateration(knownRoutersGeoCord, distances);
        var result = multilateration.FindOptimalLocation();
        Console.WriteLine(result);
        Assert.That(result.Latitude, Is.EqualTo(49.02751575703979).Within(0.0001));
        Assert.That(result.Longitude, Is.EqualTo(8.386484553128351).Within(0.0001));
    }

    [Test]
    public void TestRssiConverter()
    {
        var dp = new MeasurementEntityDto()
        {
            Rssi = -87,
            Ssid = "DHBW-KA5"
        };
        var result = RSSItoDistanceConverter.Convert(dp);
        Console.WriteLine(result);
        Assert.That(result, Is.Positive);
    }

    [Test]
    public void TestRayCastingWithPointInAudimax()
    {
        RayCastingAlgorithm rc = new RayCastingAlgorithm();
        GeoCoordinate audimax = new GeoCoordinate(49.02700149361377, 8.385664256051086);
        string result = rc.GetRoom(audimax);
        Assert.That(result, Is.EqualTo("audimax"));
    }

    [Test]
    public void TestRayCastingWithPointOutside()
    {
        RayCastingAlgorithm rc = new RayCastingAlgorithm();
        GeoCoordinate pointOutside = new GeoCoordinate(49.026994, 8.385271);
        string result = rc.GetRoom(pointOutside);
        Assert.That(result, Is.Null);
    }


    [Test]
    public void AbusingAsAMain()
    {
        List<double[]> coordinates = new List<double[]> {
            new double[] { 8.385570490303623, 49.02685019084306 },
            new double[] { 8.385575703581326, 49.02687383312321 },
            new double[] { 8.385647104316815, 49.026866917465505 },
            new double[] { 8.385704184695946, 49.02687890785823 },
            new double[] { 8.38574095401178, 49.027045656180164 },
            new double[] { 8.385691563768605, 49.02706854018315 },
            new double[] { 8.385643447456738, 49.02707308909208 },
            new double[] { 8.385618280449936, 49.02707546832719 },
            new double[] { 8.385623461200174, 49.02709896269424 },
            new double[] { 8.385604471434805, 49.02709549178795 },
            new double[] { 8.385599144829467, 49.027071335804365 },
            new double[] { 8.385534093388914, 49.02706131996549 },
            new double[] { 8.385499115470612, 49.026902695656915 },
            new double[] { 8.385558735967296, 49.02688048255547 },
            new double[] { 8.385553468534818, 49.02685659489871 },
            new double[] { 8.385570490303623, 49.02685019084306 }
        };

        foreach (double[] coord in coordinates) {
            Console.WriteLine("new GeoCoordinate(longitude:{0}, latitude:{1}),", coord[0], coord[1]);
        }
        
    }
}