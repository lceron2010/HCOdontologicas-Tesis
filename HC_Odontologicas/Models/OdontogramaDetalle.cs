using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class OdontogramaDetalle
    {
		public string Codigo { get; set; }
		public string CodigoOdontograma { get; set; }
		public int Pieza { get; set; }
		public string Region { get; set; }
		public string Enfermedad { get; set; }
		public bool Valor { get; set; }
		public string Diagnostico { get; set; }

		public virtual Odontograma Odontograma { get; set; }
		public virtual EnfermedadOdontograma EnfermedadOdontograma { get; set; }
		public virtual RegionPiezaDental RegionPiezaDental { get; set; }
		
    }
}
