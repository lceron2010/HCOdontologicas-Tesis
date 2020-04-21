using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
	public partial class PlantillaCorreoElectronico
	{
		public string Codigo { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Asunto { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Cuerpo { get; set; }


	}
}
