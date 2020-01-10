﻿using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Facultad
    {
        public Facultad()
        {
            Carrera = new HashSet<Carrera>();
            Paciente = new HashSet<Paciente>();
        }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        

        public virtual ICollection<Carrera> Carrera { get; set; }
        public virtual ICollection<Paciente> Paciente { get; set; }
    }
}
