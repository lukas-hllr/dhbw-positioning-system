using System;

namespace Dhbw_positioning_System_Backend;

public class RSStoDistanceConverter
{
    private const double SignalPropagation = 2.0;//Usually between 2 and 4.3, hast to be tested.  
    public static double Convert(double receivedSignalStrength,double referenceValue)
    {
        return Math.Pow(10, (receivedSignalStrength + referenceValue) / -(10 * SignalPropagation));
    }
}