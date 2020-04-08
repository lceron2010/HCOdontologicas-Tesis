using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(HC_Odontologicas.Areas.Identity.IdentityHostingStartup))]
namespace HC_Odontologicas.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
			});
		}
	}
}