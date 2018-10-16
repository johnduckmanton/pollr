/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pollr.Api.Dal;
using Pollr.Api.Exceptions;
using Pollr.Api.Helpers;
using Pollr.Api.Hubs;
using Pollr.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pollr.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollRepository _pollRepository;
        private IHubContext<VoteHub> _hubContext;

        public PollsController(IPollRepository pollRepository, IHubContext<VoteHub> hubContext)
        {
            _pollRepository = pollRepository;
            _hubContext = hubContext;
        }


        /// <summary>
        /// Get all polls matching the specified status
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(Poll[]))]
        public async Task<ActionResult> GetPolls([FromQuery]string status)
        {

            IEnumerable<Poll> polls;

            if (status != null) {
                polls = await _pollRepository.GetPollsByStatusAsync(status);
            }
            else {
                polls = await _pollRepository.GetAllPollsAsync();
            }

            return Ok(polls);
        }

        /// <summary>
        /// Get a poll
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Poll))]
        public async Task<ActionResult> GetPoll(string id)
        {
            try {
                var poll = await _pollRepository.GetPollAsync(id) ?? new Poll();
                return Ok(poll);
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get a poll using its handle
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [Route("handle/{handle}")]
        [ProducesResponseType(200, Type = typeof(Poll))]
        public async Task<ActionResult> GetPollByHandle([FromRoute]string handle)
        {
            try {
                var poll = await _pollRepository.GetPollByHandleAsync(handle) ?? new Poll();
                return Ok(poll);
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a new poll based on the specified poll definition
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new poll</returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Poll))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody]PollRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                Poll poll = await _pollRepository.CreatePollAsync(request.Name, request.PollDefinitionId, request.IsOpen); ;
                return CreatedAtAction(nameof(GetPoll), poll);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update a poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pollData"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Put(string id, [FromBody]Poll pollData)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                await _pollRepository.UpdatePollAsync(id, pollData);
                return NoContent();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Delete a poll
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(string id)
        {
            try {
                await _pollRepository.RemovePollAsync(id);
                return NoContent();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Open a poll for voting 
        /// </summary>
        /// <param name="id">The id of the poll to open</param>
        /// <returns></returns>
        [HttpPut("{id}/actions/open")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Open(string id)
        {
            try {
                bool result = await _pollRepository.OpenPollAsync(id);
                if (result)
                    return NoContent();
                else
                    return StatusCode(500, "Error occurred updating the database");
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Increments the current question counter for the poll 
        /// </summary>
        /// <param name="id">The id of the poll</param>
        /// <returns></returns>
        [HttpPut("{id}/actions/nextquestion")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> NextQuestion(string id)
        {
            try {
                Poll updatedPoll = await _pollRepository.SetNextQuestionAsync(id);
                if (updatedPoll != null) {

                    await _hubContext.Clients.All.SendAsync("LoadQuestion", new LoadQuestionRequest() { PollId = id, QuestionIndex = updatedPoll.CurrentQuestion -1 });
                    return Ok(updatedPoll);
                }
                else {
                    return StatusCode(500, "Error occurred updating the database");
                }
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (PollAtLastQuestionException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Sets the status to closed. This will prevent any further votes
        /// being taken against the poll
        /// </summary>
        /// <param name="id">The id of the poll to close</param>
        /// <returns></returns>
        [HttpPut("{id}/actions/close")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Close(string id)
        {
            try {
                bool result = await _pollRepository.ClosePollAsync(id);
                if (result)
                    return NoContent();
                else
                    return StatusCode(500, "Error occurred updating the database");
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Vote on a specific question in the poll
        /// </summary>
        /// <param name="id">The id of the poll</param>
        /// <param name="question">the number of the question to vote on</param>
        /// <param name="answer">the number of the answer being voted for</param>
        /// <returns></returns>
        [HttpPut("{id}/actions/vote")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Vote(string id, int question, int answer)
        {
            if (question < 1 || answer < 1) {
                return BadRequest("Index values must start at 1");
            }

            try {

                // Register the vote in the database
                Poll updatedPoll = await _pollRepository.VoteAsync(id, question, answer);
                if (updatedPoll != null) {

                    // then notify connected clients of the poll status using SignalR
                    PollResult message = PollHelper.GetPollResults(updatedPoll);
                    SendToConnectedClients(message);

                    return NoContent();
                }
                else
                    return BadRequest("Poll does not exist or is closed");
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Send a message to all connected SignalR clients
        /// </summary>
        /// <param name="message"></param>
        private void SendToConnectedClients(object message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            _hubContext.Clients.All.SendAsync("message", message);
        }

        /// <summary>
        /// Get the results for a poll
        /// </summary>
        /// <param name="id">The id of the poll</param>
        /// <returns></returns>
        [HttpGet("{id}/results")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("{id}/results")]
        public async Task<ActionResult> GetPollResults(string id)
        {
            var poll = await _pollRepository.GetPollAsync(id) ?? new Poll();
            if (poll == null)
                return NotFound();

            PollResult result = PollHelper.GetPollResults(poll);

            return Ok(result);
        }
    }
}
