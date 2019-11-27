using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            Doctor = new HashSet<Doctor>();
            PerfilDetalle = new HashSet<PerfilDetalle>();
        }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<Doctor> Doctor { get; set; }
        public virtual ICollection<PerfilDetalle> PerfilDetalle { get; set; }
    }
}
