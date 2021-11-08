using GoogleMeasurementProtocol;
using GoogleMeasurementProtocol.Parameters.EventTracking;
using GoogleMeasurementProtocol.Parameters.User;
using System;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Core.Services
{
	public class GAService : IGAService
	{
		public async Task SendGARequestForCurrencyRate(string currencyCodeA, string currencyCodeB, string rateType, decimal rate)
		{
			try
			{
				var clientId = new ClientId(Guid.NewGuid());
				var tid = "UA-211999513-2";
				var eventCategory = $"{currencyCodeA}-{currencyCodeB}-{rateType}-currency-rate";
				var eventAction = "get";
				var eventLabel = rate.ToString();

				//Create a factory based on your tracking id
				var factory = new GoogleAnalyticsRequestFactory(tid);

				//Create a PageView request by specifying request type
				var request = factory.CreateRequest(HitTypes.Event);

				//Add parameters to your request, each parameter has a corresponding class which has name = parameter name from google reference docs
				request.Parameters.Add(new EventCategory(eventCategory));
				request.Parameters.Add(new EventAction(eventAction));
				request.Parameters.Add(new EventLabel(eventLabel));

				await request.PostAsync(clientId);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error during send GA request: {e.Message} {e.StackTrace}");
			}
		}
	}
}
