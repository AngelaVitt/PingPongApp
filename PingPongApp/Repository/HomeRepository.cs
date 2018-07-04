using System.Configuration;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using PingPongApp.Models;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace PingPongApp.Repository
{
    public class HomeRepository
    {
        public static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DevResultsConnection"].ConnectionString;

        public static async Task<List<PlayerInfo>> GetListOfPlayers()
        {
            var playerList = new List<PlayerInfo>();

            var query = "SELECT * FROM dbo.players";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                using (var reader = command.ExecuteReader())
                {
                    var playerIdCol = reader.GetOrdinal("Id");
                    var playerNameCol = reader.GetOrdinal("Name");
                    var playerAgeCol = reader.GetOrdinal("Age");
                    while (reader.Read())
                    {
                        var player = new PlayerInfo()
                        {
                            PlayerId = reader.GetInt32(playerIdCol),
                            PlayerName = reader.GetString(playerNameCol),
                            PlayerAge = reader.GetInt32(playerAgeCol)
                        };
                        playerList.Add(player);
                    }
                }
                command.Dispose();
                connection.Close();


            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return playerList;
        }
        public static async Task<List<MatchInfo>> GetListOfMatches()
        {
            var matchList = new List<MatchInfo>();

            var query = "SELECT matches.*, player1.name AS playerOneName, player2.name AS playerTwoName, winningPlayer.name AS winningPlayerName FROM dbo.matches " +
                "JOIN players AS player1 ON playerOneId = player1.Id " +
                "JOIN players AS player2 ON playerTwoId = player2.Id " +
                "JOIN players AS winningPlayer ON WinningPlayerId = winningPlayer.Id";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                using (var reader = command.ExecuteReader())
                {
                    var IdCol = reader.GetOrdinal("Id");
                    var playerOneIdCol = reader.GetOrdinal("PlayerOneId");
                    var playerTwoIdCol = reader.GetOrdinal("PlayerTwoId");
                    var playerOneScoreCol = reader.GetOrdinal("PlayerOneScore");
                    var playerTwoScoreCol = reader.GetOrdinal("PlayerTwoScore");
                    var playerWinnerIdCol = reader.GetOrdinal("WinningPlayerId");
                    var playerWinnerNameCol = reader.GetOrdinal("winningPlayerName");
                    var playerOneNameCol = reader.GetOrdinal("playerOneName");
                    var playerTwoNameCol = reader.GetOrdinal("playerTwoName");
                    while (reader.Read())
                    {
                        var playerMatchInfo = new List<PlayerMatchInfo>();
                        var playerOne = new PlayerMatchInfo()
                        {
                            PlayerId = reader.GetInt32(playerOneIdCol),
                            PlayerScore = reader.GetInt32(playerOneScoreCol),
                            PlayerName = reader.GetString(playerOneNameCol)
                                                
                        };
                        playerMatchInfo.Add(playerOne);
                        var playerTwo = new PlayerMatchInfo()
                        {
                            PlayerId = reader.GetInt32(playerTwoIdCol),
                            PlayerScore = reader.GetInt32(playerTwoScoreCol),
                            PlayerName = reader.GetString(playerTwoNameCol)

                        };
                        playerMatchInfo.Add(playerTwo);
                        var match = new MatchInfo()
                        {
                            MatchId = reader.GetInt32(IdCol),
                            Winner = reader.GetString(playerWinnerNameCol),
                            Players = playerMatchInfo
                        };
                        matchList.Add(match);
                    }
                }
                command.Dispose();
                connection.Close();


            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return matchList;
        }
        public static async Task<bool> SetMatch(List<PlayerMatchInfo> playerMatchInfo)
        {
            var higherScore = 0;
            foreach (var element in playerMatchInfo)
            {
                if (higherScore < element.PlayerScore)
                    higherScore = element.PlayerScore;
            }
            var winnerId = playerMatchInfo.Where(x => x.PlayerScore == higherScore).FirstOrDefault().PlayerId;
            var query = "INSERT INTO matches " +
                "VALUES(@PlayerOneId, @PlayerTwoId,@PlayerOneScore,@PlayerTwoScore,@WinningPlayerId,@Date";
            query = query.Replace("@PlayerOneId", playerMatchInfo[0].PlayerId.ToString())
                         .Replace("@PlayerTwoId", playerMatchInfo[1].PlayerId.ToString())
                         .Replace("@PlayerOneScore", playerMatchInfo[0].PlayerScore.ToString())
                         .Replace("@PlayerTwoScore", playerMatchInfo[1].PlayerScore.ToString())
                         .Replace("@WinningPlayerId", winnerId.ToString())
                         .Replace("@Date", DateTime.UtcNow.ToString());
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                var reader = command.ExecuteNonQuery();


                command.Dispose();
                connection.Close();


            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return true;
        }
        public static async Task<PlayerMatchDetail> GetPlayerMatchesInfo(int playerId)
        {

            var matchDetails = new PlayerMatchDetail();
            var listOfPlayers = new HashSet<int>();
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var numberOfMatchesPlayedQuery = "SELECT * FROM dbo.matches " +
                    "JOIN players on " + playerId + " = players.Id " +
                    "WHERE PlayerOneId = " + playerId + " or PlayerTwoId = " + playerId +
                    "ORDER BY MatchDate Desc";
                SqlCommand command = new SqlCommand(numberOfMatchesPlayedQuery, connection);
                using (var reader = command.ExecuteReader())
                {
                    var playerOneIdCol = reader.GetOrdinal("PlayerOneId");
                    var playerTwoIdCol = reader.GetOrdinal("PlayerTwoId");
                    var playerOneScoreCol = reader.GetOrdinal("PlayerOneScore");
                    var playerTwoScoreCol = reader.GetOrdinal("PlayerTwoScore");
                    var playerWinnerIdCol = reader.GetOrdinal("WinningPlayerId");
                    var playerNameCol = reader.GetOrdinal("Name");
                    var playerAgeCol = reader.GetOrdinal("Age");
                    var matchesWon = 0;
                    var matchesPlayed = 0;
                    while (reader.Read())
                    {
                        var PlayerOneId = reader.GetInt32(playerOneIdCol);
                        var PlayerTwoId = reader.GetInt32(playerTwoIdCol);
                        var winnerId = reader.GetInt32(playerWinnerIdCol);
                        var playerAge = reader.GetInt32(playerAgeCol);
                        var playerName = reader.GetString(playerNameCol);

                        matchDetails.PlayerId = playerId;
                        matchDetails.PlayerName = playerName;
                        matchDetails.PlayerAge = playerAge;


                        matchesPlayed += 1;

                        if (winnerId == playerId)
                            matchesWon += 1;

                        var opponentId = 0;
                        if (PlayerOneId != playerId)
                            opponentId = PlayerOneId;
                        else
                            opponentId = PlayerTwoId;

                        listOfPlayers.Add(opponentId); //Make list unique 

                    }
                    matchDetails.NumberOfGamesPlayed = matchesPlayed;
                    if (matchesPlayed == 0 || matchesWon == matchesPlayed)
                        matchDetails.PercentWon = 100;
                    else
                        matchDetails.PercentWon = (float)matchesWon / matchesPlayed * 100;


                }
                command.Dispose();
                connection.Close();
                connection.Open();
                var opponentInfoList = new List<OpponentInfo>();
                matchDetails.OpponentInfo = opponentInfoList;
                foreach (var Id in listOfPlayers)
                {
                    var opponentQuery = "SELECT * FROM dbo.matches " +
                        "JOIN players on " + Id + " = players.Id " +
                        "WHERE(PlayerOneId = " + playerId + " and PlayerTwoId = " + Id + ") or" +
                        "(PlayerOneId = " + Id + " and PlayerTwoId = " + playerId + ")" +
                        "ORDER BY MatchDate Desc";
                    var secondCommand = new SqlCommand(opponentQuery, connection);
                    using (var reader = secondCommand.ExecuteReader())
                    {
                        var playerOneIdCol = reader.GetOrdinal("PlayerOneId");
                        var playerTwoIdCol = reader.GetOrdinal("PlayerTwoId");
                        var playerOneScoreCol = reader.GetOrdinal("PlayerOneScore");
                        var playerTwoScoreCol = reader.GetOrdinal("PlayerTwoScore");
                        var playerWinnerIdCol = reader.GetOrdinal("WinningPlayerId");
                        var playerNameCol = reader.GetOrdinal("Name");
                        var matchDateCol = reader.GetOrdinal("MatchDate");

                        var matchesPlayed = 0;
                        var matchesLost = 0;
                        var opponent = new OpponentInfo();
                        var first = true;

                        while (reader.Read())
                        {
                            var PlayerOneId = reader.GetInt32(playerOneIdCol);
                            var PlayerTwoId = reader.GetInt32(playerTwoIdCol);
                            var winnerId = reader.GetInt32(playerWinnerIdCol);
                            var playerName = reader.GetString(playerNameCol);
                            var matchDate = reader.GetDateTime(matchDateCol);

                            opponent.PlayerId = Id;
                            opponent.PlayerName = playerName;
                            if (first)
                            {
                                opponent.MostRecentMatchDate = matchDate;
                                if (winnerId != Id)
                                    opponent.WinnerName = matchDetails.PlayerName;
                                else
                                    opponent.WinnerName = playerName;

                                first = false;
                            }

                            matchesPlayed += 1;

                            if (winnerId != Id)
                                matchesLost += 1;

                        }
                        if (matchesPlayed == 0 || matchesLost == matchesPlayed)
                            opponent.PercentWon = 100;
                        else
                            opponent.PercentWon = (float)matchesLost / matchesPlayed * 100;
                        opponentInfoList.Add(opponent);
                    }
                    matchDetails.OpponentInfo = opponentInfoList;
                    secondCommand.Dispose();
                }

                connection.Close();


            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return matchDetails;
        }
    }
}