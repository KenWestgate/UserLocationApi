using System;
using UserLocation.Api.Documents.Common;

namespace UserLocation.Api.Documents.Get
{
    public class UserCurrentLocation
    {
        public string Id { get; set; }
        public Location CurrentLocation { get; set; }
        public DateTime TimeAtLocation { get; set; }
    }
}
