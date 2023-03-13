using System;
using System.Linq;
using GeoCoordinatePortable;

namespace Dhbw_positioning_System_Backend.Calculation;

public static class Trilateration
{
    public static GeoCoordinate IterativeIntersection(GeoCoordinate[] positions, double[] trueDistances, double epsilon = 0.1)
    {
        // Check that the number of positions and distances are the same
        if (positions.Length != trueDistances.Length)
            throw new ArgumentException("The number of positions and distances must be the same.");

        // Initialize the position estimate to the centroid of the reference points
        double lat = 0, lon = 0;
        foreach (GeoCoordinate p in positions)
        {
            lat += p.Latitude;
            lon += p.Longitude;
        }
        lat /= positions.Length;
        lon /= positions.Length;
        GeoCoordinate estimatedPosition = new GeoCoordinate(lat, lon);
        double accuracy=9999, lastAccuracy=99999;
        // Iterate until the position estimate converges (Which happens when the error is not getting better)
        while (GetAccuracyImprovement(accuracy,lastAccuracy)<epsilon)
        {
            // Calculate the distances from the current estimate to each reference point
            double[] calculatedDistances = GetCalculatedDistances(positions, estimatedPosition);

            // Check if the current estimate meets the required accuracy
            lastAccuracy = accuracy;
            accuracy = CalculatedAccuracy(trueDistances, calculatedDistances);
            // Update the position estimate using a weighted least-squares algorithm
            estimatedPosition = UpdatePosition(positions, calculatedDistances);
        }
        return estimatedPosition;
    }
    private static double GetAccuracyImprovement(double error, double lastError)
    {
        return Math.Abs(lastError - error);
    }
    private static GeoCoordinate UpdatePosition(GeoCoordinate[] positions, double[] calculatedDistances)
    {
        int n = calculatedDistances.Length;
        double[] weights = new double[n];
        double sum = 0;
        for (int i = 0; i < n; i++)
        {
            weights[i] = 1.0 / calculatedDistances[i];
            sum += weights[i];
        }
        double latSum = 0, lonSum = 0;
        for (int i = 0; i < n; i++)
        {
            latSum += weights[i] * positions[i].Latitude;
            lonSum += weights[i] * positions[i].Longitude;
        }
        double lat = latSum / sum;
        double lon = lonSum / sum;
        return new GeoCoordinate(lat, lon);
    }
    private static double CalculatedAccuracy(double[] trueDistances, double[] calculatedDistances)
    {
        return trueDistances.Select((t, i) => Math.Abs(calculatedDistances[i] - t)).Sum();
    }
    private static double[] GetCalculatedDistances(GeoCoordinate[] positions,GeoCoordinate calculatedPosition)
    {
        int n = positions.Length; 
        double[] calculatedDistances = new double[n];
        for (int i = 0; i < n; i++)
        {
            calculatedDistances[i] = positions[i].GetDistanceTo(calculatedPosition);
        }
        return calculatedDistances;
    }

    private static double MeanSquaredError(GeoCoordinate estimatedLocation, GeoCoordinate[] knownPoints, double[] distances)
    {
        int n = knownPoints.Length;
        double result = 0.0;

        for (int i = 0; i < n; i++)
        {
            double estimatedDistance = estimatedLocation.GetDistanceTo(knownPoints[i]);
            result += Math.Pow(estimatedDistance - distances[i], 2);
        }
        return result/n;
    }
    
}