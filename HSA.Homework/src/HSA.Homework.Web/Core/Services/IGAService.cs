using System.Threading.Tasks;

namespace HSA.Homework.Web.Core.Services
{
	public interface IGAService
	{
		Task SendGARequestForCurrencyRate(string currencyCodeA, string currencyCodeB, string rateType, decimal rate);
	}
}
