using System;
using TechnicalTest.Api.Documents.Common;

namespace TechnicalTest.Api.Documents.Get
{
    public class UserCurrentLocation
    {
        public string Id { get; set; }
        public Location CurrentLocation { get; set; }
        public DateTime TimeAtLocation { get; set; }
    }
}
