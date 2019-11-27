using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class PerfilDetalle
    {
        public string CodigoPerfil { get; set; }
        public string CodigoMenu { get; set; }
        public bool Ver { get; set; }
        public bool Crear { get; set; }
        public bool Editar { get; set; }
        public bool Eliminar { get; set; }
        public bool Exportar { get; set; }
        public bool Importar { get; set; }

        public virtual Menu CodigoMenuNavigation { get; set; }
        public virtual Perfil CodigoPerfilNavigation { get; set; }
    }
}
