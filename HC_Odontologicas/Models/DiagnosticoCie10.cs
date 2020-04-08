using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
    public partial class DiagnosticoCie10
    {
        public string Codigo { get; set; }
        public string CodigoDiagnostico { get; set; }
        public string CodigoCie10 { get; set; }
		[NotMapped]
		public bool Seleccionado { get; set; }
		
		public virtual Cie10 Cie10 { get; set; }
        public virtual Diagnostico Diagnostico { get; set; }
    }
}
