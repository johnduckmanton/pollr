/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pollr.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Dal
{
    public class PollDefinitionRepository : IPollDefinitionRepository
    {
        private readonly DatabaseContext _context = null;

        public PollDefinitionRepository(IOptions<Settings> settings)
        {
            _context = new DatabaseContext(settings);
        }

        /// <summary>
        /// Return all poll definitions
        /// </summary>
        /// <param name="publishedOnly"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PollDefinition>> GetPollDefinitions(bool publishedOnly = false)
        {
            try {
                if (publishedOnly) {
                    var filter = Builders<PollDefinition>.Filter.Eq("Published", true);
                    return await _context.PollDefinitions.Find(filter).ToListAsync();
                }
                else {
                    return await _context.PollDefinitions.Find(_ => true).ToListAsync();
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Return the poll definition matching the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PollDefinition> GetPollDefinition(string id)
        {

            var filter = Builders<PollDefinition>.Filter.Eq("_Id", ObjectId.Parse(id));

            try {
                return await _context.PollDefinitions
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Create a new poll definition
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddPollDefinition(PollDefinition item)
        {
            try {
                item.Id = ObjectId.GenerateNewId();
                await _context.PollDefinitions.InsertOneAsync(item);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Delete a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemovePollDefinition(string id)
        {
            try {
                DeleteResult actionResult = await _context.PollDefinitions.DeleteOneAsync(
                        Builders<PollDefinition>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Update a poll definition
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePollDefinition(string id, PollDefinition item)
        {
            try {
                ReplaceOneResult actionResult
                    = await _context.PollDefinitions
                                    .ReplaceOneAsync(n => n.Id.Equals(ObjectId.Parse(id))
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Set a poll definition to published
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> PublishPollDefinition(string id)
        {
            var builder = Builders<PollDefinition>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(id));
                //& builder.Where(s => s.IsPublished == false);
            var update = Builders<PollDefinition>.Update
                            .Set(s => s.IsPublished, true);

            try {
                UpdateResult actionResult
                     = await _context.PollDefinitions.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Set a poll definition to unpublished
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> UnpublishPollDefinition(string id)
        {
            var builder = Builders<PollDefinition>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(id));
            //& builder.Where(s => s.IsPublished == false);
            var update = Builders<PollDefinition>.Update
                            .Set(s => s.IsPublished, false);

            try {
                UpdateResult actionResult
                     = await _context.PollDefinitions.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}


