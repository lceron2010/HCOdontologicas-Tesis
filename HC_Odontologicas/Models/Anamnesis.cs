using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Anamnesis
    {
        public Anamnesis()
        {
            AnamnesisEnfermedad = new HashSet<AnamnesisEnfermedad>();
        }

        public string Codigo { get; set; }
        public string CodigoHistoriaClinica { get; set; }
        public string MotivoConsulta { get; set; }
        public string EnfermedadActual { get; set; }
        public string Alerta { get; set; }
        public string AntecedentesQuirurgicos { get; set; }
        public string Alergico { get; set; }
        public string Medicamentos { get; set; }
        public string Habitos { get; set; }
        public string AntecedentesFamiliares { get; set; }
        public bool? Fuma { get; set; }
        public bool? Embarazada { get; set; }
        public string GrupoSanguineo { get; set; }
        public string Endocrino { get; set; }
        public string Traumatologico { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual HistoriaClinica CodigoHistoriaClinicaNavigation { get; set; }
        public virtual ICollection<AnamnesisEnfermedad> AnamnesisEnfermedad { get; set; }
    }
}
