/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pollr.Api.Dal;
using Pollr.Api.Models;

namespace pollr.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollRepository _pollRepository;

        public PollsController(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        /// <summary>
        /// Get all polls
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Poll[]))]
        public async Task<ActionResult> Get()
        {

            var polls = await _pollRepository.GetAllPolls();
            return Ok(polls);
        }

        /// <summary>
        /// Get a poll
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Poll))]
        public async Task<ActionResult> Get(string id)
        {
            //return GetPollByIdInternal(id);
            var poll = await _pollRepository.GetPoll(id) ?? new Poll();

            return Ok(poll);
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

            Poll poll = await _pollRepository.CreatePoll(request.Name, request.PollDefinitionId, request.IsOpen); ;
            return CreatedAtAction(nameof(Get), poll);
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
                await _pollRepository.UpdatePoll(id, pollData);
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
                await _pollRepository.RemovePoll(id);
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
                bool result = await _pollRepository.OpenPoll(id);
                if (result)
                    return NoContent();
                else
                    return BadRequest("Poll does not exist or is already open");
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
                bool result = await _pollRepository.NextQuestion(id);
                if (result)
                    return Ok();
                else
                    return BadRequest("Poll does not exist or is closed");
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
                bool result = await _pollRepository.ClosePoll(id);
                if (result)
                    return NoContent();
                else
                    return BadRequest("Poll does not exist or is already closed");
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
                bool result = await _pollRepository.Vote(id, question, answer);
                if (result)
                    return NoContent();
                else
                    return BadRequest("Poll does not exist or is closed");
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
