using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
	public partial class PlantillaCertificadoMedico
	{
		public string Codigo { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Nombre { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Descripcion { get; set; }

	}

	[NotMapped]
	public partial class CertificadoMedicoImprimir
	{
		public DateTime Fecha { get; set; }
		public string CodigoPaciente { get; set; }
		public String CodigoPersonal { get; set; }

		public string CedulaPaciente { get; set; }
		public string NombrePaciente { get; set; }
		public DateTime FechaCita { get; set; }
		public TimeSpan HoraInicio { get; set; }
		public TimeSpan HoraFin { get; set; }
		public string CIE10Codigo { get; set; }
		public Int32 Pieza { get; set; }
		public string Procedimiento { get; set; }
		public bool CitasSubsecuentes { get; set; }
		public bool Reposo { get; set; }
		public DateTime? FechaInicioReposo { get; set; }
		public DateTime? FechaFinReposo { get; set; }
		public DateTime? FechaReincorporarse { get; set; }


		public string CIE10Nombre { get; set; }
		public string FechaLetras { get; set; }


		public string NombreMedico { get; set; }
		public string CedulaMedico { get; set; }
		public string CorreoMedico { get; set; }
		public string CuerpoCertificado { get; set; }
		public string Observacion { get; set; }
		public string Recomendacion { get; set; }

		public int NumdiasReposo { get; set; }

	}


	[NotMapped]
	public partial class CertificadoMedicoPdf
	{
		public string Contenido { set; get; }
		public string Odontologo { set; get; }
		public string FechaActual { get; set; }

	}

}
