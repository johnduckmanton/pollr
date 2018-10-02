/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pollr.Api.Models;

namespace Pollr.Api.Dal
{
    internal class DatabaseContext
    {
        private IOptions<Settings> settings;
        private readonly IMongoDatabase _database = null;

        public DatabaseContext(IOptions<Settings> settings)
        {

            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<PollDefinition> PollDefinitions
        {
            get {
                return _database.GetCollection<PollDefinition>("polldefinitions");
            }
        }

        public IMongoCollection<Poll> Polls
        {
            get {
                return _database.GetCollection<Poll>("polls");
            }
        }
    }
}