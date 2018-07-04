using System;
namespace PingPongApp.Models
{
    public class OpponentInfo
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public DateTime MostRecentMatchDate { get; set; }
        public double PercentWon { get; set; }
        public string WinnerName { get; set; }
    }
}
