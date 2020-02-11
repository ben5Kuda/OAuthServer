using System;
using System.Collections.Generic;

namespace IdSrvr4Demo.Context
{
    public partial class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public int LeagueId { get; set; }
        public int CountryId { get; set; }
    }
}
