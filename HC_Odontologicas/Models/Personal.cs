using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Personal
    {
        public Personal()
        {
            Agenda = new HashSet<Agenda>();
            HistoriaClinica = new HashSet<HistoriaClinica>();
        }

        public string Codigo { get; set; }
        public string CodigoPerfil { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Estado { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }

        public virtual Perfil CodigoPerfilNavigation { get; set; }
        public virtual ICollection<Agenda> Agenda { get; set; }
        public virtual ICollection<HistoriaClinica> HistoriaClinica { get; set; }
    }
}
