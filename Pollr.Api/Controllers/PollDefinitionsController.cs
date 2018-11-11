/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pollr.Api.Core;
using Pollr.Api.Data;
using Pollr.Api.Exceptions;
using Pollr.Api.Helpers;
using Pollr.Api.Models;
using Pollr.Api.Models.PollDefinitions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pollr.api.Controllers
{
    /// <summary>
    /// Controller for Poll Definitions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PollDefinitionsController : ControllerBase
    {
        private readonly IPollDefinitionRepository _pollDefinitionRepository;
        private readonly ILogger<PollDefinitionsController> _logger;

        public PollDefinitionsController(IPollDefinitionRepository pollDefinitionRepository, ILogger<PollDefinitionsController> logger)
        {
            _pollDefinitionRepository = pollDefinitionRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of poll definitions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PollDefinition[]))]
        public async Task<ActionResult> Get(bool publishedOnly = false)
        {
            _logger.LogInformation(LoggingEvents.GetPollDefinitions, "Listing all poll definitions (published only: {publishedOnly}", publishedOnly.ToString());

            IEnumerable<PollDefinition> pollDefinitions = await _pollDefinitionRepository.GetPollDefinitionsAsync(publishedOnly);

            return Ok(pollDefinitions);
        }

        /// <summary>
        /// Get a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPoll")]
        [ProducesResponseType(200, Type = typeof(PollDefinition))]
        public async Task<ActionResult> Get(int id)
        {
            _logger.LogInformation(LoggingEvents.GetPollDefinition, $"Getting poll definition {id}");

            try {
                var pollDefinition = await _pollDefinitionRepository.GetPollDefinitionAsync(id);
                return Ok(pollDefinition);
            }
            catch (PollDefNotFoundException e) {
                ApiStatusMessage a = ApiStatusMessage.CreateFromException(e);
                return BadRequest(a);
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.GetPollDefinition, $"Error getting poll definition {id}: Exception {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Create a poll definition
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PollDefinition))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody]PollDefinition pollDefinition)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try
            {
                await _pollDefinitionRepository.AddPollDefinitionAsync(pollDefinition); ;

                _logger.LogInformation(LoggingEvents.InsertPollDefinition, $"Poll Definition {pollDefinition.Id} Created");
                return CreatedAtRoute("GetPoll", new { id = pollDefinition.Id }, pollDefinition);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.InsertPollDefinition, $"Error creating new poll definition: Exception {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Put(int id, [FromBody]PollDefinition pollDefinition)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (pollDefinition.Id != id)
                return BadRequest("Body(id) does not match this poll id ");

            try {
                await _pollDefinitionRepository.UpdatePollDefinitionAsync(pollDefinition);
                _logger.LogInformation(LoggingEvents.UpdatePollDefinition, $"Poll Definition {id} Updated", pollDefinition.Id);

                return new NoContentResult();
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.UpdatePollDefinition, $"Error updating poll definition {id}: Exception {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Delete a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Delete(int id)
        {
            try {
                await _pollDefinitionRepository.RemovePollDefinitionAsync(id);
                _logger.LogInformation(LoggingEvents.DeletePollDefinition, $"Poll Definition {id} Deleted");

                return NoContent();
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.DeletePollDefinition, $"Error deleting poll definition {id}: Exception {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Publish a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPut("{id}/actions/publish")]
        public async Task<ActionResult> Publish(int id, bool isPublished)
        {
            try {
                bool result = await _pollDefinitionRepository.SetPublishedStatusAsync(id, isPublished);
                if (result) {
                    _logger.LogInformation(LoggingEvents.PublishPollDefinition, $"Poll Definition {id} Publish status updated to {isPublished}");
                    return Ok();
                }
                else
                    return BadRequest("Poll definition does not exist or is already published");
            }
            catch (Exception e) {
                _logger.LogError(LoggingEvents.PublishPollDefinition, $"Error publishing poll definition {id}: Exception {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

    }
}
