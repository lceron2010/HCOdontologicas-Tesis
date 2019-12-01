using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class DiagnosticoCie10
    {
        public string Codigo { get; set; }
        public string CodigoDiagnostico { get; set; }
        public string CodigoCie10 { get; set; }

        public virtual Cie10 CodigoCie10Navigation { get; set; }
        public virtual Diagnostico CodigoDiagnosticoNavigation { get; set; }
    }
}
