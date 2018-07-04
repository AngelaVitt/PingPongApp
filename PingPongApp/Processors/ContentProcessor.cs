using System;
using PingPongApp.Repository;

namespace PingPongApp.Processors
{
    public class ContentProcessor
    {
        public static bool ProcessContact()
        {
            return HomeRepository.AddContactToDB();
        }
    }
}
