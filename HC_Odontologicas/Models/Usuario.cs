using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Usuario
    {
        public string Codigo { get; set; }
        public string CodigoPerfil { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Estado { get; set; }

        public virtual Perfil CodigoPerfilNavigation { get; set; }
    }
}
