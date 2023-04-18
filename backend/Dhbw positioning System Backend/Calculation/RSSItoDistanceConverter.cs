using System;
using Dhbw_positioning_System_Backend.Model.dto;

namespace Dhbw_positioning_System_Backend.Calculation;

public class RSSItoDistanceConverter
{
    private const double SignalPropagation24GHz = 3.0; //Usually between 2 and 4.3, has to be tested.  
    private const double SignalPropagation5GHz = 3.0; //Usually between 2 and 4.3, has to be tested.  
    private const double RssiAtOneMeter24GHz = -32; //Measured reference-RSSI at a distance of 1m  
    private const double RssiAtOneMeter5GHz = -24; //Measured reference-RSSI at a distance of 1m  
    public static double Convert(MeasurementEntityDto ap)
    {
        //if(ap.Ssid.Equals("DHBW-KA5")){
        //    return Math.Pow(10, (RssiAtOneMeter5GHz - ap.Rssi) / (10 * SignalPropagation5GHz));
        //} else {
        //    return Math.Pow(10, (RssiAtOneMeter24GHz - ap.Rssi) / (10 * SignalPropagation24GHz));
        //}
        return 0.00001 * Math.Pow(ap.Rssi * -1, 3.5537);
    }
}