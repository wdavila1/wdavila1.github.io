using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlanificadorActividades.EntityModels;
using System.Linq;

namespace PlanificadorActividades.Pages;

    public class RestriccionesModel : PageModel
    {
        private readonly PlanificadorActividadesContext _context;

        public RestriccionesModel(PlanificadorActividadesContext context)
        {
            _context = context;
        }

        public Persona Persona { get; set; } = null!;
        public List<Tarea> TareasRestringidas { get; set; } = new();
        public List<Tarea> TareasDisponibles { get; set; } = new();
        public string MensajeError { get; set; } = string.Empty;

        [BindProperty]
        public int TareaId { get; set; }

        public void OnGet(int id)
        {
            // Cargar la persona
            Persona = _context.Personas.Find(id) ?? throw new ArgumentException("Persona no encontrada.");

            // Obtener tareas restringidas
            TareasRestringidas = _context.TareaPersonas
                .Where(tp => tp.PersonaId == id)
                .Select(tp => tp.Tarea)
                .ToList();

            // Obtener tareas disponibles
            var tareasRestringidasIds = TareasRestringidas.Select(tr => tr.TareaId).ToList();
            TareasDisponibles = _context.Tareas
                .Where(t => !tareasRestringidasIds.Contains(t.TareaId))
                .OrderBy(t => t.NombreTarea)
                .ToList();

                 ViewData["year"] = DateTime.Now.Year;
        }

       public IActionResult OnPostAgregarRestriccion(int id)
{
    if (_context.TareaPersonas.Any(tp => tp.PersonaId == id && tp.TareaId == TareaId))
    {
        MensajeError = "La tarea ya está restringida para esta persona.";
        OnGet(id); // Recargar datos para mostrar en la página.
        return Page();
    }

    _context.TareaPersonas.Add(new TareaPersona { PersonaId = id, TareaId = TareaId });
    _context.SaveChanges();
    return RedirectToPage("/Restricciones", new { id }); // Redirigir a la misma página con el parámetro id.
}


       public IActionResult OnPostEliminarRestriccion(int id, int tareaId)
{
    var restriccion = _context.TareaPersonas
        .FirstOrDefault(tp => tp.PersonaId == id && tp.TareaId == tareaId);

    if (restriccion == null)
    {
        MensajeError = "La restricción no existe.";
        OnGet(id); // Recargar datos para mostrar en la página.
        return Page();
    }

    _context.TareaPersonas.Remove(restriccion);
    _context.SaveChanges();
    return RedirectToPage("/Restricciones", new { id }); // Redirigir a la misma página con el parámetro id.
}


    }

