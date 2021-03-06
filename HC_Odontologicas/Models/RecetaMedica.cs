﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
	public partial class RecetaMedica
	{
		public string Codigo { get; set; }
		public string CodigoCitaOdontologica { get; set; }
		public string Descripcion { get; set; }
		public DateTime Fecha { get; set; }
		public string CodigoPlantillaRecetaMedica { get; set; }
		public string Indicaciones { get; set; }

		[NotMapped]
		public string CodigoPaciente { get; set; }
		[NotMapped]
		public string CodigoPersonal { get; set; }

		[NotMapped]
		public string fechaString { get; set; }

		public virtual CitaOdontologica CitaOdontologica { get; set; }
		public virtual PlantillaRecetaMedica PlantillaRecetaMedica { get; set; }
	}
}
