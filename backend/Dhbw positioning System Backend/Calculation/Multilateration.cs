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

    public GeoCoordinate FindOptimalLocationLM()
    {
        double[] x = new double[]{0,0};
        double[] s = new double[]{1,1};
        double epsx = 0.0000000001;
        int maxits = 0;
        alglib.minlmstate state;
        alglib.minlmreport rep;

        alglib.minlmcreatev(_knownRouter.Length, x, 0.0001, out state);
        alglib.minlmsetcond(state, epsx, maxits);
        alglib.minlmsetscale(state, s);
        
        alglib.minlmoptimize(state, ErrorFunctionLM, null, null);
        alglib.minlmresults(state, out x, out rep);
        
        GeoCoordinate result = new GeoCoordinate(x[0], x[1]);
        return result;
    }

    private void ErrorFunctionLM(double[] x, double[] fi, object obj)
    {
        GeoCoordinate estimatedLocation = new GeoCoordinate(x[0], x[1]);
        for (int i = 0; i < _distances.Length;i++)
        {
            fi[i] = estimatedLocation.GetDistanceTo(_knownRouter[i]) - _distances[i];
        }
    }

    public GeoCoordinate FindOptimalLocationLBFGS()
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
        alglib.minlbfgsoptimize(state, ErrorFunctionLBFGS, null, null);
        alglib.minlbfgsresults(state, out x, out rep);
        GeoCoordinate result = new GeoCoordinate(x[0], x[1]);
        return result;
    }

    private void ErrorFunctionLBFGS(double[] x, ref double function, object obj)
    {
        GeoCoordinate initialGuess = new GeoCoordinate(x[0], x[1]);
        function = MeanSquaredError(initialGuess);
    }
    
    private double MeanSquaredError(GeoCoordinate estimatedLocation)
    {
        int n = _knownRouter.Length;
        double result = 0.0;

        for (int i = 0; i < n; i++)
        {
            double estimatedDistance = estimatedLocation.GetDistanceTo(_knownRouter[i]);
            result += Math.Pow(estimatedDistance - _distances[i], 2);
        }
        return result/n;
    }

    public double Error(GeoCoordinate location)
    {
        int n = _knownRouter.Length;
        double result = 0.0;

        for (int i = 0; i < n; i++)
        {
            double estimatedDistance = location.GetDistanceTo(_knownRouter[i]);
            result += Math.Abs(estimatedDistance - _distances[i]);
        }
        return result/n;
    } 
    
}