using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class OdontogramaDetalle
    {
        public string Codigo { get; set; }
        public string CodigoOdontograma { get; set; }
        public DateTime Fecha { get; set; }
		
        public virtual CitaOdontologica CitaOdontologica { get; set; }
    }
}
