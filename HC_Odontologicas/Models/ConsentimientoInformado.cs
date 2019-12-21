using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
    public partial class ConsentimientoInformado
    {
        public string Codigo { get; set; }
        public string CodigoHistoriaClinica { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Firma { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
		[NotMapped]
		public string CodigoPaciente { get; set; }
		[NotMapped]
		public string CodigoPersonal { get; set; }


		public virtual HistoriaClinica HistoriaClinica { get; set; }
    }
}
