using System;
using System.Collections.Generic;
namespace PingPongApp.Models
{
    public class PlayerMatchDetail
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int PlayerAge { get; set; }
        public int NumberOfGamesPlayed { get; set; }
        public double PercentWon { get; set; }
        public List<OpponentInfo> OpponentInfo { get; set; }

    }
}
