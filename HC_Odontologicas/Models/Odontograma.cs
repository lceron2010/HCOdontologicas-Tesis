﻿using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Odontograma
    {
        public Odontograma()
        {
            OdontogramaDetalle = new HashSet<OdontogramaDetalle>();
        }

        public string Codigo { get; set; }
        public string CodigoCitaOdontologica { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public bool Estado { get; set; }

        public virtual CitaOdontologica CitaOdontologica { get; set; }
        public virtual ICollection<OdontogramaDetalle> OdontogramaDetalle { get; set; }
    }
}
