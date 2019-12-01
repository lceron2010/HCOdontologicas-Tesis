using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Agenda
    {
        public string Codigo { get; set; }
        public string CodigoPaciente { get; set; }
        public string CodigoPersonal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool Estado { get; set; }
        public string Observaciones { get; set; }

        public virtual Paciente CodigoPacienteNavigation { get; set; }
        public virtual Personal CodigoPersonalNavigation { get; set; }
    }
}
