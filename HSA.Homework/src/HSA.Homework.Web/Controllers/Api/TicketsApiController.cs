using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HSA.Homework.Web;
using HSA.Homework.Web.Core.Services;

namespace HSA.Homework.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsApiController : ControllerBase
    {
        private readonly ITicketsService _ticketsService;

        public TicketsApiController(ITicketsService ticketsService)
        {
            _ticketsService = ticketsService;
        }

        [HttpGet("get/postgres")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return (await _ticketsService.AllPostgres()).ToList();
        }

        [HttpGet("get/mongo")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsMongo()
        {
            return (await _ticketsService.AllMongo()).ToList();
        }
    }
}
