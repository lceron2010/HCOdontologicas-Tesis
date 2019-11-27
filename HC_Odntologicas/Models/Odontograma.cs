using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class Odontograma
    {
        public Odontograma()
        {
            OdontogramaDetalle = new HashSet<OdontogramaDetalle>();
        }

        public string Codigo { get; set; }
        public string CodigoHistoriaClinica { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public bool Estado { get; set; }

        public virtual HistoriaClinica CodigoHistoriaClinicaNavigation { get; set; }
        public virtual ICollection<OdontogramaDetalle> OdontogramaDetalle { get; set; }
    }
}
