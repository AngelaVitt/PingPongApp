using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PingPongApp.Models;
using PingPongApp.Repository;

namespace PingPongApp.Processors
{
    public class HomeManagerProcessor
    {
        public static async Task<List<PlayerInfo>> ProcessContact()
        {
            return await HomeRepository.GetListOfPlayers();
        }
    }
}
