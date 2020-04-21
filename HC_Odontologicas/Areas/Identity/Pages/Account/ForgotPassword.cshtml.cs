using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HC_Odontologicas.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<UsuarioLogin> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly HCOdontologicasContext _context;        

        public ForgotPasswordModel(UserManager<UsuarioLogin> userManager, IEmailSender emailSender, HCOdontologicasContext context)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Ingrese su correo electrónico")]
            [EmailAddress(ErrorMessage = "Ingrese un correo electrónico valido")]
            [Display(Name = "Correo electrónico*")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    PlantillaCorreoElectronico correo = new PlantillaCorreoElectronico();
                    correo = _context.PlantillaCorreoElectronico.SingleOrDefault(p => p.Asunto.Contains("Recuperar Contrasenia"));
                    string cuerpo = FuncionesEmail.RecuperarContrasenia(correo.Cuerpo, user.Email, user.UserName, user.Password);
                    string message = await FuncionesEmail.EnviarEmail(_emailSender, Input.Email, correo.Asunto, cuerpo);

                    ModelState.AddModelError(string.Empty, message);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                else
                {
                    string message = "Correo electrónico no registrado.";
                    ModelState.AddModelError(string.Empty, message);
                    return Page();
                }
            }

            return Page();
        }
    }
}
