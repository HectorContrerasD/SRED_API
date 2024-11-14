using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace SRED_API.Models.Entities;

public partial class WebsitosSredContext : DbContext
{
    public WebsitosSredContext()
    {
    }

    public WebsitosSredContext(DbContextOptions<WebsitosSredContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aula> Aula { get; set; }

    public virtual DbSet<Equipo> Equipo { get; set; }

    public virtual DbSet<Reporte> Reporte { get; set; }

    public virtual DbSet<Tipoequipo> Tipoequipo { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.HasKey(e => e.IdAula).HasName("PRIMARY");

            entity.ToTable("aula");

            entity.Property(e => e.IdAula)
                .HasColumnType("int(11)")
                .HasColumnName("idAula");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)");
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.IdEquipo).HasName("PRIMARY");

            entity.ToTable("equipo");

            entity.HasIndex(e => e.TipoEquipoIdTipoEquipo, "equipo_ibfk_1");

            entity.HasIndex(e => e.AulaIdAula, "equipo_ibfk_2");

            entity.Property(e => e.IdEquipo)
                .HasColumnType("int(11)")
                .HasColumnName("idEquipo");
            entity.Property(e => e.AulaIdAula)
                .HasColumnType("int(11)")
                .HasColumnName("Aula_idAula");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)");
            entity.Property(e => e.NumeroIdentificacion).HasMaxLength(15);
            entity.Property(e => e.TipoEquipoIdTipoEquipo)
                .HasColumnType("int(11)")
                .HasColumnName("TipoEquipo_idTipoEquipo");

            entity.HasOne(d => d.AulaIdAulaNavigation).WithMany(p => p.Equipo)
                .HasForeignKey(d => d.AulaIdAula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("equipo_ibfk_2");

            entity.HasOne(d => d.TipoEquipoIdTipoEquipoNavigation).WithMany(p => p.Equipo)
                .HasForeignKey(d => d.TipoEquipoIdTipoEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("equipo_ibfk_1");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("PRIMARY");

            entity.ToTable("reporte");

            entity.HasIndex(e => e.EquipoIdEquipo, "reporte_ibfk_1");

            entity.Property(e => e.IdReporte)
                .HasColumnType("int(11)")
                .HasColumnName("idReporte");
            entity.Property(e => e.Descripcion).HasMaxLength(45);
            entity.Property(e => e.EquipoIdEquipo)
                .HasColumnType("int(11)")
                .HasColumnName("Equipo_idEquipo");
            entity.Property(e => e.Estado).HasColumnType("tinyint(4)");
            entity.Property(e => e.Folio).HasMaxLength(10);
            entity.Property(e => e.NoControlAl).HasMaxLength(8);

            entity.HasOne(d => d.EquipoIdEquipoNavigation).WithMany(p => p.Reporte)
                .HasForeignKey(d => d.EquipoIdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reporte_ibfk_1");
        });

        modelBuilder.Entity<Tipoequipo>(entity =>
        {
            entity.HasKey(e => e.IdTipoEquipo).HasName("PRIMARY");

            entity.ToTable("tipoequipo");

            entity.Property(e => e.IdTipoEquipo)
                .HasColumnType("int(11)")
                .HasColumnName("idTipoEquipo");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)");
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("idUsuario");
            entity.Property(e => e.NumeroControl).HasMaxLength(45);
            entity.Property(e => e.Rol).HasColumnType("tinyint(4)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
