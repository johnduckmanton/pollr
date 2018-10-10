using Microsoft.AspNetCore.SignalR;
using Pollr.Api.Dal;
using Pollr.Api.Helpers;
using Pollr.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pollr.Api.Hubs
{


    public class VoteHub : Hub
    {
        public const string VOTERS_GROUP = "voters_group";
        public const string WATCHERS_GROUP = "watchers_group";
        private readonly IPollRepository _pollRepository;
        public static HashSet<string> ConnectedIds = new HashSet<string>();

        public VoteHub(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public async Task<VoteResult> Vote(string pollId, int question, int answer)
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

                    // then notify connected 'watcher' clients of the poll status using SignalR
                    string message = PollHelper.GetPollResultsAsJson(updatedPoll);
                    await SendMessageToGroups(new List<string> { WATCHERS_GROUP }, "results", message);

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

        public Task SendMessageToAll(string method, string message)
        {
            return Clients.All.SendAsync(method, message);
        }

        public Task SendMessageToGroups(List<string> groups, string method, string message)
        {
            return Clients.Groups(groups).SendAsync(method, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}