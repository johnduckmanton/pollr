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
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<PollDefinition> GetPollDefinition(string id)
        {

            var filter = Builders<PollDefinition>.Filter.Eq("_Id", ObjectId.Parse(id));

            try {
                return await _context.PollDefinitions
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddPollDefinition(PollDefinition item)
        {
            try {
                await _context.PollDefinitions.InsertOneAsync(item);
            }
            catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemovePollDefinition(string id)
        {
            try {
                DeleteResult actionResult = await _context.PollDefinitions.DeleteOneAsync(
                        Builders<PollDefinition>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdatePollDefinition(string id, PollDefinition item)
        {
            try {
                ReplaceOneResult actionResult
                    = await _context.PollDefinitions
                                    .ReplaceOneAsync(n => n._Id.Equals(ObjectId.Parse(id))
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }
        }

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
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllPollDefinitions()
        {
            try {
                DeleteResult actionResult
                    = await _context.PollDefinitions.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }
        }
    }
}


