using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlanificadorActividades.EntityModels;

public partial class TareaPersona
{
    // No es necesario usar el atributo [Key] aquí
    public int TareaId { get; set; }
    public int PersonaId { get; set; }

    // Relaciones
    [ForeignKey("TareaId")]
    [InverseProperty("TareaPersonas")]
    public virtual Tarea Tarea { get; set; } = null!;

    [ForeignKey("PersonaId")]
    [InverseProperty("TareaPersonas")]
    public virtual Persona Persona { get; set; } = null!;
}
