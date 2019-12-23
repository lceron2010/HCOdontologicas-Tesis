using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
    public partial class Paciente
    {
        public Paciente()
        {
            Agenda = new HashSet<Agenda>();
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
		[Required(ErrorMessage = "Campo requerido.")]
		public string EstadoCivl { get; set; }
        public string DependenciaDondeTrabaja { get; set; }
        public string Cargo { get; set; }
        public string Procedencia { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string TipoPaciente { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public bool Estado { get; set; }
		public string NombreCompleto { get; set; }
		public string CodigoTipoIdentificacion { get; set; }

		public virtual ICollection<Agenda> Agenda { get; set; }
        public virtual ICollection<CitaOdontologica> CitaOdontologica { get; set; }
		public virtual TipoIdentificacion TipoIdentificacion { get; set; }
	}
}
