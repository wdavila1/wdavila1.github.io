using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PlanificadorActividades.EntityModels;

public partial class PlanificadorActividadesContext : DbContext
{
    public PlanificadorActividadesContext()
    {
    }

    public PlanificadorActividadesContext(DbContextOptions<PlanificadorActividadesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actividad> Actividades { get; set; } = null!;
    public virtual DbSet<Persona> Personas { get; set; } = null!;
    public virtual DbSet<Tarea> Tareas { get; set; } = null!;
    public virtual DbSet<ActividadTareaPersona> ActividadTareaPersonas { get; set; } = null!;

    public virtual DbSet<TareaPersona> TareaPersonas { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string database = "PlanificadorActividades.db";
            string dir = Environment.CurrentDirectory;
            string path = string.Empty;

            if (dir.EndsWith("net9.0"))
            {
                // En el directorio <proyecto>\bin\<Debug|Release>\net9.0.
                path = Path.Combine("..", "..", "..", "..", database);
            }
            else
            {
                // En el directorio principal del proyecto.
                path = Path.Combine("..", database);
            }

            path = Path.GetFullPath(path); // Convertir a ruta absoluta.

            try
            {
                PlanificadorActividadesLogger.WriteLine($"Ruta de la base de datos: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    message: $"{path} no encontrado.", fileName: path);
            }

            optionsBuilder.UseSqlite($"Data Source={path}");
            optionsBuilder.LogTo(PlanificadorActividadesLogger.WriteLine,
                new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        }
    }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.HasIndex(e => e.Fecha, "FechaIndex");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasIndex(e => e.NombreTarea, "NombreTareaIndex");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasIndex(e => e.NombrePersona, "NombrePersonaIndex");
        });

        OnModelCreatingPartial(modelBuilder);
    }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configuración para ActividadTareaPersona
     modelBuilder.Entity<ActividadTareaPersona>(entity =>
    {
        entity.HasKey(e => new { e.ActividadId, e.TareaId }); // Configuración de llave compuesta

        entity.HasOne(atp => atp.Actividad)
            .WithMany(a => a.ActividadTareaPersonas)
            .HasForeignKey(atp => atp.ActividadId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(atp => atp.Tarea)
            .WithMany(t => t.ActividadTareaPersonas)
            .HasForeignKey(atp => atp.TareaId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(atp => atp.Persona)
            .WithMany(p => p.ActividadTareaPersonas)
            .HasForeignKey(atp => atp.PersonaId)
            .OnDelete(DeleteBehavior.SetNull); // Si se elimina la persona, el campo se pone en null
    });

    // Configuración para TareaPersona
    modelBuilder.Entity<TareaPersona>(entity =>
    {
        entity.HasKey(e => new { e.TareaId, e.PersonaId });
    });

    modelBuilder.Entity<Actividad>(entity =>
    {
        entity.HasIndex(e => e.Fecha, "FechaIndex");
    });

    modelBuilder.Entity<Tarea>(entity =>
    {
        entity.HasIndex(e => e.NombreTarea, "NombreTareaIndex");
    });

    modelBuilder.Entity<Persona>(entity =>
    {
        entity.HasIndex(e => e.NombrePersona, "NombrePersonaIndex");
    });

    OnModelCreatingPartial(modelBuilder);
}



    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

