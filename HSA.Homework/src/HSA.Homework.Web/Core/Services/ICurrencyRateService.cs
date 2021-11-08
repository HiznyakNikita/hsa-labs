using HSA.Homework.Web.Models;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Core.Services
{
	public interface ICurrencyRateService
	{
		Task<CurrencyRateInfo> GetMonoCurrencyRateInfo(int currencyCodeA, int currencyCodeB);
	}
}
