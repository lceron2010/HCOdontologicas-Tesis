using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using HC_Odontologicas.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HC_Odontologicas.Models;
using HC_Odontologicas.Controllers;
using Microsoft.AspNetCore.Authentication;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using HC_Odontologicas.Areas.Services;

namespace HC_Odontologicas
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<HCOdontologicasContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			
			services.AddDefaultIdentity<UsuarioLogin>()
				.AddEntityFrameworkStores<HCOdontologicasContext>();


			services.AddTransient<IEmailSender, EmailSender>(i =>
			  new EmailSender(
				  Configuration["EmailSender:Host"],
				  Configuration.GetValue<int>("EmailSender:Port"),
				  Configuration.GetValue<bool>("EmailSender:EnableSSL"),
				  Configuration["EmailSender:UserName"],
				  Configuration["EmailSender:Password"]
				 )
			  );

			services.AddControllersWithViews();
			
			services.AddRazorPages();

			services.AddTransient<IClaimsTransformation, AddClaimsTransformation>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IHostingEnvironment env1)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});

			RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");
		}
	}
}
