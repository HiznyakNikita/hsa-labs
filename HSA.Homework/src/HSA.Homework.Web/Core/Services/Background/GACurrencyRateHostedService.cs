using GoogleMeasurementProtocol;
using GoogleMeasurementProtocol.Parameters.EventTracking;
using GoogleMeasurementProtocol.Parameters.User;
using HSA.Homework.Web.Models;
using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HSA.Homework.Web.Core.Services.Background
{
	public class GACurrencyRateHostedService : IHostedService
	{
		private const int ISO4217_UAH = 980;
		private const int ISO4217_USD = 840;

		private readonly CrontabSchedule _crontabSchedule;
		private readonly ICurrencyRateService _currencyRateService;
		private readonly IGAService _gaService;
		private DateTime _nextRun;
		private const string Schedule = "*/5 * * * * *"; // run each 5 sec

		public GACurrencyRateHostedService(ICurrencyRateService currencyRateService, IGAService gaService)
		{
			_crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
			_nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);

			_currencyRateService = currencyRateService;
			_gaService = gaService;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			Task.Run(async () =>
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					await Task.Delay(UntilNextExecution(), cancellationToken);

					var currencyRateInfo = await _currencyRateService.GetMonoCurrencyRateInfo(ISO4217_USD, ISO4217_UAH);
					if (currencyRateInfo != null)
					{
						await _gaService.SendGARequestForCurrencyRate("UAH", "USD", "buy", currencyRateInfo.RateBuy);
					}

					_nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
				}
			}, cancellationToken);

			return Task.CompletedTask;
		}

		private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
