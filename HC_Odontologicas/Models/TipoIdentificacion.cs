﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC_Odontologicas.Models
{
	public class TipoIdentificacion
	{
		public TipoIdentificacion()
		{
			Paciente = new HashSet<Paciente>();
			Personal = new HashSet<Personal>();
		}

		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public bool Estado { get; set; }

		public virtual ICollection<Paciente> Paciente { get; set; }
		public virtual ICollection<Personal> Personal { get; set; }
	}
}