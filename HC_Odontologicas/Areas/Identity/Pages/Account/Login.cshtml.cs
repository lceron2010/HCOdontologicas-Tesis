using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HC_Odontologicas.Models;

namespace HC_Odontologicas.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class LoginModel : PageModel
	{
		private readonly SignInManager<UsuarioLogin> _signInManager;
		private readonly ILogger<LoginModel> _logger;
		public LoginModel(SignInManager<UsuarioLogin> signInManager, ILogger<LoginModel> logger, HCOdontologicasContext context)
		{
			_signInManager = signInManager;
			_logger = logger;
		}
		[BindProperty]
		public InputModel Input { get; set; }
		public string ReturnUrl { get; set; }
		[TempData]
		public string ErrorMessage { get; set; }
		public class InputModel
		{
			[Required(ErrorMessage = "Ingrese su usuario")]			
			public string UsuarioLogin { get; set; }

			[Required(ErrorMessage = "Ingrese su contraseña")]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }

		}

		public async Task<IActionResult> OnGetAsync(string returnUrl = null)
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}
			returnUrl = returnUrl ?? Url.Content("~/");
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			ReturnUrl = returnUrl;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			try
			{
				returnUrl = returnUrl ?? Url.Content("~/");

				if (ModelState.IsValid)
				{
					return await Login(Input.UsuarioLogin, Input.Password, false);
				}
				// If we got this far, something failed, redisplay form
				return Page();
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return Page();
			}
		}

		private async Task<IActionResult> Login(string username, string password, bool isPersisten = false)
		{

			var result = await _signInManager.PasswordSignInAsync(username, password, isPersisten, lockoutOnFailure: true);
			if (result.Succeeded)
			{
				_logger.LogInformation("Usuario Logueado");
				//await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), username, null, null, "L");
				return Redirect("/Home");
			}
			else
			{
				string message = "El usuario o la contraseña son incorrectos. Verifique la tecla BLOQ MAYUS.";
				ModelState.AddModelError(string.Empty, message);
				return Page();
			}
		}
				
	}
}
