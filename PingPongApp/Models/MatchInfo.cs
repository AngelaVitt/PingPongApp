using System;
using System.Collections.Generic;

namespace PingPongApp.Models
{
    public class MatchInfo
    {
        public int MatchId { get; set; }
        public List<PlayerMatchInfo> Players { get; set; }
        public string Winner { get; set; }


    }
}
