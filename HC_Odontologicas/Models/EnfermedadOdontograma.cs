using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
	public partial class EnfermedadOdontograma
	{
		public EnfermedadOdontograma()
		{
			OdontogramaDetalle = new HashSet<OdontogramaDetalle>();
		}

		public int Codigo { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }

		public virtual ICollection<OdontogramaDetalle> OdontogramaDetalle { get; set; }
	}
}
