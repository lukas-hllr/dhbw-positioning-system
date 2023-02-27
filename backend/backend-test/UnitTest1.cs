using Dhbw_positioning_System_Backend.Calculation;
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

        double[] distances = new double[] {15.7, 4.8, 13.2};
        var result = Trilateration.IterativeIntersection(knownRoutersGeoCord, distances, 2);
        Assert.That(result.Latitude, Is.EqualTo(49.02724149333333).Within(0.001));
        Assert.That(result.Longitude, Is.EqualTo(8.385532226666667).Within(0.001));
    }
}