namespace MrRondon.Services.Api.ViewModels
{
    public class LocationVm
    {
        public LocationVm(string id, string label, string address, PinType pinType, LocationType locationType, Position position, byte[]image=null)
        {
            Id = id;
            Label = label;
            Address = address;
            PinType = pinType;
            Position = position;
            LocationType = locationType;
            Image = image;
        }

        public string Id { get; }
        public string Label { get; }
        public string Address { get; }
        public byte[] Image { get; }
        public PinType PinType { get; }
        public LocationType LocationType { get; }
        public Position Position { get; }
    }

    public class Position
    {
        public Position(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }
    }
}