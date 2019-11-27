using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class OdontogramaDetalle
    {
        public string Codigo { get; set; }
        public string CodigoOdontograma { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Odontograma CodigoOdontograma1 { get; set; }
        public virtual HistoriaClinica CodigoOdontogramaNavigation { get; set; }
    }
}
