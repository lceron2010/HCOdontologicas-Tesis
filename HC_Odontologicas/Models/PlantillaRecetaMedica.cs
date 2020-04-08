using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
	public partial class PlantillaRecetaMedica
	{
		public PlantillaRecetaMedica()
		{
			RecetaMedica = new HashSet<RecetaMedica>();
		}

		public string Codigo { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Nombre { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Descripcion { get; set; }


		public virtual ICollection<RecetaMedica> RecetaMedica { get; set; }
	}
}
