/*---------------------------------------------------------------------------------------------
 *  Copyright Async(c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pollr.Api.Exceptions;
using Pollr.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pollr.Api.Dal
{
    /// <summary>
    /// Handles all data access for Poll Definitions
    /// </summary>
    public class PollDefinitionRepository : IPollDefinitionRepository
    {
        private readonly DatabaseContext _context = null;
        private ILogger _logger;


        public PollDefinitionRepository(IOptions<DatabaseSettings> settings,
            ILogger<PollDefinitionRepository> logger)
        {
            _context = new DatabaseContext(settings);
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
                var filter = Builders<PollDefinition>.Filter.Eq("Published", true);
                return await _context.PollDefinitions.Find(filter).ToListAsync();
            }
            else
            {
                return await _context.PollDefinitions.Find(_ => true).ToListAsync();
            }

        }

        /// <summary>
        /// Return the poll definition matching the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PollDefinition> GetPollDefinitionAsync(string id)
        {

            var filter = Builders<PollDefinition>.Filter.Eq(s => s.Id, ObjectId.Parse(id));

            PollDefinition pollDefinition = await _context.PollDefinitions
                            .Find(filter)
                            .FirstOrDefaultAsync();

            if (pollDefinition == null)
                throw new PollDefNotFoundException();

            return pollDefinition;
        }

        /// <summary>
        /// Create a new poll definition
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddPollDefinitionAsync(PollDefinition item)
        {
            item.Id = ObjectId.GenerateNewId();
            await _context.PollDefinitions.InsertOneAsync(item);
        }

        /// <summary>
        /// Delete a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemovePollDefinitionAsync(string id)
        {
            DeleteResult actionResult = await _context.PollDefinitions.DeleteOneAsync(
                    Builders<PollDefinition>.Filter.Eq(s => s.Id, ObjectId.Parse(id)));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;

        }

        /// <summary>
        /// Update a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePollDefinitionAsync(string id, PollDefinition item)
        {
            ReplaceOneResult actionResult
                = await _context.PollDefinitions
                                .ReplaceOneAsync(n => n.Id.Equals(ObjectId.Parse(id))
                                        , item
                                        , new UpdateOptions { IsUpsert = true });
            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Set a poll definition to published
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> PublishPollDefinitionAsync(string id)
        {
            var builder = Builders<PollDefinition>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(id));
            var update = Builders<PollDefinition>.Update
                            .Set(s => s.IsPublished, true);

            UpdateResult actionResult
                    = await _context.PollDefinitions.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;

        }

        /// <summary>
        /// Set a poll definition to unpublished
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> UnpublishPollDefinitionAsync(string id)
        {
            var builder = Builders<PollDefinition>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(id));
            //& builder.Where(s => s.IsPublished == false);
            var update = Builders<PollDefinition>.Update
                            .Set(s => s.IsPublished, false);

            UpdateResult actionResult
                    = await _context.PollDefinitions.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }
    }
}


