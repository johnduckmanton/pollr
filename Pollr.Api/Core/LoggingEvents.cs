/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Pollr.Api.Core
{
    public class LoggingEvents
    {
        // Poll Events
        public const int GetPolls = 1000;
        public const int GetPoll = 1001;
        public const int GetPollByHandle = 1002;
        public const int InsertPoll = 1003;
        public const int UpdatePoll = 1004;
        public const int DeletePoll = 1005;
        public const int OpenPoll = 1006;
        public const int ClosePoll = 1007;
        public const int PollSetNextQuestion = 1008;
        public const int PollVoteRegistered = 1009;
        public const int GetPollResults = 1010;
        public const int ResetPoll = 1011;

        // Poll Definition Events
        public const int GetPollDefinitions = 2000;
        public const int GetPollDefinition = 2001;
        public const int InsertPollDefinition = 2002;
        public const int UpdatePollDefinition = 2003;
        public const int DeletePollDefinition = 2004;
        public const int PublishPollDefinition = 2005;

        // Error Events
        public const int GetPollNotFound = 9000;
        public const int UpdatePollNotFound = 9001;
        public const int GetPollDefinitionNotFound = 9010;
        public const int UpdatePollDefinitionNotFound = 9011;

        // Status Events
        public const int GetStatus = 10000;

    }
}
