using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pollr.Api.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
using System.Security.Authentication;

namespace PollDefinitionListApp
{
    public class Dal : IDisposable
    {
        //private MongoServer mongoServer = null;
        private bool disposed = false;

        // To do: update the connection string with the DNS name
        // or IP address of your server. 
        //For example, "mongodb://testlinux.cloudapp.net
        private string userName = "FILLME";
        private string host = "FILLME";
        private string password = "FILLME";

        // This sample uses a database named "PollDefinitions" and a 
        //collection named "PollDefinitionsList".  The database and collection 
        //will be automatically created if they don't already exist.
        private string dbName = "Pollr";
        private string collectionName = "PollDefinitions";

        // Default constructor.        
        public Dal()
        {
        }

        // Gets all PollDefinition items from the MongoDB server.        
        public List<PollDefinition> GetAllPollDefinitions()
        {
            try {
                var collection = GetPollDefinitionsCollection();
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException) {
                return new List<PollDefinition>();
            }
        }

        // Creates a PollDefinition and inserts it into the collection in MongoDB.
        public void CreatePollDefinition(PollDefinition pollDefinition)
        {
            var collection = GetPollDefinitionsCollectionForEdit();
            try {
                collection.InsertOne(pollDefinition);
            }
            catch (MongoCommandException ex) {
                string msg = ex.Message;
            }
        }

        private IMongoCollection<PollDefinition> GetPollDefinitionsCollection()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var pollDefinitionCollection = database.GetCollection<PollDefinition>(collectionName);
            return pollDefinitionCollection;
        }

        private IMongoCollection<PollDefinition> GetPollDefinitionsCollectionForEdit()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var pollDefinitionCollection = database.GetCollection<PollDefinition>(collectionName);
            return pollDefinitionCollection;
        }

        # region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed) {
                if (disposing) {
                }
            }

            this.disposed = true;
        }

        # endregion
    }
}
