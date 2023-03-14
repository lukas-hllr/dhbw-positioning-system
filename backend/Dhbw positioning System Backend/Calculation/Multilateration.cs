using System;
using GeoCoordinatePortable;

namespace Dhbw_positioning_System_Backend.Calculation;

public class Multilateration
{
    private readonly GeoCoordinate[] _knownRouter;
    private readonly double[] _distances;
    public Multilateration(GeoCoordinate[] knownRouter, double[] distances)
    {
        _knownRouter = knownRouter;
        _distances = distances;
    }

    public GeoCoordinate FindOptimalLocation()
    {
        double[] x = {_knownRouter[0].Latitude,_knownRouter[0].Longitude};
        const double epsg = 0.0000000001;
        const double epsf = 0;
        const double epsx = 0;
        const double diffstep = 1.0e-6;
        const int maxits = 0;
        alglib.minlbfgsstate state;
        alglib.minlbfgsreport rep;

        alglib.minlbfgscreatef(1, x, diffstep, out state);
        alglib.minlbfgssetcond(state, epsg, epsf, epsx, maxits);
        alglib.minlbfgsoptimize(state, ErrorFunction, null, null);
        alglib.minlbfgsresults(state, out x, out rep);
        GeoCoordinate result = new GeoCoordinate(x[0], x[1]);
        return result;
    }

    private void ErrorFunction(double[] x, ref double function, object obj)
    {
        GeoCoordinate initialGuess = new GeoCoordinate(x[0], x[1]);
        function = MeanSquaredError(initialGuess);
    }
    
    private double MeanSquaredError(GeoCoordinate estimatedLocation)
    {
        int n = this._knownRouter.Length;
        double result = 0.0;

        for (int i = 0; i < n; i++)
        {
            double estimatedDistance = estimatedLocation.GetDistanceTo(_knownRouter[i]);
            result += Math.Pow(estimatedDistance - _distances[i], 2);
        }
        return result/n;
    }
    
}