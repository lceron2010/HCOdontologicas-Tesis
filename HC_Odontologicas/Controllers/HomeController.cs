﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HC_Odontologicas.Models;
using System.Security.Claims;

namespace HC_Odontologicas.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			var i = (ClaimsIdentity)User.Identity;
			var cookie = Request.Cookies;
			if (i.IsAuthenticated)
			{
				return View();
			}
			//else if (cookie.Count == 1)
			//    return Redirect("../Identity/Account/AccessDenied");
			else
			{
				return Redirect("../Identity/Account/Login");
			}
			//return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}