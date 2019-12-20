﻿using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class HistoriaClinica
    {
        public HistoriaClinica()
        {
            Anamnesis = new HashSet<Anamnesis>();
            ConsentimientoInformado = new HashSet<ConsentimientoInformado>();
            Diagnostico = new HashSet<Diagnostico>();
            Odontograma = new HashSet<Odontograma>();
            OdontogramaDetalle = new HashSet<OdontogramaDetalle>();
            RecetaMedica = new HashSet<RecetaMedica>();
        }

        public string Codigo { get; set; }
        public string CodigoPaciente { get; set; }
        public string CodigoPersonal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Observaciones { get; set; }
        public bool Estado { get; set; }

        public virtual Paciente Paciente { get; set; }
        public virtual Personal Personal { get; set; }
        public virtual ICollection<Anamnesis> Anamnesis { get; set; }
        public virtual ICollection<ConsentimientoInformado> ConsentimientoInformado { get; set; }
        public virtual ICollection<Diagnostico> Diagnostico { get; set; }
        public virtual ICollection<Odontograma> Odontograma { get; set; }
        public virtual ICollection<OdontogramaDetalle> OdontogramaDetalle { get; set; }
        public virtual ICollection<RecetaMedica> RecetaMedica { get; set; }
    }
}