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
}