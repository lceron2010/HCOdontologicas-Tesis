using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
    public partial class Paciente
    {
        public Paciente()
        {            
			CitaOdontologica = new HashSet<CitaOdontologica>();
        }

        public string Codigo { get; set; }
		
		[Required(ErrorMessage = "Campo requerido.")]
		[Display(Name = "Número Único")]
		public string NumeroUnico { get; set; }
		
		[Required(ErrorMessage = "Campo requerido.")]
		public string Nombres { get; set; }
		
		[Required(ErrorMessage = "Campo requerido.")]
		public string Apellidos { get; set; }
		
		[Required(ErrorMessage = "Campo requerido.")]
		[Display(Name = "Identificación")]
		public string Identificacion { get; set; }
		
		[Required(ErrorMessage = "Campo requerido.")]
		[Display(Name = "Fecha Nacimiento")]
		[Remote(action: "VerificarFechaNac", controller: "Validaciones")]
		public DateTime FechaNacimiento { get; set; }
		
		[Required(ErrorMessage = "Campo requerido.")]
		[Display(Name = "Género")]
		[Remote(action: "VerificarGenero", controller: "Validaciones")]
		public string Genero { get; set; }

		[Display(Name = "Dirección")]
		public string Direccion { get; set; }

		[Display(Name = "Teléfono")]
		public string Telefono { get; set; }

		[Display(Name = "Celular")]
		public string Celular { get; set; }
		
		[EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
		[Display(Name = "Correo Electrónico Personal")]
		public string MailPersonal { get; set; }
		
		[Required(ErrorMessage = "Campo requerido.")]
		[EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
		[Display(Name = "Correo Electrónico EPN*")]
		public string MailEpn { get; set; }

		[Remote(action: "VerificarEstadoCivil", controller: "Validaciones")]
		[Display(Name = "Estado Civil")]
		public string EstadoCivil { get; set; }

		[Display(Name = "Dependencia Donde Trabaja")]
		public string DependenciaDondeTrabaja { get; set; }
		
		public string Cargo { get; set; }
        public string Procedencia { get; set; }

		[Remote(action: "ValidarTipoPaciente", controller: "Validaciones")]
		[Display(Name = "Celular")]
		public string TipoPaciente { get; set; }
		
		public bool Estado { get; set; }
		public string NombreCompleto { get; set; }

		[Remote(action: "ValidarCodigoTipoIdentificacion", controller: "Validaciones")]
		public string CodigoTipoIdentificacion { get; set; }
		
		[Remote(action: "VerificarCodigoFacultad", controller: "Validaciones")]
		public string CodigoFacultad { get; set; }

		[Remote(action: "VerificarCodigoCarrera", controller: "Validaciones")]
		public string CodigoCarrera { get; set; }

		public virtual Carrera Carrera { get; set; }
		public virtual Facultad Facultad { get; set; }		
        public virtual ICollection<CitaOdontologica> CitaOdontologica { get; set; }
		public virtual TipoIdentificacion TipoIdentificacion { get; set; }

		[NotMapped]
		public string CedulaNombre
		{
			get
			{
				return Identificacion + "-" + NombreCompleto;
			}
		}
	}
}
