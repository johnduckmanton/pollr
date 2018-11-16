/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
//#r "Newtonsoft.Json"
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pollr.VoteFunc
{
    public static class GetVoteFromDevice
    {
        // The URL to the Pollr Web Api. For example: "http://jrd-pollr-api.azurewebsites.net/api";
        private static readonly string apiUrl = Environment.GetEnvironmentVariable("PollrApiUrl");

        // The Id of the Poll that wee want to register votes against
        private static readonly string pollId = Environment.GetEnvironmentVariable("TargetPollId");

        private static readonly HttpClient httpClient = new HttpClient();


        // This function is designed to receive an IoT Hub message from
        // an M5Stack device. The device has three buttons, and will send a 
        // message when a button is pressed. The idea is to register a vote
        // by detecting receiving the button press message and registering a vote 
        // for the corresponding answer in the poll.
        //
        // There are currently some fudges:
        // - There is no good way to identify which poll is currently active
        //   To address this for now, we set the TARGET_POLL_ID app setting with the database id of the 
        //   poll we want to vote on.
        // 
        // - There is also no way to detect which question is currently active
        //   We handle this by calling the api method that allows us to vote on the currently
        //   set question for the specified poll.
        //
        // - Currently it doesn't prevent multiple votes from the same device
        //   Not addressed yet, as this is only a demo app.

        [FunctionName("get-vote-from-device")]
        public static async Task Run([EventHubTrigger("samples-workitems", Connection = "IotHubConnection")]string myEventHubMessage, TraceWriter log)
        {

            // Check that the environment variabels are properly defined
            if (apiUrl == null)
            {
                log.Error($"### 'PollrApiUrl' environment variable is not set. Cannot continue.");
                return;
            }

            log.Info($"### API URL is '{apiUrl}'");


            if (pollId == null)
            {
                log.Error($"### 'TargetPollId' environment variable is not set. Cannot continue.");
                return;
            }

            log.Info($"### Target poll id is '{pollId}'");

            log.Info($"### Event Hub trigger function processed a message: {myEventHubMessage}");


            // Parse the Json received from the device and
            // extract the button that was pressed
            if (!string.IsNullOrEmpty(myEventHubMessage))
            {
                try
                {
                    JObject obj = JObject.Parse(myEventHubMessage);
                    string btn = (string)obj["btn"];
                    if (btn == null || btn.Length == 0)
                    {
                        log.Error($"### Couldn't parse button from: {myEventHubMessage}");
                        return;
                    }
                    log.Info($"Button {btn} was pressed.");

                    var answerIdx = btn;

                    var voteUrl = apiUrl + $"/polls/{pollId}/actions/vote/current?answer={answerIdx}";
                    log.Info($"Posting vote to: {voteUrl}");

                    HttpResponseMessage response = await httpClient.PutAsync(voteUrl, null);
                    response.EnsureSuccessStatusCode();
                    log.Info($"Response: {response.StatusCode}");
                }
                catch (JsonReaderException)
                {
                    log.Error($"Message '{myEventHubMessage}' is not valid JSON.");
                }
            }
        }
    }
}
