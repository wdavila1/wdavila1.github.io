using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlanificadorActividades.EntityModels;

[Index("NombreTarea", Name = "NombreTareaIndex")]
public partial class Tarea
{
    [Key]
    public int TareaId { get; set; }

    [Required]
    [StringLength(100)]
    public string NombreTarea { get; set; } = null!;

    // Relación con Persona (Responsable de la tarea)
    public int? ResponsableId { get; set; }  // Puede ser null si no hay responsable
    [ForeignKey("ResponsableId")]
    public virtual Persona? Responsable { get; set; }  // Puede ser nula, si no hay responsable

    [InverseProperty("Tarea")]
    public virtual ICollection<ActividadTareaPersona> ActividadTareaPersonas { get; set; } = new List<ActividadTareaPersona>();

    [InverseProperty("Tarea")]
    public virtual ICollection<TareaPersona> TareaPersonas { get; set; } = new List<TareaPersona>();
}

