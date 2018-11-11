/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.SignalR;
using Pollr.Api.Data;
using Pollr.Api.Helpers;
using Pollr.Api.Models;
using Pollr.Api.Models.Polls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pollr.Api.Hubs
{


    public class VoteHub : Hub
    {
        public const string VOTERS_GROUP = "voters_group";
        public const string WATCHERS_GROUP = "watchers_group";

        public static string LOAD_QUESTION = "LoadQuestion";
        public static string NEW_CONNECTION = "NewConnection";
        public static string VOTE_RECEIVED = "VoteReceived";
        public static string BROADCAST = "Broadcast";

        private readonly IPollRepository _pollRepository;
        public static HashSet<string> connectedIds = new HashSet<string>();

        public VoteHub(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        /// <summary>
        /// Update the database poll record with the selected vote
        /// and notify any connected clients
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public async Task<VoteResult> Vote(int pollId, int question, int answer)
        {
            VoteResult result = new VoteResult();

            if (question < 1 || answer < 1) {
                result.StatusCode = 400;
                result.Errors.Add("Question and Answer index values must start at 1");
                return result;
            }

            try {

                // Register the vote in the database
                Poll updatedPoll = await _pollRepository.VoteAsync(pollId, question, answer);
                if (updatedPoll != null) {

                    // then notify other connected clients of the poll status using SignalR
                    PollResult message = PollHelper.GetPollResults(updatedPoll);
                    await SendMessageToOthers(VOTE_RECEIVED, message);

                    result.StatusCode = 200;
                }
                else {
                    result.StatusCode = 400;
                    result.Errors.Add("Poll does not exist or is closed");
                }
            }
            catch (Exception e) {
                result.StatusCode = 500;
                result.Errors.Add(e.Message);
            }

            return result;

        }

        public int GetConnectionCount ()
        {
            return connectedIds.Count;
        }

        public Task SendMessageToAll(string method, object data)
        {
            return Clients.All.SendAsync(method, data);
        }

        public Task SendMessageToGroups(List<string> groups, string method, object data)
        {
            return Clients.Groups(groups).SendAsync(method, data);
        }

        public Task SendMessageToOthers(string method, object data)
        {
            return Clients.AllExcept(Context.ConnectionId).SendAsync(method, data);
        }


        public override async Task OnConnectedAsync()
        {
            // Add the connection to our HashSet and broadcast the current count
            // HACK: This doesn't scale across multiple servers without adding something like Redis
            // but is just for demo purposes
            connectedIds.Add(Context.ConnectionId);
            await SendMessageToAll(NEW_CONNECTION, connectedIds.Count);
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            connectedIds.Remove(Context.ConnectionId);
            await SendMessageToAll("ConnectionCount", connectedIds.Count);
            await base.OnDisconnectedAsync(exception);
        }

        public Task BroadcastMessage(string type, string payload)
        {
            throw new NotImplementedException();
        }
    }
}