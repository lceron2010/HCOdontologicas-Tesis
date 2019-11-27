using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HC_Odntologicas.Models
{
    public partial class HCOdontologicasContext : DbContext
    {
        public HCOdontologicasContext()
        {
        }

        public HCOdontologicasContext(DbContextOptions<HCOdontologicasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agenda> Agenda { get; set; }
        public virtual DbSet<Anamnesis> Anamnesis { get; set; }
        public virtual DbSet<AnamnesisEnfermedad> AnamnesisEnfermedad { get; set; }
        public virtual DbSet<Cie10> Cie10 { get; set; }
        public virtual DbSet<ConsentimientoInformado> ConsentimientoInformado { get; set; }
        public virtual DbSet<Diagnostico> Diagnostico { get; set; }
        public virtual DbSet<DiagnosticoCie10> DiagnosticoCie10 { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<Enfermedad> Enfermedad { get; set; }
        public virtual DbSet<HistoriaClinica> HistoriaClinica { get; set; }
        public virtual DbSet<LogAuditoria> LogAuditoria { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Odontograma> Odontograma { get; set; }
        public virtual DbSet<OdontogramaDetalle> OdontogramaDetalle { get; set; }
        public virtual DbSet<Paciente> Paciente { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<PerfilDetalle> PerfilDetalle { get; set; }
        public virtual DbSet<PlantillaCertificadoMedico> PlantillaCertificadoMedico { get; set; }
        public virtual DbSet<PlantillaCorreoElectronico> PlantillaCorreoElectronico { get; set; }
        public virtual DbSet<RecetaMedica> RecetaMedica { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-MMSTLC9\\SQLEXPRESS; Database=HCOdontologicas; Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agenda>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoDoctor)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPaciente)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoDoctorNavigation)
                    .WithMany(p => p.Agenda)
                    .HasForeignKey(d => d.CodigoDoctor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Agenda_Doctor");

                entity.HasOne(d => d.CodigoPacienteNavigation)
                    .WithMany(p => p.Agenda)
                    .HasForeignKey(d => d.CodigoPaciente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Agenda_Paciente");
            });

            modelBuilder.Entity<Anamnesis>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Alergico)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Alerta)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.AntecedentesFamiliares)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.AntecedentesQuirurgicos)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoHistoriaClinica)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Endocrino)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.EnfermedadActual)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.GrupoSanguineo)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Habitos)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Medicamentos)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.MotivoConsulta)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Traumatologico)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoHistoriaClinicaNavigation)
                    .WithMany(p => p.Anamnesis)
                    .HasForeignKey(d => d.CodigoHistoriaClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anamnesis_HistoriaClinica");
            });

            modelBuilder.Entity<AnamnesisEnfermedad>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoAnamnesis)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoEnfermedad)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoAnamnesisNavigation)
                    .WithMany(p => p.AnamnesisEnfermedad)
                    .HasForeignKey(d => d.CodigoAnamnesis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnamnesisEnfermedad_Anamnesis");

                entity.HasOne(d => d.CodigoEnfermedadNavigation)
                    .WithMany(p => p.AnamnesisEnfermedad)
                    .HasForeignKey(d => d.CodigoEnfermedad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnamnesisEnfermedad_Enfermedad");
            });

            modelBuilder.Entity<Cie10>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CIE10");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoInterno)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ConsentimientoInformado>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoHistoriaClinica)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Firma)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoHistoriaClinicaNavigation)
                    .WithMany(p => p.ConsentimientoInformado)
                    .HasForeignKey(d => d.CodigoHistoriaClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsentimientoInformado_HistoriaClinica");
            });

            modelBuilder.Entity<Diagnostico>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoHistoriaClinica)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Firma)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoHistoriaClinicaNavigation)
                    .WithMany(p => p.Diagnostico)
                    .HasForeignKey(d => d.CodigoHistoriaClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Diagnostico_HistoriaClinica");
            });

            modelBuilder.Entity<DiagnosticoCie10>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("DiagnosticoCIE10");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoCie10)
                    .IsRequired()
                    .HasColumnName("CodigoCIE10")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoDiagnostico)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoCie10Navigation)
                    .WithMany(p => p.DiagnosticoCie10)
                    .HasForeignKey(d => d.CodigoCie10)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiagnosticoCIE10_CIE10");

                entity.HasOne(d => d.CodigoDiagnosticoNavigation)
                    .WithMany(p => p.DiagnosticoCie10)
                    .HasForeignKey(d => d.CodigoDiagnostico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiagnosticoCIE10_Diagnostico");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.HasIndex(e => e.CorreoElectronico)
                    .HasName("UK_Doctot_EMAIL")
                    .IsUnique();

                entity.HasIndex(e => e.NombreUsuario)
                    .HasName("UK_Doctor_NOMBREUSUARIO")
                    .IsUnique();

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPerfil)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Contrasenia)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoPerfilNavigation)
                    .WithMany(p => p.Doctor)
                    .HasForeignKey(d => d.CodigoPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOCTOR_REFERENCE_PERFIL");
            });

            modelBuilder.Entity<Enfermedad>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HistoriaClinica>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoDoctor)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPaciente)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoDoctorNavigation)
                    .WithMany(p => p.HistoriaClinica)
                    .HasForeignKey(d => d.CodigoDoctor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoriaClinica_Doctor");

                entity.HasOne(d => d.CodigoPacienteNavigation)
                    .WithMany(p => p.HistoriaClinica)
                    .HasForeignKey(d => d.CodigoPaciente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoriaClinica_Paciente");
            });

            modelBuilder.Entity<LogAuditoria>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Accion)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(54)
                    .IsUnicode(false);

                entity.Property(e => e.DireccionIp)
                    .HasColumnName("DireccionIP")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Tabla)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Accion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoMenu)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Icono)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CodigoMenuNavigation)
                    .WithMany(p => p.InverseCodigoMenuNavigation)
                    .HasForeignKey(d => d.CodigoMenu)
                    .HasConstraintName("FK_Menu_Menu");
            });

            modelBuilder.Entity<Odontograma>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoHistoriaClinica)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoHistoriaClinicaNavigation)
                    .WithMany(p => p.Odontograma)
                    .HasForeignKey(d => d.CodigoHistoriaClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Odontograma_HistoriaClinica");
            });

            modelBuilder.Entity<OdontogramaDetalle>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoOdontograma)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.CodigoOdontogramaNavigation)
                    .WithMany(p => p.OdontogramaDetalle)
                    .HasForeignKey(d => d.CodigoOdontograma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OdontogramaDetalle_HistoriaClinica");

                entity.HasOne(d => d.CodigoOdontograma1)
                    .WithMany(p => p.OdontogramaDetalle)
                    .HasForeignKey(d => d.CodigoOdontograma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OdontogramaDetalle_Odontograma");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Cargo)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.DependenciaDondeTrabaja)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCivl)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");

                entity.Property(e => e.Genero)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MailEpn)
                    .HasColumnName("MailEPN")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.MailPersonal)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroUnico)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Procedencia)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPaciente)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.Codigo)
                    .HasName("PK_Perfil_1");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PerfilDetalle>(entity =>
            {
                entity.HasKey(e => new { e.CodigoPerfil, e.CodigoMenu })
                    .HasName("PK_PerfilDetalle_1");

                entity.Property(e => e.CodigoPerfil)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoMenu)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoMenuNavigation)
                    .WithMany(p => p.PerfilDetalle)
                    .HasForeignKey(d => d.CodigoMenu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PerfilDetalle_Menu");

                entity.HasOne(d => d.CodigoPerfilNavigation)
                    .WithMany(p => p.PerfilDetalle)
                    .HasForeignKey(d => d.CodigoPerfil)
                    .HasConstraintName("FK_PerfilDetalle_Perfil");
            });

            modelBuilder.Entity<PlantillaCertificadoMedico>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(65)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlantillaCorreoElectronico>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Asunto)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Comentario)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Cuerpo)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecetaMedica>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoHistoriaClinica)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.CodigoHistoriaClinicaNavigation)
                    .WithMany(p => p.RecetaMedica)
                    .HasForeignKey(d => d.CodigoHistoriaClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecetaMedica_HistoriaClinica");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
