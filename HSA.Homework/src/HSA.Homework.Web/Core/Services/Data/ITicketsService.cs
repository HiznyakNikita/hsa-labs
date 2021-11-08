using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Core.Services
{
	public interface ITicketsService
	{
        Task<IEnumerable<Ticket>> All(int count = 1000);
        Task<IEnumerable<Ticket>> AllMongo();
        Task<IEnumerable<Ticket>> AllPostgres(int count = 1000);

        Task<Ticket> Get(string id);

        Task<Ticket> Insert(Ticket ticket);

        Task<Ticket> Update(string id, Ticket ticket);

        Task Delete(string id);

        Task<long> SyncBenchmark(int count = 1000);
    }
}
