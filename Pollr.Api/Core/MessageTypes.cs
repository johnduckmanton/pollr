using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Core
{
    public class MessageTypes
    {
        public static string LOAD_QUESTION = "LoadQuestion";
        public static string RESET_POLL = "ResetPoll";
        public static string NEW_CONNECTION = "NewConnection";
        public static string VOTE_RECEIVED = "VoteReceived";
        public static string BROADCAST = "Broadcast";
    }
}
