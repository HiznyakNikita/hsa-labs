using HSA.Homework.Web.Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Datalayer.Mongo
{
	public class MongoTicketRepository : IMongoTicketRepository
	{
        private readonly IMongoDatabase _mongoDb;
		private readonly MongoSettings _mongoSettings;

		public MongoTicketRepository(IOptions<MongoSettings> mongoSettings)
        {
            _mongoSettings = mongoSettings.Value;

            var dbClient = new MongoClient(_mongoSettings.ConnectionString);
            _mongoDb = dbClient.GetDatabase(_mongoSettings.DatabaseName);
		}

        public async Task Insert(List<Ticket> tickets)
        {
            var collection = _mongoDb.GetCollection<Ticket>(_mongoSettings.TicketsCollectionName);
            await collection.InsertManyAsync(tickets);
        }

        public async Task<IEnumerable<Ticket>> GetAll()
        {
            var filter = new BsonDocument();
            var collection = _mongoDb.GetCollection<Ticket>(_mongoSettings.TicketsCollectionName);
            return (await collection.FindAsync(filter)).ToList();
        }
    }
}
