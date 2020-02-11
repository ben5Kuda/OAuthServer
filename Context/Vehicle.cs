using System;
using System.Collections.Generic;

namespace IdSrvr4Demo.Context
{
    public partial class Vehicle
    {
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Registration { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
    }
}
