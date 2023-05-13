using System;
using Dhbw_positioning_System_Backend.Model.dto;

namespace Dhbw_positioning_System_Backend.Calculation;

public static class RSSItoDistanceConverter
{
    private const double SignalPropagation24GHz = 3.0; //Usually between 2 and 4.3, has to be tested.  
    private const double SignalPropagation5GHz = 3.0; //Usually between 2 and 4.3, has to be tested.  
    private const double RssiAtOneMeter24GHz = -32; //Measured reference-RSSI at a distance of 1m  
    private const double RssiAtOneMeter5GHz = -24; //Measured reference-RSSI at a distance of 1m  
    
    public static double ConvertWithFormula5G(double rssi)
    {
        return Math.Pow(10, (RssiAtOneMeter5GHz - rssi) / (10 * SignalPropagation5GHz));
    }
    
    public static double ConvertWithFormula2G(double rssi)
    {
        return Math.Pow(10, (RssiAtOneMeter24GHz - rssi) / (10 * SignalPropagation24GHz));
    }
    
    public static double ConvertWithRegression(double rssi)
    {
        return 0.00001 * Math.Pow(rssi * -1, 3.5537);
    }
}