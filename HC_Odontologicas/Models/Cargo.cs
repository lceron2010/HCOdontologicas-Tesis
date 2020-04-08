using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
	public class Cargo
	{
		public Cargo()
		{
			Personal = new HashSet<Personal>();
		}

		public string Codigo { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Nombre { get; set; }
		public string Descripcion { get; set; }

		public virtual ICollection<Personal> Personal { get; set; }
	}
}
