using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class PlantillaCertificadoMedico
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Contenido { get; set; }
        public bool Estado { get; set; }
    }
}
