using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
    public partial class PlantillaCertificadoMedico
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

    }

    [NotMapped]
    public partial class CertificadoMedicoImprimir
    {
        public DateTime Fecha { get; set; }
        public string CodigoPaciente { get; set; }
        public String CodigoPersonal { get; set; }

        public string NombrePaciente { get; set; }
        public string CedulaPaciente { get; set; }
        public string CIE10Nombre { get; set; }
        public string CIE10Codigo { get; set; }
        public string Pieza { get; set; }
        public string NumHorasReposo { get; set; }
        public string FechaLetras { get; set; }
        public string Observacion { get; set; }
        public string Recomendacion { get; set; }
        public string NombreMedico { get; set; }
        public string CedulaMedico { get; set; }
        public string CorreoMedico { get; set; }
        public string CuerpoCertificado { get; set; }
    }
}
