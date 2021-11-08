using System.Collections.Generic;

namespace HSA.Homework.Web.Core.Services
{
	public interface IBookingsService
	{
		IEnumerable<Booking> All();
	}
}
