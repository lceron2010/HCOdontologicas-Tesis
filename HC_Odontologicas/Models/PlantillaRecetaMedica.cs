using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class PlantillaRecetaMedica
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
