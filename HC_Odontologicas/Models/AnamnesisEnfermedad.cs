using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
    public partial class AnamnesisEnfermedad
    {
        public string Codigo { get; set; }
        public string CodigoAnamnesis { get; set; }
        public string CodigoEnfermedad { get; set; }
		[NotMapped]
		public bool Seleccionado { get; set; }

		public virtual Anamnesis Anamnesis { get; set; }
        public virtual Enfermedad Enfermedad { get; set; }
    }
}
