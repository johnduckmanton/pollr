/*---------------------------------------------------------------------------------------------
 *  Copyright Async(c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pollr.Api.Exceptions;
using Pollr.Api.Models.PollDefinitions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Data
{
    /// <summary>
    /// Handles all data access for Poll Definitions
    /// </summary>
    public class PollDefinitionRepository : IPollDefinitionRepository
    {
        private readonly PollrContext _context = null;
        private readonly ILogger _logger;


        public PollDefinitionRepository(DbContextOptions<PollrContext> options,
            ILogger<PollDefinitionRepository> logger)
        {
            _context = new PollrContext(options);
            _logger = logger;
        }

        /// <summary>
        /// Return all poll definitions
        /// </summary>
        /// <param name="publishedOnly"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PollDefinition>> GetPollDefinitionsAsync(bool publishedOnly = false)
        {
            if (publishedOnly)
            {
                return await _context.PollDefinitions
                    .Include(p => p.Questions)
                    .ThenInclude(q => q.Answers)
                    .Where(p => p.IsPublished == true)
                    .ToListAsync();
            }
            else
            {
                return await _context.PollDefinitions
                    .Include(p => p.Questions)
                    .ThenInclude(q => q.Answers)
                    .ToListAsync();
            }

        }

        /// <summary>
        /// Return the poll definition matching the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PollDefinition> GetPollDefinitionAsync(int id)
        {

            var pollDefinition = await _context.PollDefinitions
                    .Include(p => p.Questions)
                    .ThenInclude(q => q.Answers)
                    .Where(p => p.Id == id)
                    .FirstOrDefaultAsync();
            if (pollDefinition == null)
                throw new PollDefNotFoundException();

            return pollDefinition;
        }

        /// <summary>
        /// Create a new poll definition
        /// </summary>
        /// <param name="pollDefinition"></param>
        /// <returns></returns>
        public async Task<PollDefinition> AddPollDefinitionAsync(PollDefinition pollDefinition)
        {
            await _context.PollDefinitions
                .AddAsync(pollDefinition);
            await _context.SaveChangesAsync();

            return pollDefinition;
        }

        /// <summary>
        /// Delete a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemovePollDefinitionAsync(int id)
        {

            var pollDefinition = await _context.PollDefinitions.FindAsync(id);
            if (pollDefinition == null)
                throw new PollDefNotFoundException();

            _context.PollDefinitions.Remove(pollDefinition);
            return (await _context.SaveChangesAsync() > 0);
        }

        /// <summary>
        /// Update a poll definition
        /// </summary>
        /// <param name="pollDefinition"></param>
        /// <returns></returns>
        public async Task<PollDefinition> UpdatePollDefinitionAsync(PollDefinition pollDefinition)
        {

            _context.PollDefinitions.Update(pollDefinition);

            try
            {
                await _context.SaveChangesAsync();
                return pollDefinition;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollDefinitionExists(pollDefinition.Id))
                {
                    throw new PollDefNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Set a poll definition to published/unpublished
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isPublished"></param>
        /// <returns>bool</returns>
        /// 
        public async Task<bool> SetPublishedStatusAsync(int id, bool isPublished)
        {

            var pollDefinition = await _context.PollDefinitions.FindAsync(id);
            if (pollDefinition == null)
                throw new PollDefNotFoundException();

            pollDefinition.IsPublished = isPublished;
            _context.Entry(pollDefinition).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync() > 0);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollDefinitionExists(id))
                {
                    throw new PollDefNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool PollDefinitionExists(int id)
        {
            return _context.PollDefinitions.Any(e => e.Id == id);
        }

    }
}


