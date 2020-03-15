using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
    public partial class PlantillaConsentimientoInformado
    {		

		public string Codigo { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Descripcion { get; set; }       

		
	}
}
