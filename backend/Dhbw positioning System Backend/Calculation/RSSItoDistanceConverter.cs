using System;
using Dhbw_positioning_System_Backend.Model.dto;

namespace Dhbw_positioning_System_Backend.Calculation;

public static class RSSItoDistanceConverter
{
    private const double SignalPropagation24GHz = 3.31; //Usually between 2 and 4.3, has to be tested.  
    private const double SignalPropagation5GHz = 4.33; //Usually between 2 and 4.3, has to be tested.  
    private const double RssiAtOneMeter24GHz = -20; //Measured reference-RSSI at a distance of 1m  
    private const double RssiAtOneMeter5GHz = -20; //Measured reference-RSSI at a distance of 1m  

    private const double OptimizedSignalPropagation24GHz = 4.41; //manually optimized  
    private const double OptimizedSignalPropagation5GHz = 4.17; //manually optimized   
    private const double OptimizedRssiAtOneMeter24GHz = -0.3; //manually optimized  
    private const double OptimizedRssiAtOneMeter5GHz = -22; //manually optimized  

    public static double ConvertWithFormula5G(double rssi)
    {
        return Math.Pow(10, (RssiAtOneMeter5GHz - rssi) / (10 * SignalPropagation5GHz));
    }
    
    public static double ConvertWithFormula2G(double rssi)
    {
        return Math.Pow(10, (RssiAtOneMeter24GHz - rssi) / (10 * SignalPropagation24GHz));
    }

    public static double ConvertWithOptimizedFormula5G(double rssi)
    {
        return Math.Pow(10, (OptimizedRssiAtOneMeter5GHz - rssi) / (10 * OptimizedSignalPropagation5GHz));
    }

    public static double ConvertWithOptimizedFormula2G(double rssi)
    {
        return Math.Pow(10, (OptimizedRssiAtOneMeter24GHz - rssi) / (10 * OptimizedSignalPropagation24GHz));
    }

    public static double ConvertWithRegression(double rssi)
    {
        return 0.00001 * Math.Pow(rssi * -1, 3.5537);
    }
}