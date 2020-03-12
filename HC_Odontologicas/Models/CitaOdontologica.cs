using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC_Odontologicas.Models
{
    public partial class CitaOdontologica
    {
        public CitaOdontologica()
        {
            Anamnesis = new HashSet<Anamnesis>();
            ConsentimientoInformado = new HashSet<ConsentimientoInformado>();
            Diagnostico = new HashSet<Diagnostico>();
            Odontograma = new HashSet<Odontograma>();            
            RecetaMedica = new HashSet<RecetaMedica>();
        }

        public string Codigo { get; set; }
        public string CodigoPaciente { get; set; }
        public string CodigoPersonal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
		public DateTime FechaInicio { get; set; }
		public DateTime FechaFin { get; set; }
		public TimeSpan HoraInicio { get; set; }
		public TimeSpan HoraFin { get; set; }
		public string UsuarioCreacion { get; set; }



        //paciente
        [NotMapped]
        public string NumeroUnico { get; set; }
        [NotMapped]
        public string Nombres { get; set; }
        [NotMapped]
        public string Apellidos { get; set; }
        [NotMapped]
        public string Identificacion { get; set; }
        [NotMapped]
        public DateTime FechaNacimiento { get; set; }
        [NotMapped] 
        public string Genero { get; set; }
        [NotMapped]
        public string Direccion { get; set; }
        [NotMapped]
        public string Telefono { get; set; }
        [NotMapped]
        public string Celular { get; set; }
        [NotMapped]
        public string MailPersonal { get; set; }
        [NotMapped]
        public string MailEpn { get; set; }
        [NotMapped]
        public string EstadoCivil { get; set; }
        [NotMapped]
        public string DependenciaDondeTrabaja { get; set; }
        [NotMapped]
        public string Cargo { get; set; }
        [NotMapped]
        public string Procedencia { get; set; }
        [NotMapped]
        public string TipoPaciente { get; set; }
        [NotMapped]
        public bool EstadoPaciente { get; set; }
        [NotMapped]
        public string NombreCompleto { get; set; }
        [NotMapped]
        public string CodigoTipoIdentificacion { get; set; }
        [NotMapped]
        public string CodigoFacultad { get; set; }
        [NotMapped]
        public string CodigoCarrera { get; set; }

        //anamnesis
        [NotMapped]
        public string MotivoConsulta { get; set; }
        [NotMapped]
        public string EnfermedadActual { get; set; }
        [NotMapped]
        public string Alerta { get; set; }
        [NotMapped]
        public string AntecedentesQuirurgicos { get; set; }
        [NotMapped]
        public string Alergico { get; set; }
        [NotMapped]
        public string Medicamentos { get; set; }
        [NotMapped]
        public string Habitos { get; set; }
        [NotMapped]
        public string AntecedentesFamiliares { get; set; }
        [NotMapped]
        public bool Fuma { get; set; }
        [NotMapped]
        public bool Embarazada { get; set; }
        [NotMapped]
        public DateTime? UltimaVisitaOdontologo { get; set; }
        [NotMapped]
        public string Endocrino { get; set; }
        [NotMapped]
        public string Traumatologico { get; set; }
        [NotMapped]
        public DateTime Fecha { get; set; }

        //odontograma      
        [NotMapped]
        public DateTime FechaActualizacion { get; set; }
        [NotMapped]
        public string ObservacionesOdontograma { get; set; }
        [NotMapped]
        public string EstadoOdontograma { get; set; }

        //diagnostico
        [NotMapped]
        public DateTime FechaDiagnostico { get; set; }
        [NotMapped]
        public int Pieza { get; set; }
        [NotMapped]
        public string Observacion { get; set; }
        [NotMapped]
        public string Recomendacion { get; set; }
        [NotMapped]
        public string Firma { get; set; }
        [NotMapped]
        public string CodigoDiagnosticoCie10 { get; set; }


        //consentimiento informado      
        [NotMapped]
        public string Descripcion { get; set; }
        [NotMapped]
        public string FirmaConcentimiento { get; set; }
        [NotMapped]
        public DateTime FechaConcentimiento { get; set; }

        //receta medica
        [NotMapped]
        public string DescripcionReceta { get; set; }
        [NotMapped]
        public DateTime FechaReceta { get; set; }
        [NotMapped]
        public string CodigoPlantillaRecetaMedica { get; set; }
        [NotMapped]
        public string Indicaciones { get; set; }




        public virtual Paciente Paciente { get; set; }
        public virtual Personal Personal { get; set; }
        public virtual ICollection<Anamnesis> Anamnesis { get; set; }
        public virtual ICollection<ConsentimientoInformado> ConsentimientoInformado { get; set; }
        public virtual ICollection<Diagnostico> Diagnostico { get; set; }
        public virtual ICollection<Odontograma> Odontograma { get; set; }        
        public virtual ICollection<RecetaMedica> RecetaMedica { get; set; }
    }
}
