namespace VirtualClinic.Entities
{
    public class GeoLocation : BaseEntity
    {
        public GeoLocation(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }

        public double lat { get; set; }
        public double lng { get; set; }
    }
}