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
		public string NumeroUnico { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Nombres { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Apellidos { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Identificacion { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public DateTime FechaNacimiento { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Genero { get; set; }		
		public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string MailPersonal { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string MailEpn { get; set; }		
		public string EstadoCivil { get; set; }
        public string DependenciaDondeTrabaja { get; set; }
        public string Cargo { get; set; }
        public string Procedencia { get; set; }		
		public string TipoPaciente { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public bool Estado { get; set; }
		public string NombreCompleto { get; set; }

		public string CodigoTipoIdentificacion { get; set; }
		public string CodigoFacultad { get; set; }
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
