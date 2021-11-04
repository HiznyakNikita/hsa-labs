using HSA.Homework.Web.Datalayer.Mongo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Core.Services
{
	public class TicketsService: ITicketsService
	{
		private readonly AirlinesDbContext _context;
		private readonly IMongoTicketRepository _mongoTicketRepository;

		public TicketsService(AirlinesDbContext context, IMongoTicketRepository mongoTicketRepository)
		{
			_context = context;
			_mongoTicketRepository = mongoTicketRepository;
		}

        public async Task<IEnumerable<Ticket>> All(int count = 1000)
        {
            var airlinesDbContext = _context.Tickets
                .Include(t => t.BookRefNavigation)
                .Take(count);
            return await airlinesDbContext.ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> AllPostgres(int count = 1000)
        {
            var airlinesDbContext = _context.Tickets
                .Take(count);
            return await airlinesDbContext.ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> AllMongo()
        {
            var tickets = await _mongoTicketRepository.GetAll();
            return tickets;
        }

        public async Task<Ticket> Get(string id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.BookRefNavigation)
                .FirstOrDefaultAsync(m => m.TicketNo == id);
         
            return ticket;
        }

        public async Task<Ticket> Insert(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket> Update(string id, Ticket ticket)
        {
            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(ticket.TicketNo))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return ticket;
        }

        public async Task Delete(string id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        private bool TicketExists(string id)
        {
            return _context.Tickets.Any(e => e.TicketNo == id);
        }

        public async Task<long> SyncBenchmark(int count = 1000)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var airlinesDbContext = _context.Tickets
                .Take(count);
            var tickets = await airlinesDbContext.ToListAsync();
            await _mongoTicketRepository.Insert(tickets);

            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }
    }
}
