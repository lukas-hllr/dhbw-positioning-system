using System;
using GeoCoordinatePortable;

namespace Dhbw_positioning_System_Backend.Calculation;

public static class Trilateration
{
    public static GeoCoordinate IterativeIntersection(GeoCoordinate[] positions, double[] distances, double epsilon = 2)
    {
        // Check that the number of positions and distances are the same
        if (positions.Length != distances.Length)
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
        GeoCoordinate position = new GeoCoordinate(lat, lon);

        // Iterate until the position estimate converges
        while (true)
        {
            // Calculate the distances from the current estimate to each reference point
            double[] currentDistances = new double[distances.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                currentDistances[i] = positions[i].GetDistanceTo(position);
            }

            // Check if the current estimate meets the required accuracy
            double error = 0;
            for (int i = 0; i < distances.Length; i++)
            {
                error += Math.Abs(currentDistances[i] - distances[i]);
            }
            if (error < epsilon)
                break;

            // Update the position estimate using a weighted least-squares algorithm
            double[] weights = new double[distances.Length];
            double sum = 0;
            for (int i = 0; i < distances.Length; i++)
            {
                weights[i] = 1.0 / currentDistances[i];
                sum += weights[i];
            }
            double latSum = 0, lonSum = 0;
            for (int i = 0; i < distances.Length; i++)
            {
                latSum += weights[i] * positions[i].Latitude;
                lonSum += weights[i] * positions[i].Longitude;
            }
            lat = latSum / sum;
            lon = lonSum / sum;
            position = new GeoCoordinate(lat, lon);
        }

        return position;
    }
}