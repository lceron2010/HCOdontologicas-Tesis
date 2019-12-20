using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HC_Odontologicas.Models
{
	public partial class HCOdontologicasContext : IdentityDbContext<UsuarioLogin>
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
		public virtual DbSet<Cargo> Cargo { get; set; }
		public virtual DbSet<Cie10> Cie10 { get; set; }
		public virtual DbSet<ConsentimientoInformado> ConsentimientoInformado { get; set; }
		public virtual DbSet<Diagnostico> Diagnostico { get; set; }
		public virtual DbSet<DiagnosticoCie10> DiagnosticoCie10 { get; set; }
		public virtual DbSet<Enfermedad> Enfermedad { get; set; }
		public virtual DbSet<HistoriaClinica> HistoriaClinica { get; set; }
		public virtual DbSet<LogAuditoria> LogAuditoria { get; set; }
		public virtual DbSet<Menu> Menu { get; set; }
		public virtual DbSet<Odontograma> Odontograma { get; set; }
		public virtual DbSet<OdontogramaDetalle> OdontogramaDetalle { get; set; }
		public virtual DbSet<Paciente> Paciente { get; set; }
		public virtual DbSet<Perfil> Perfil { get; set; }
		public virtual DbSet<PerfilDetalle> PerfilDetalle { get; set; }
		public virtual DbSet<Personal> Personal { get; set; }
		public virtual DbSet<PlantillaCertificadoMedico> PlantillaCertificadoMedico { get; set; }
		public virtual DbSet<PlantillaCorreoElectronico> PlantillaCorreoElectronico { get; set; }
		public virtual DbSet<RecetaMedica> RecetaMedica { get; set; }
		public virtual DbSet<Usuario> Usuario { get; set; }
		public virtual DbSet<TipoIdentificacion> TipoIdentificacion { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=DESKTOP-MMSTLC9\\SQLEXPRESS; Database=HCOdontologicas; Integrated Security=true");
			}
			optionsBuilder.ConfigureWarnings(warnings =>
		  warnings.Default(WarningBehavior.Ignore)
				  .Log(CoreEventId.IncludeIgnoredWarning)
				  .Throw(RelationalEventId.QueryClientEvaluationWarning));
			optionsBuilder.EnableSensitiveDataLogging(true);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Agenda>(entity =>
			{
				entity.HasKey(e => e.Codigo);

				entity.Property(e => e.Codigo)
					.HasMaxLength(8)
					.IsUnicode(false);

				entity.Property(e => e.CodigoPaciente)
					.IsRequired()
					.HasMaxLength(8)
					.IsUnicode(false);

				entity.Property(e => e.CodigoPersonal)
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

				entity.HasOne(d => d.CodigoPacienteNavigation)
					.WithMany(p => p.Agenda)
					.HasForeignKey(d => d.CodigoPaciente)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Agenda_Paciente");

				entity.HasOne(d => d.CodigoPersonalNavigation)
					.WithMany(p => p.Agenda)
					.HasForeignKey(d => d.CodigoPersonal)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Agenda_Personal");
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

				entity.HasOne(d => d.HistoriaClinica)
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

			modelBuilder.Entity<Cargo>(entity =>
			{
				entity.HasKey(e => e.Codigo);

				entity.Property(e => e.Codigo)
					.HasMaxLength(4)
					.IsUnicode(false);

				entity.Property(e => e.Descripcion)
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.Nombre)
					.IsRequired()
					.HasMaxLength(64)
					.IsUnicode(false);
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

				entity.Property(e => e.CodigoPaciente)
					.IsRequired()
					.HasMaxLength(8)
					.IsUnicode(false);

				entity.Property(e => e.CodigoPersonal)
					.IsRequired()
					.HasMaxLength(8)
					.IsUnicode(false);

				entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

				entity.Property(e => e.Observaciones)
					.HasMaxLength(512)
					.IsUnicode(false);

				entity.HasOne(d => d.Paciente)
					.WithMany(p => p.HistoriaClinica)
					.HasForeignKey(d => d.CodigoPaciente)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_HistoriaClinica_Paciente");

				entity.HasOne(d => d.Personal)
					.WithMany(p => p.HistoriaClinica)
					.HasForeignKey(d => d.CodigoPersonal)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_HistoriaClinica_Personal");
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

				entity.HasOne(d => d.MenuCodigo)
					.WithMany(p => p.SubMenu)
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

				entity.Property(e => e.Identificacion)
					.IsRequired()
					.HasMaxLength(13)
					.IsUnicode(false);

				entity.Property(e => e.Celular)
					.HasMaxLength(32)
					.IsUnicode(false);

				entity.Property(e => e.CodigoTipoIdentificacion)
				   .HasMaxLength(4)
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

				entity.Property(e => e.NombreCompleto)
				  .IsRequired()
				  .HasMaxLength(129)
				  .IsUnicode(false)
				  .HasComputedColumnSql("(([Empleado].[Apellidos]+' ')+[Empleado].[Nombres])");

				entity.HasOne(d => d.CodigoTipoIdentificacionNavigation)
				   .WithMany(p => p.Paciente)
				   .HasForeignKey(d => d.CodigoTipoIdentificacion)
				   .HasConstraintName("FK_Paciente_TipoIdentificacion");

			});

			modelBuilder.Entity<Perfil>(entity =>
			{
				entity.HasKey(e => e.Codigo)
					.HasName("PK_Perfil");

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
					.HasName("PK_PerfilDetalle");

				entity.Property(e => e.CodigoPerfil)
					.HasMaxLength(4)
					.IsUnicode(false);

				entity.Property(e => e.CodigoMenu)
					.HasMaxLength(4)
					.IsUnicode(false);

				entity.HasOne(d => d.Menu)
					.WithMany(p => p.PerfilDetalle)
					.HasForeignKey(d => d.CodigoMenu)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_PerfilDetalle_Menu");

				entity.HasOne(d => d.CodigoPerfilNavigation)
					.WithMany(p => p.PerfilDetalle)
					.HasForeignKey(d => d.CodigoPerfil)
					.HasConstraintName("FK_PerfilDetalle_Perfil");
			});

			modelBuilder.Entity<Personal>(entity =>
			{
				entity.HasKey(e => e.Codigo);

				entity.HasIndex(e => e.Apellidos)
					.HasName("UK_Personal_NOMBREUSUARIO")
					.IsUnique();

				entity.HasIndex(e => e.CorreoElectronico)
					.HasName("UK_Personal_EMAIL")
					.IsUnique();

				entity.Property(e => e.Codigo)
					.HasMaxLength(8)
					.IsUnicode(false);

				entity.Property(e => e.Apellidos)
					.IsRequired()
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.Celular)
					.HasMaxLength(20)
					.IsUnicode(false);

				entity.Property(e => e.CodigoCargo)
					.IsRequired()
					.HasMaxLength(4)
					.IsUnicode(false);

				entity.Property(e => e.CodigoPerfil)
					.IsRequired()
					.HasMaxLength(4)
					.IsUnicode(false);

				entity.Property(e => e.CodigoTipoIdentificacion)
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

				entity.Property(e => e.Direccion)
					.IsRequired()
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.Identificacion)
					.IsRequired()
					.HasMaxLength(15)
					.IsUnicode(false);

				entity.Property(e => e.NombreUsuario)
					.IsRequired()
					.HasMaxLength(64)
					.IsUnicode(false);

				entity.Property(e => e.Nombres)
					.IsRequired()
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.Telefono)
					.HasMaxLength(20)
					.IsUnicode(false);

				entity.Property(e => e.NombreCompleto)
				  .IsRequired()
				  .HasMaxLength(129)
				  .IsUnicode(false)
				  .HasComputedColumnSql("(([Empleado].[Apellidos]+' ')+[Empleado].[Nombres])");

				entity.HasOne(d => d.Cargo)
					.WithMany(p => p.Personal)
					.HasForeignKey(d => d.CodigoCargo)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Personal_Cargo");

				entity.HasOne(d => d.Perfil)
				   .WithMany(p => p.Personal)
				   .HasForeignKey(d => d.CodigoPerfil)
				   .OnDelete(DeleteBehavior.ClientSetNull)
				   .HasConstraintName("FK_Personal_Perfil");

				entity.HasOne(d => d.TipoIdentificacion)
					.WithMany(p => p.Personal)
					.HasForeignKey(d => d.CodigoTipoIdentificacion)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Personal_TipoIdentificacion");
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

			modelBuilder.Entity<Usuario>(entity =>
			{
				entity.HasKey(e => e.Codigo);

				entity.Property(e => e.Codigo)
					.HasMaxLength(8)
					.IsUnicode(false);

				entity.Property(e => e.CodigoPerfil)
					.IsRequired()
					.HasMaxLength(4)
					.IsUnicode(false);

				entity.Property(e => e.Contrasenia)
					.IsRequired()
					.HasMaxLength(32)
					.IsUnicode(false);

				entity.Property(e => e.CorreoElectronico)
					.IsRequired()
					.HasMaxLength(64)
					.IsUnicode(false);

				entity.Property(e => e.NombreUsuario)
					.IsRequired()
					.HasMaxLength(64)
					.IsUnicode(false);

				entity.HasOne(d => d.CodigoPerfilNavigation)
					.WithMany(p => p.Usuario)
					.HasForeignKey(d => d.CodigoPerfil)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Usuario_Perfil");
			});

			modelBuilder.Entity<UsuarioLogin>(entity =>
			{				
				entity.Property(e => e.CodigoPerfil)
					.HasMaxLength(4)
					.IsUnicode(false);

				entity.Property(e => e.Password)
					.HasMaxLength(64)
					.IsUnicode(false);

				entity.Property(e => e.NombreCompleto)
				.HasMaxLength(129);
			});

			modelBuilder.Entity<TipoIdentificacion>(entity =>
			{
				entity.HasKey(e => e.Codigo);

				entity.Property(e => e.Codigo)
					.HasMaxLength(4)
					.IsUnicode(false);

				entity.Property(e => e.Descripcion)
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.Nombre)
					.IsRequired()
					.HasMaxLength(64)
					.IsUnicode(false);
			});

			// OnModelCreatingPartial(modelBuilder);
		}

	}    // partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
}
