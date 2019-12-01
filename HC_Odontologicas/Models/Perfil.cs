﻿using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            PerfilDetalle = new HashSet<PerfilDetalle>();
            Personal = new HashSet<Personal>();
            Usuario = new HashSet<Usuario>();
        }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<PerfilDetalle> PerfilDetalle { get; set; }
        public virtual ICollection<Personal> Personal { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
