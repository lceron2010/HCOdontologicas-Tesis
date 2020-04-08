using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
	public partial class Odontograma
	{
		public Odontograma()
		{
			OdontogramaDetalle = new List<OdontogramaDetalle>();
		}

		public string Codigo { get; set; }
		public string CodigoCitaOdontologica { get; set; }
		public DateTime FechaActualizacion { get; set; }
		public string Observaciones { get; set; }
		public string Estado { get; set; }
		[NotMapped]
		public string CodigoPaciente { get; set; }
		[NotMapped]
		public string CodigoPersonal { get; set; }

		public virtual CitaOdontologica CitaOdontologica { get; set; }
		public virtual List<OdontogramaDetalle> OdontogramaDetalle { get; set; }
	}
}
