using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Agenda = new HashSet<Agenda>();
            HistoriaClinica = new HashSet<HistoriaClinica>();
        }

        public string Codigo { get; set; }
        public string CodigoPerfil { get; set; }
        public string Nombre { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Estado { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }

        public virtual Perfil CodigoPerfilNavigation { get; set; }
        public virtual ICollection<Agenda> Agenda { get; set; }
        public virtual ICollection<HistoriaClinica> HistoriaClinica { get; set; }
    }
}
