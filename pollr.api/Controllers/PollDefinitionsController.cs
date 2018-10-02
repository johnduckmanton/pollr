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
    public class PollDefinitionsController : ControllerBase
    {
        private readonly IPollDefinitionRepository _pollDefinitionRepository;

        public PollDefinitionsController(IPollDefinitionRepository pollDefinitionRepository)
        {
            _pollDefinitionRepository = pollDefinitionRepository;
        }

        /// <summary>
        /// Get a list of poll definitions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PollDefinition[]))]
        public async Task<ActionResult> Get(bool publishedOnly = false)
        {

            var pollDefinitions = await _pollDefinitionRepository.GetPollDefinitions(publishedOnly);
            return Ok(pollDefinitions);
        }

        /// <summary>
        /// Get a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PollDefinition[]))]
        public async Task<ActionResult> Get(string id)
        {
            //return GetPollDefinitionByIdInternal(id);
            var pollDefinition = await _pollDefinitionRepository.GetPollDefinition(id);
            if (pollDefinition == null)
                return NotFound();
            else
                return Ok(pollDefinition);
        }

        /// <summary>
        /// Create a poll definition
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PollDefinition))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody]PollDefinition value)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _pollDefinitionRepository.AddPollDefinition(value); ;
            return CreatedAtAction(nameof(Get), value);
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
        public async Task<ActionResult> Put(string id, [FromBody]PollDefinition value)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try { 
                await _pollDefinitionRepository.UpdatePollDefinition(id, value);
                return NoContent();
            }
            catch (Exception e) {
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
        public async Task<ActionResult> Delete(string id)
        {
            try {
                await _pollDefinitionRepository.RemovePollDefinition(id);
                return NoContent();
            }
            catch(Exception e) {
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
        public async Task<ActionResult> Publish(string id)
        {
            try {
                bool result = await _pollDefinitionRepository.PublishPollDefinition(id);
                if (result)
                    return Ok();
                else
                    return BadRequest("Poll definition does not exist or is already published");
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }

        }

    }
}
