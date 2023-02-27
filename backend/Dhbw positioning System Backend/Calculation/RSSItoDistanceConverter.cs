using System;

namespace Dhbw_positioning_System_Backend.Calculation;

public class RSSItoDistanceConverter
{
    private const double SignalPropagation = 3.0; //Usually between 2 and 4.3, has to be tested.  
    private const double RssiAtOneMeter = -32; //Measured reference-RSSI at a distance of 1m  
    public static double Convert(double measuredRssi)
    {
        return Math.Pow(10, (RssiAtOneMeter - measuredRssi) / (10 * SignalPropagation));
    }
}