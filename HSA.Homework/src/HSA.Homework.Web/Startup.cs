using HSA.Homework.Web.Core.Services;
using HSA.Homework.Web.Core.Services.Background;
using HSA.Homework.Web.Core.Settings;
using HSA.Homework.Web.Datalayer.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HSA.Homework.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMemoryCache();

			//Configure options
			services.Configure<MongoSettings>(Configuration.GetSection("MongoSettings"));

			//Repositories
			services.AddTransient<IMongoTicketRepository, MongoTicketRepository>();

			//Services
			services.AddTransient<ITicketsService, TicketsService>();
			services.AddTransient<IBookingsService, BookingsService>();
			services.AddTransient<IGAService, GAService>();

			services.AddHttpClient<ICurrencyRateService, CurrencyRateService>();

			//Hosted services
			services.AddHostedService<GACurrencyRateHostedService>();

			services.AddDbContext<AirlinesDbContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("PostgreSQLConnection")));

			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
