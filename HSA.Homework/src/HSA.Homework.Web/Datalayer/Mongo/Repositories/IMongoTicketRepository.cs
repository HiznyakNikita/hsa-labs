using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Datalayer.Mongo
{
	public interface IMongoTicketRepository
	{
		Task Insert(List<Ticket> tickets);
		Task<IEnumerable<Ticket>> GetAll();
	}
}
