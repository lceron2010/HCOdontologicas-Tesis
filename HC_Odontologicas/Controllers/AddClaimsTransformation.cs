﻿using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC_Odontologicas.Controllers
{
	public class AddClaimsTransformation : IClaimsTransformation
	{
		private readonly HCOdontologicasContext _context;
		private readonly UserManager<UsuarioLogin> _userManager;

		public AddClaimsTransformation(HCOdontologicasContext context, UserManager<UsuarioLogin> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
		{
			try
			{
				var identity = (ClaimsIdentity)principal.Identity;
				var user = _context.Usuario.Include(u => u.Perfil).SingleOrDefault(p => p.CorreoElectronico == identity.Name);

				identity.AddClaim(new Claim("CodigoPerfil", user.CodigoPerfil));
				identity.AddClaim(new Claim("NombreCompleto", user.CorreoElectronico));
				identity.AddClaim(new Claim("NombrePerfil", user.Perfil.Nombre));
				if (user.CodigoPersonal == null)
				{
					identity.AddClaim(new Claim("CodigoPersonal", ""));
				}
				else
				{
					identity.AddClaim(new Claim("CodigoPersonal", user.CodigoPersonal));
				}


				var perfilDetalle = _context.PerfilDetalle.Include(p => p.Menu).Where(p => p.CodigoPerfil == user.CodigoPerfil).ToList();
				List<Claim> perfilDetalles = new List<Claim>();
				foreach (var item in perfilDetalle)
					if (!item.Menu.Accion.Contains("#"))
						perfilDetalles.Add(new Claim(item.Menu.Accion, item.Ver + ";" + item.Crear + ";" + item.Editar + ";" + item.Eliminar + ";" + item.Exportar + ";" + item.Importar));
				identity.AddClaims(perfilDetalles);
				return Task.FromResult(principal);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

		}
	}
}