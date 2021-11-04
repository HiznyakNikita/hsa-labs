using System.Threading.Tasks;
using HSA.Homework.Web.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HSA.Homework.Web.Controllers
{
	public class TicketsController : Controller
    {
		private readonly ITicketsService _ticketsService;
		private readonly IBookingsService _bookingsService;

		public TicketsController(ITicketsService ticketsService, IBookingsService bookingsService)
        {
			_ticketsService = ticketsService;
			_bookingsService = bookingsService;
		}

        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketsService.All(1000);
            return View(tickets);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketsService.Get(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        public IActionResult Create()
        {
            ViewData["BookRef"] = new SelectList(_bookingsService.All(), "BookRef", "BookRef");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketNo,BookRef,PassengerId,PassengerName,ContactData")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                await _ticketsService.Insert(ticket);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookRef"] = new SelectList(_bookingsService.All(), "BookRef", "BookRef", ticket.BookRef);
            return View(ticket);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketsService.Get(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["BookRef"] = new SelectList(_bookingsService.All(), "BookRef", "BookRef", ticket.BookRef);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TicketNo,BookRef,PassengerId,PassengerName,ContactData")] Ticket ticket)
        {
            if (id != ticket.TicketNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _ticketsService.Update(id, ticket);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookRef"] = new SelectList(_bookingsService.All(), "BookRef", "BookRef", ticket.BookRef);
            return View(ticket);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketsService.Get(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _ticketsService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        public  async Task<IActionResult> Sync()
        {
            var count = 1000;
            var time = await _ticketsService.SyncBenchmark(count);
            ViewData["SyncBenchmark"] = $"Synchronized {count} tickets. Time: {time} ms"; 
            return View();
        }
    }
}
