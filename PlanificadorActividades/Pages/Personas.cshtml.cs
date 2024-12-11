using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlanificadorActividades.EntityModels;

namespace PlanificadorActividades.Pages;

public class PersonasModel : PageModel
{
    private readonly PlanificadorActividadesContext _context;

    public PersonasModel(PlanificadorActividadesContext context)
    {
        _context = context;
    }

    public List<Persona> Personas { get; set; } = new();

    public Dictionary<int, List<Tarea>> Restricciones { get; set; } = new();

    [BindProperty]
    public string NombrePersona { get; set; } = string.Empty;

    public string MensajeError { get; set; } = string.Empty;

    public void OnGet()
    {
        Personas = _context.Personas
            .OrderBy(p => p.NombrePersona)
            .ToList();

        Restricciones = _context.Personas
            .ToDictionary(
                p => p.PersonaId,
                p => _context.TareaPersonas
                    .Where(tp => tp.PersonaId == p.PersonaId)
                    .Select(tp => tp.Tarea)
                    .ToList()
            );

             ViewData["year"] = DateTime.Now.Year;
    }

    public IActionResult OnPostAgregarPersona()
    {
        if (string.IsNullOrWhiteSpace(NombrePersona))
        {
            MensajeError = "El nombre de la persona no puede estar vacío.";
            OnGet();
            return Page();
        }

        if (_context.Personas.Any(p => p.NombrePersona == NombrePersona))
        {
            MensajeError = "Ya existe una persona con ese nombre.";
            OnGet();
            return Page();
        }

        _context.Personas.Add(new Persona { NombrePersona = NombrePersona });
        _context.SaveChanges();

        return RedirectToPage("Personas");
    }

    public IActionResult OnPostEliminarPersona(int id)
    {
        var persona = _context.Personas.Find(id);
        if (persona == null)
        {
            MensajeError = "La persona no existe.";
            OnGet();
            return Page();
        }

        if (_context.ActividadTareaPersonas.Any(atp => atp.PersonaId == id))
        {
            MensajeError = "No se puede eliminar la persona porque está asignada como responsable en actividades.";
            OnGet();
            return Page();
        }

        _context.Personas.Remove(persona);
        _context.SaveChanges();

        return RedirectToPage("Personas");
    }

    public IActionResult OnGetAñadirRestriccion(int id)
    {
        return RedirectToPage("Restricciones", new { id });
    }
}
