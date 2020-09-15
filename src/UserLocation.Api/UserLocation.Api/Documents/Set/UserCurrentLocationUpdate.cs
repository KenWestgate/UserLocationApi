using UserLocation.Api.Documents.Common;

namespace UserLocation.Api.Documents.Set
{
    public class UserCurrentLocationUpdate
    {
        public string Id { get; set; }
        public Location CurrentLocation { get; set; }
    }
}
