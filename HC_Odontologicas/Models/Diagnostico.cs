using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
    public partial class Diagnostico
    {
        public Diagnostico()
        {
			DiagnosticoCie10 = new List<DiagnosticoCie10>();
        }

        public string Codigo { get; set; }
        public string CodigoCitaOdontologica { get; set; }
        public DateTime Fecha { get; set; }
        public int Pieza { get; set; }
        public string Observacion { get; set; }
        public string Firma { get; set; }        
		[NotMapped]
		public string CodigoPaciente { get; set; }
		[NotMapped]
		public string CodigoPersonal { get; set; }

		public virtual CitaOdontologica CitaOdontologica { get; set; }
        public virtual List<DiagnosticoCie10> DiagnosticoCie10 { get; set; }
    }
}
