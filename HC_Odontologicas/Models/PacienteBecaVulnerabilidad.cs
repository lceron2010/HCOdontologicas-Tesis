using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC_Odontologicas.Models
{
	public class PacienteBecaVulnerabilidad
	{
		public string Codigo { get; set; }
		public string NombreCompleto { get; set; }
		public string Cedula { get; set; }
		public string TipoBeca { get; set; }		
		public string Carrera { get; set; }
		public string Periodo { get; set; }
		public string EmailPersonal { get; set; }
		public string EmailEPN { get; set; }
		public Boolean Estado { get; set; }
		public string Campania { get; set; }
		public int Grupo { get; set; }

	}
}
