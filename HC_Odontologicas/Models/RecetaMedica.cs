using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class RecetaMedica
    {
        public string Codigo { get; set; }
        public string CodigoHistoriaClinica { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }

        public virtual HistoriaClinica CodigoHistoriaClinicaNavigation { get; set; }
    }
}
