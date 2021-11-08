using HSA.Homework.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Core.Services
{
	public class CurrencyRateService : ICurrencyRateService
	{
		private readonly HttpClient _httpClient;
		private readonly IMemoryCache _cache;
		private const string _monoCurrencyApiUrl = "https://api.monobank.ua/bank/currency";
		private const string _currencyRateCacheKey = "USD_UAH_rate";
		private readonly MemoryCacheEntryOptions _cacheEntryOptions;

		public CurrencyRateService(HttpClient httpClient, IMemoryCache cache)
		{
			_httpClient = httpClient;
			_cache = cache;
			_cacheEntryOptions = new MemoryCacheEntryOptions()
				.SetSlidingExpiration(TimeSpan.FromMinutes(5));
		}
		public async Task<CurrencyRateInfo> GetMonoCurrencyRateInfo(int currencyCodeA, int currencyCodeB)
		{
			try
			{
				if (_cache.TryGetValue(_currencyRateCacheKey, out CurrencyRateInfo currencyRateInfo))
				{
					return currencyRateInfo;
				}
				var response = _httpClient.GetAsync(_monoCurrencyApiUrl);
				var str = await response.Result.Content.ReadAsStringAsync();
				var currencyRatesInfo = await response.Result.Content.ReadAsAsync<List<CurrencyRateInfo>>();

				currencyRateInfo = currencyRatesInfo.FirstOrDefault(r => r.CurrencyCodeA == currencyCodeA && r.CurrencyCodeA == currencyCodeB);
				_cache.Set(_currencyRateCacheKey, currencyRateInfo, _cacheEntryOptions);

				return currencyRateInfo;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during get rate from Mono: {ex.Message} {ex.StackTrace}");
				return null;
			}
		}
	}
}
