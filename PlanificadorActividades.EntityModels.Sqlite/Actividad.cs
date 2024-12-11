using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlanificadorActividades.EntityModels;

[Index("Fecha", Name = "FechaIndex")]
public partial class Actividad
{
    [Key]
    public int ActividadId { get; set; }

    [Required]
    [StringLength(100)]
    public string NombreActividad { get; set; } = null!;

    [Column(TypeName = "date")]
    [Required]
    public DateTime Fecha { get; set; }

    [InverseProperty("Actividad")]
    public virtual ICollection<ActividadTareaPersona> ActividadTareaPersonas { get; set; } = new List<ActividadTareaPersona>();

    // Relación con Persona, para las personas asociadas a tareas
        [NotMapped]
        public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
