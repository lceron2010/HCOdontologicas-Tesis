using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Paciente
    {
        public Paciente()
        {
            Agenda = new HashSet<Agenda>();
            HistoriaClinica = new HashSet<HistoriaClinica>();
        }

        public string Codigo { get; set; }
        public string NumeroUnico { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string MailPersonal { get; set; }
        public string MailEpn { get; set; }
        public string EstadoCivl { get; set; }
        public string DependenciaDondeTrabaja { get; set; }
        public string Cargo { get; set; }
        public string Procedencia { get; set; }
        public string TipoPaciente { get; set; }
        public bool Estado { get; set; }

        public virtual ICollection<Agenda> Agenda { get; set; }
        public virtual ICollection<HistoriaClinica> HistoriaClinica { get; set; }
    }
}
