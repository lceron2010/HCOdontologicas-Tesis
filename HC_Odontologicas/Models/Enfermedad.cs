using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
    public partial class Enfermedad
    {
        public Enfermedad()
        {
            AnamnesisEnfermedad = new HashSet<AnamnesisEnfermedad>();
        }

        public string Codigo { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

        public virtual ICollection<AnamnesisEnfermedad> AnamnesisEnfermedad { get; set; }
    }
}
