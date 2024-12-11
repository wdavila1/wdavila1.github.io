using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlanificadorActividades.EntityModels;

public partial class ActividadTareaPersona
{
    [Key]
    [Column(Order = 0)] // Orden explícito para claves compuestas
    public int ActividadId { get; set; }

    [Key]
    [Column(Order = 1)] // Orden explícito para claves compuestas
    public int TareaId { get; set; }

    public int? PersonaId { get; set; } // Puede ser null si no hay responsable asignado

    [ForeignKey("ActividadId")]
    [InverseProperty("ActividadTareaPersonas")]
    public virtual Actividad Actividad { get; set; } = null!;

    [ForeignKey("TareaId")]
    [InverseProperty("ActividadTareaPersonas")]
    public virtual Tarea Tarea { get; set; } = null!;

    [ForeignKey("PersonaId")]
    [InverseProperty("ActividadTareaPersonas")]
    public virtual Persona? Persona { get; set; } // Relación opcional con Persona
}
