using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Cie10
    {
        public Cie10()
        {
            DiagnosticoCie10 = new HashSet<DiagnosticoCie10>();
        }

        public string Codigo { get; set; }
        public string CodigoInterno { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        
        public virtual ICollection<DiagnosticoCie10> DiagnosticoCie10 { get; set; }
    }
}
