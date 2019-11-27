using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
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

        public virtual HistoriaClinica CodigoHistoriaClinicaNavigation { get; set; }
    }
}
