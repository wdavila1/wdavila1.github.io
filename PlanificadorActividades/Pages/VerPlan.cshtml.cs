using PlanificadorActividades.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace PlanificadorActividades.Pages
{
    public class VerPlanModel : PageModel
    {
        private readonly PlanificadorActividadesContext _context;

        public VerPlanModel(PlanificadorActividadesContext context)
        {
            _context = context;
        }

        // Propiedades para la vista
        public Actividad Actividad { get; set; } = null!;
        public List<ActividadTareaPersona> ActividadTareas { get; set; } = new();
        public List<Tarea> TareasDisponibles { get; set; } = new();
        public List<Persona> PersonasDisponibles { get; set; } = new();
        [BindProperty]
        public string TareaNombre { get; set; } = string.Empty;
        public string MensajeError { get; set; } = string.Empty;

        // Método para cargar datos iniciales
        public void OnGet(int id)
        {
            // Cargar la actividad junto con sus tareas y responsables
            Actividad = _context.Actividades
                .Include(a => a.ActividadTareaPersonas)
                    .ThenInclude(atp => atp.Tarea)
                .Include(a => a.ActividadTareaPersonas)
                    .ThenInclude(atp => atp.Persona)
                .FirstOrDefault(a => a.ActividadId == id) 
                ?? throw new ArgumentException("Actividad no encontrada.");

            // Obtener las tareas asignadas a la actividad
            ActividadTareas = Actividad.ActividadTareaPersonas.ToList();

            // Obtener las tareas disponibles (no asignadas a esta actividad)
            var tareasAsignadasIds = ActividadTareas.Select(atp => atp.TareaId).ToList();
            TareasDisponibles = _context.Tareas
                .Where(t => !tareasAsignadasIds.Contains(t.TareaId))
                .OrderBy(t => t.NombreTarea)
                .ToList();

            // Obtener los responsables disponibles (no asignados a esta actividad)
            var personasAsignadasIds = ActividadTareas
                .Where(atp => atp.PersonaId.HasValue)
                .Select(atp => atp.PersonaId)
                .ToList();

            PersonasDisponibles = _context.Personas
                .Where(p => !personasAsignadasIds.Contains(p.PersonaId))
                .OrderBy(p => p.NombrePersona)
                .ToList();

                 ViewData["year"] = DateTime.Now.Year;
        }

        // Método para agregar una tarea a la actividad
        public IActionResult OnPostAgregarTarea(int id)
        {
            var tarea = _context.Tareas.FirstOrDefault(t => t.NombreTarea == TareaNombre);
            if (tarea == null)
            {
                MensajeError = "Tarea no encontrada.";
                OnGet(id);
                return Page();
            }

            // Validar que la tarea no esté ya asociada a la actividad
            if (_context.ActividadTareaPersonas.Any(atp => atp.ActividadId == id && atp.TareaId == tarea.TareaId))
            {
                MensajeError = "La tarea ya está asociada a esta actividad.";
                OnGet(id);
                return Page();
            }

            var nuevaRelacion = new ActividadTareaPersona
            {
                ActividadId = id,
                TareaId = tarea.TareaId
            };

            _context.ActividadTareaPersonas.Add(nuevaRelacion);
            _context.SaveChanges();

            return RedirectToPage(new { id });
        }

        public IActionResult OnPostAsignarResponsable(int actividadId, int tareaId)
{
    // Validar si la tarea existe en la actividad
    var actividadTarea = _context.ActividadTareaPersonas
        .Include(atp => atp.Persona)
        .Include(atp => atp.Tarea)
        .FirstOrDefault(atp => atp.ActividadId == actividadId && atp.TareaId == tareaId);

    if (actividadTarea == null)
    {
        MensajeError = "No se encontró la relación entre la tarea y la actividad.";
        OnGet(actividadId);
        return Page();
    }

    // Validar si la tarea ya tiene un responsable
    if (actividadTarea.PersonaId != null)
    {
        MensajeError = "La tarea ya tiene un responsable asignado.";
        OnGet(actividadId);
        return Page();
    }

    // Obtener responsables disponibles en memoria
    var responsablesDisponibles = _context.Personas
        .Where(p =>
            !_context.TareaPersonas.Any(tp => tp.PersonaId == p.PersonaId && tp.TareaId == tareaId) // Sin restricciones
            && !_context.ActividadTareaPersonas.Any(atp => atp.ActividadId == actividadId && atp.PersonaId == p.PersonaId)) // No asignado ya
        .ToList(); // Traer resultados a memoria

    if (!responsablesDisponibles.Any())
    {
        MensajeError = "No hay responsables disponibles para esta tarea (debido a restricciones o asignaciones existentes).";
        OnGet(actividadId);
        return Page();
    }

    // Selección aleatoria en memoria
    var responsableSeleccionado = responsablesDisponibles
        .OrderBy(r => Guid.NewGuid()) // Selección aleatoria en memoria
        .FirstOrDefault();

    // Asignar el responsable
    actividadTarea.PersonaId = responsableSeleccionado!.PersonaId;

    _context.SaveChanges();

    return RedirectToPage(new { id = actividadId });
}



        // Método para eliminar una tarea de la actividad
        public IActionResult OnPostEliminarTarea(int actividadId, int tareaId)
        {
            // Verificar si la relación existe
            var relacion = _context.ActividadTareaPersonas
                .FirstOrDefault(atp => atp.ActividadId == actividadId && atp.TareaId == tareaId);

            if (relacion == null)
            {
                MensajeError = "No se encontró la tarea en esta actividad.";
                return RedirectToPage(new { id = actividadId });
            }

            // Eliminar la relación
            _context.ActividadTareaPersonas.Remove(relacion);
            _context.SaveChanges();

            return RedirectToPage(new { id = actividadId });
        }
    }
}
