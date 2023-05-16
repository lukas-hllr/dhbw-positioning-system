namespace Dhbw_positioning_System_Backend.Model.dto
{
    public class MeasurementEntityDto
    {
        public string Ssid { get; set; }
        public string Mac { get; set; }
        public int Rssi { get; set; }

        public MeasurementEntityDto(){}
        public MeasurementEntityDto(MeasurementEntity me){
            this.Ssid = me.Ssid;
            this.Mac = me.Mac;
            this.Rssi = (int) me.Rssi;
        }
        public MeasurementEntity toMeasurementEntity(){
            var me = new MeasurementEntity(){
                Ssid = this.Ssid,
                Mac = this.Mac,
                Rssi = this.Rssi
            };
            return me;
        }
    }
}