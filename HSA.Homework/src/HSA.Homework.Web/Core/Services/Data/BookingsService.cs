using System;
using System.Collections.Generic;
using System.Linq;

namespace HSA.Homework.Web.Core.Services
{
	public class BookingsService : IBookingsService
	{
		private readonly AirlinesDbContext _dbContext;

		public BookingsService(AirlinesDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IEnumerable<Booking> All()
		{
			return _dbContext.Bookings.ToList();
		}
	}
}
