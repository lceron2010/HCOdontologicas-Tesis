using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC_Odontologicas.Models
{
	public class UsuarioLogin : IdentityUser
	{
		public UsuarioLogin() : base() { }
				
		public string CodigoPerfil { get; set; }
		public string Password { get; set; }
		public string NombreCompleto { get; set; }
	}
	
}
