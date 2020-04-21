using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
	public partial class Carrera
	{
		public Carrera()
		{
			Paciente = new HashSet<Paciente>();
		}

		public string Codigo { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string CodigoFacultad { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Nombre { get; set; }
		public string Descripcion { get; set; }


		public virtual Facultad Facultad { get; set; }
		public virtual ICollection<Paciente> Paciente { get; set; }
	}
}
