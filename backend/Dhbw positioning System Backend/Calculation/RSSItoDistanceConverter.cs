using System;
using Dhbw_positioning_System_Backend.Model;

namespace Dhbw_positioning_System_Backend.Calculation;

public class RSSItoDistanceConverter
{
    private const double SignalPropagation24GHz = 3.0; //Usually between 2 and 4.3, has to be tested.  
    private const double SignalPropagation5GHz = 3.0; //Usually between 2 and 4.3, has to be tested.  
    private const double RssiAtOneMeter24GHz = -32; //Measured reference-RSSI at a distance of 1m  
    private const double RssiAtOneMeter5GHz = -32; //Measured reference-RSSI at a distance of 1m  
    public static double Convert(DataPoint ap)
    {
        if(ap.SSID.Equals("DHBW-KA5")){
            return Math.Pow(10, (RssiAtOneMeter5GHz - ap.Level) / (10 * SignalPropagation5GHz));
        } else {
            return Math.Pow(10, (RssiAtOneMeter24GHz - ap.Level) / (10 * SignalPropagation24GHz));
        }
    }
}