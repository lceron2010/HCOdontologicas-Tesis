﻿using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class PlantillaCorreoElectronico
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
    }
}