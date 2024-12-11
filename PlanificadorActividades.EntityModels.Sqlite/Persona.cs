using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlanificadorActividades.EntityModels;

[Index("NombrePersona", Name = "NombrePersonaIndex")]
public partial class Persona
{
    [Key]
    public int PersonaId { get; set; }

    [Required]
    [StringLength(100)]
    public string NombrePersona { get; set; } = null!;

    // Relación con las tareas que esta persona tiene asignadas como responsable
    public virtual ICollection<Tarea> TareasResponsables { get; set; } = new List<Tarea>();

    [InverseProperty("Persona")]
    public virtual ICollection<ActividadTareaPersona> ActividadTareaPersonas { get; set; } = new List<ActividadTareaPersona>();

    [InverseProperty("Persona")]
    public virtual ICollection<TareaPersona> TareaPersonas { get; set; } = new List<TareaPersona>();
}

