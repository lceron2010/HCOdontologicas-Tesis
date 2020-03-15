using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
    public partial class Personal
    {
        public Personal()
        {           
			CitaOdontologica = new HashSet<CitaOdontologica>();
        }

        public string Codigo { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Identificacion { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string CorreoElectronico { get; set; }

        public bool Estado { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string CodigoTipoIdentificacion { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string CodigoCargo { get; set; }
		public string NombreCompleto { get; set; }
		
		public virtual Cargo Cargo { get; set; }		
		public virtual TipoIdentificacion TipoIdentificacion { get; set; }
		     
        public virtual ICollection<CitaOdontologica> CitaOdontologica { get; set; }
    }
}
