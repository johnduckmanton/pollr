/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pollr.Api.Core;
using Pollr.Api.Data;
using Pollr.Api.Exceptions;
using Pollr.Api.Helpers;
using Pollr.Api.Hubs;
using Pollr.Api.Models;
using Pollr.Api.Models.Polls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pollr.api.Controllers
{
    /// <summary>
    /// Controller for Polls
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollRepository _pollRepository;
        private readonly ILogger<PollsController> _logger;
        private IHubContext<VoteHub> _hubContext;

        public PollsController(IPollRepository pollRepository, IHubContext<VoteHub> hubContext, ILogger<PollsController> logger)
        {
            _pollRepository = pollRepository;
            _logger = logger;
            _hubContext = hubContext;
        }


        /// <summary>
        /// Get all polls matching the specified status
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(Poll[]))]
        public async Task<ActionResult> GetPolls([FromQuery]PollStatus status = PollStatus.Undefined)
        {
            IEnumerable<Poll> polls;

            if (status != PollStatus.Undefined) {
                _logger.LogInformation(LoggingEvents.GetPolls, "Listing all polls with status ({status})", status.ToString());
                polls = await _pollRepository.GetPollsByStatusAsync(status);
            }
            else {
                _logger.LogInformation(LoggingEvents.GetPolls, "Listing all polls");
                polls = await _pollRepository.GetAllPollsAsync();
            }

            return Ok(polls);
        }

        /// <summary>
        /// Get a poll
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPoll")]
        [ProducesResponseType(200, Type = typeof(Poll))]
        public async Task<ActionResult> Get(int id)
        {
            _logger.LogInformation(LoggingEvents.GetPoll, "Getting poll {id}", id);

            try {
                var poll = await _pollRepository.GetPollAsync(id);
                return Ok(poll);
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.GetPoll, "Error getting poll {id}: Exception {ex}", id, e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get a poll using its handle
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [HttpGet("handle/{handle}", Name = "GetPollByHandle")]
        [ProducesResponseType(200, Type = typeof(Poll))]
        public async Task<ActionResult> GetPollByHandle([FromRoute]string handle)
        {
            _logger.LogInformation(LoggingEvents.GetPoll, "Getting poll with handle {handle}", handle);

            try {
                var poll = await _pollRepository.GetPollByHandleAsync(handle);
                return Ok(poll);
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.GetPollByHandle, "Error getting poll {id}: Exception {ex}", handle, e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a new poll based on the specified poll definition
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new poll</returns>
        [HttpPost()]
        [ProducesResponseType(201, Type = typeof(Poll))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody]PollRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                Poll poll = await _pollRepository.CreatePollAsync(request.Name, request.PollDefinitionId, request.Handle, request.Description, request.IsOpen);
                _logger.LogInformation(LoggingEvents.InsertPoll, "Poll {Id} Created", poll.Id);
                return CreatedAtRoute("GetPoll", new { controller = "Polls", id = poll.Id }, poll);
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.InsertPoll, $"Error creating new poll: Exception {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update a poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="poll"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Put(int id, [FromBody]Poll poll)
        {
            if (poll == null || poll.Id != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var updatedPoll = await _pollRepository.UpdatePollAsync(poll);
                _logger.LogInformation(LoggingEvents.UpdatePoll, "Poll {id} Updated", poll.Id);
                return Ok(updatedPoll);
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.UpdatePoll, "Error updating poll {id}: Exception {ex}", id, e.Message);
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Delete a poll
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(int id)
        {
            try {
                await _pollRepository.RemovePollAsync(id);
                _logger.LogInformation(LoggingEvents.DeletePoll, "Poll {id} Deleted", id);
                return new NoContentResult();
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.DeletePoll, "Error deleting poll {id}: Exception {ex}", id, e.Message);
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
        public async Task<ActionResult> Open(int id)
        {
            try {
                bool result = await _pollRepository.OpenPollAsync(id);
                if (result) {
                    _logger.LogInformation(LoggingEvents.OpenPoll, "Poll {id} status set to open", id);
                    return new NoContentResult();
                }
                else
                    return StatusCode(500, "Error occurred updating the database");
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Error occurred whilst attempting to update poll {id}: {e.Message}");
                throw (e);
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.OpenPoll, "Error updating poll status to open {id}: Exception {ex}", id, e.Message);
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
        public async Task<ActionResult> NextQuestion(int id)
        {
            try {
                Poll updatedPoll = await _pollRepository.SetNextQuestionAsync(id);
                if (updatedPoll != null) {
                    PollResult message = PollHelper.GetPollResults(updatedPoll);

                    await _hubContext.Clients.All.SendAsync(MessageTypes.LOAD_QUESTION, message);

                    _logger.LogInformation(LoggingEvents.PollSetNextQuestion, "Poll {id} next question updated", id);
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
                _logger.LogError(LoggingEvents.PollSetNextQuestion, "Error setting next question for poll {id}: Exception {ex}", id, e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Reset the vote counts for the poll 
        /// </summary>
        /// <param name="id">The id of the poll</param>
        /// <returns></returns>
        [HttpPut("{id}/actions/reset")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> ResetPoll(int id)
        {
            try
            {
                Poll poll = await _pollRepository.GetPollAsync(id);
                if (poll != null)
                {

                    // Go through each question and reset the vote counts
                    foreach (Question question in poll.Questions)
                    {
                        foreach (Answer answer in question.Answers)
                        {
                            answer.VoteCount = 0;
                        }
                    }

                    poll.CurrentQuestion = 1;

                    // Update the poll then notify connected clients of the poll status using SignalR
                    poll = await _pollRepository.UpdatePollAsync(poll);
                    PollResult message = PollHelper.GetPollResults(poll);

                    await _hubContext.Clients.All.SendAsync(MessageTypes.RESET_POLL, message);

                    _logger.LogInformation(LoggingEvents.ResetPoll, "Poll {id} has been reset", id);
                    return Ok(poll);
                }
                else
                {
                    return StatusCode(500, "Error occurred updating the database");
                }
            }
            catch (PollNotFoundException e)
            {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.PollSetNextQuestion, "Error resetting poll {id}: Exception {ex}", id, e.Message);
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
        public async Task<ActionResult> Close(int id)
        {
            try {
                bool result = await _pollRepository.ClosePollAsync(id);
                if (result) {
                    _logger.LogInformation(LoggingEvents.ClosePoll, "Poll {id} status set to closed", id);
                    return new NoContentResult();
                }
                else
                    return StatusCode(500, "Error occurred updating the database");
            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.ClosePoll, "Error updating poll status to closed {id}: Exception {ex}", id, e.Message);
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
        public async Task<ActionResult> Vote(int id, int question, int answer)
        {
            if (question < 1 || answer < 1) {
                return BadRequest("Index values must start at 1");
            }

            try {
                    await RegisterVote(id, question, answer);
                    return new NoContentResult();

            }
            catch (PollNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (PollClosedException)
            {
                return BadRequest("Poll is not open for voting");
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.OpenPoll, "Error updating poll status to open {id}: Exception {ex}", id, e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Vote on the current question in the poll
        /// </summary>
        /// <param name="id">The id of the poll</param>
        /// <param name="answer">the number of the answer being voted for</param>
        /// <returns></returns>
        [HttpPut("{id}/actions/vote/current")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> VoteOnCurrent(int id, int answer)
        {
            if (answer < 1)
            {
                return BadRequest("Answer values must start at 1");
            }

            try
            {
                // Get the poll to determine the current question
                var poll = await _pollRepository.GetPollAsync(id);
                if (poll.Status != PollStatus.Open || poll.CurrentQuestion == 0)
                    return BadRequest("Poll is not open for voting");

                await RegisterVote(id, poll.CurrentQuestion, answer);
                return new NoContentResult();

            }
            catch (PollClosedException)
            {
                return BadRequest("Poll is not open for voting");
            }
            catch (PollNotFoundException e)
            {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.OpenPoll, "Error updating poll status to open {id}: Exception {ex}", id, e.Message);
                return StatusCode(500, e.Message);
            }
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
        public async Task<ActionResult> GetPollResults(int id)
        {
            var poll = await _pollRepository.GetPollAsync(id) ?? new Poll();
            if (poll == null)
                return NotFound();

            PollResult result = PollHelper.GetPollResults(poll);
            _logger.LogInformation(LoggingEvents.GetPollResults, "Get poll results for {id}", id);

            return Ok(result);
        }

        /// <summary>
        /// Register the vote by updating the database
        /// and sending a message to all clients connected
        /// to the hub to inform interested parties
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        private async Task RegisterVote(int pollId, int question, int answer)
        {
            // Register the vote in the database
            Poll updatedPoll = await _pollRepository.VoteAsync(pollId, question, answer);
            if (updatedPoll != null)
            {

                // then notify connected clients of the poll status using SignalR
                PollResult message = PollHelper.GetPollResults(updatedPoll);
                await _hubContext.Clients.All.SendAsync(MessageTypes.VOTE_RECEIVED, message);
                _logger.LogInformation(LoggingEvents.PollVoteRegistered, "Poll {id} vote registered", pollId);
            }
        }
    }
}
