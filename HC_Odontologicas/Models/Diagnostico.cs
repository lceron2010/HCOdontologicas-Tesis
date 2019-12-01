using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Diagnostico
    {
        public Diagnostico()
        {
            DiagnosticoCie10 = new HashSet<DiagnosticoCie10>();
        }

        public string Codigo { get; set; }
        public string CodigoHistoriaClinica { get; set; }
        public DateTime Fecha { get; set; }
        public int Pieza { get; set; }
        public string Observacion { get; set; }
        public string Firma { get; set; }
        public bool Estado { get; set; }

        public virtual HistoriaClinica CodigoHistoriaClinicaNavigation { get; set; }
        public virtual ICollection<DiagnosticoCie10> DiagnosticoCie10 { get; set; }
    }
}
