using HSA.Homework.Web.Core.Cache;
using HSA.Homework.Web.Core.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Datalayer.Mongo
{
    public class MongoTicketRepository : IMongoTicketRepository
    {
        private readonly IMongoDatabase _mongoDb;
        private readonly MongoSettings _mongoSettings;
        private readonly IMemoryCache _cache;

        private const string _cacheKey = "tickets_mongo";

        public MongoTicketRepository(
            IOptions<MongoSettings> mongoSettings,
            IMemoryCache cache)
        {
            _mongoSettings = mongoSettings.Value;

            var dbClient = new MongoClient(_mongoSettings.ConnectionString);
            _mongoDb = dbClient.GetDatabase(_mongoSettings.DatabaseName);

            _cache = cache;
        }

        public async Task Insert(List<Ticket> tickets)
        {
            var collection = _mongoDb.GetCollection<Ticket>(_mongoSettings.TicketsCollectionName);
            await collection.InsertManyAsync(tickets);
        }

        public async Task<IEnumerable<Ticket>> GetAll(int count = 500)
        {
            var tickets = await GetTickets(count: count);

            return tickets;
        }

        public async Task<List<Ticket>> GetTickets(int beta = 1, int count = 500)
        {
            List<Ticket> tickets = new List<Ticket>();
            if (!_cache.TryGetValue(_cacheKey, out CacheEntry<List<Ticket>> entry)
                || entry.ShouldBeRecalculated(beta))
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var filter = new BsonDocument();
                var collection = _mongoDb.GetCollection<Ticket>(_mongoSettings.TicketsCollectionName);
                tickets = await collection.Find(filter).Limit(count).ToListAsync();

                stopwatch.Stop();

                _cache.Set(_cacheKey, 
                    new CacheEntry<List<Ticket>>()
                    {
                        Value = tickets,
                        TimeToRecomputeValueInSeconds = stopwatch.ElapsedMilliseconds / 1000,
                        Expiry = DateTime.Now.AddSeconds(10)
                    },
                    new MemoryCacheEntryOptions()
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(10)
                    });

                return tickets;
            }

            return entry.Value;
        }
    }
}
