﻿using System;
using System.Collections.Generic;

namespace HC_Odntologicas.Models
{
    public partial class LogAuditoria
    {
        public long Codigo { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Tabla { get; set; }
        public string Clave { get; set; }
        public string Accion { get; set; }
        public string DireccionIp { get; set; }
    }
}
