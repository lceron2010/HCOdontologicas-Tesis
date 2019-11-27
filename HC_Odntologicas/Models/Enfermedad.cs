using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class Enfermedad
    {
        public Enfermedad()
        {
            AnamnesisEnfermedad = new HashSet<AnamnesisEnfermedad>();
        }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

        public virtual ICollection<AnamnesisEnfermedad> AnamnesisEnfermedad { get; set; }
    }
}
