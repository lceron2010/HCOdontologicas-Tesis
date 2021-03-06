﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
	public partial class Anamnesis
	{
		public Anamnesis()
		{
			AnamnesisEnfermedad = new List<AnamnesisEnfermedad>();
		}

		public string Codigo { get; set; }
		public string CodigoCitaOdontologica { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string MotivoConsulta { get; set; }
		public string EnfermedadActual { get; set; }
		public string Alerta { get; set; }
		public string AntecedentesQuirurgicos { get; set; }
		public string Alergico { get; set; }
		public string Medicamentos { get; set; }
		public string Habitos { get; set; }
		public string AntecedentesFamiliares { get; set; }
		public bool Fuma { get; set; }
		public bool Embarazada { get; set; }
		public DateTime? UltimaVisitaOdontologo { get; set; }
		public string Endocrino { get; set; }
		public string Traumatologico { get; set; }
		public DateTime Fecha { get; set; }
		[NotMapped]
		public string CodigoPaciente { get; set; }
		[NotMapped]
		public string CodigoPersonal { get; set; }



		public virtual CitaOdontologica CitaOdontologica { get; set; }
		public virtual List<AnamnesisEnfermedad> AnamnesisEnfermedad { get; set; }
	}
}
