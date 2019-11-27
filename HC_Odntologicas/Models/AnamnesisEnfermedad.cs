using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class AnamnesisEnfermedad
    {
        public string Codigo { get; set; }
        public string CodigoAnamnesis { get; set; }
        public string CodigoEnfermedad { get; set; }

        public virtual Anamnesis CodigoAnamnesisNavigation { get; set; }
        public virtual Enfermedad CodigoEnfermedadNavigation { get; set; }
    }
}
