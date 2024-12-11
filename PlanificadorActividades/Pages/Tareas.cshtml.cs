using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlanificadorActividades.EntityModels;

namespace PlanificadorActividades.Pages;

public class TareasModel : PageModel
{
    private PlanificadorActividadesContext _context;

    public TareasModel(PlanificadorActividadesContext context)
    {
        _context = context;
    }

    // Lista de tareas para la tabla
    public List<Tarea> Tareas { get; set; } = new();

    // Propiedad para enlazar el nombre de la tarea del formulario
    [BindProperty]
    public string NombreTarea { get; set; } = string.Empty;

    // Mensaje de error para mostrar en la página
    public string MensajeError { get; set; } = string.Empty;

    // Método para cargar tareas ordenadas alfabéticamente
    public void OnGet()
    {
        Tareas = _context.Tareas
            .OrderBy(t => t.NombreTarea)
            .ToList();

             ViewData["year"] = DateTime.Now.Year;
    }

    // Método para agregar una nueva tarea
    public IActionResult OnPostAgregarTarea()
{
    Console.WriteLine($"Intentando agregar tarea: {NombreTarea}");

    if (string.IsNullOrWhiteSpace(NombreTarea))
    {
        MensajeError = "El nombre de la tarea no puede estar vacío.";
        OnGet();
        return Page();
    }

    if (_context.Tareas.Any(t => t.NombreTarea == NombreTarea))
    {
        MensajeError = "Ya existe una tarea con ese nombre.";
        OnGet();
        return Page();
    }

    _context.Tareas.Add(new Tarea { NombreTarea = NombreTarea });
    _context.SaveChanges();

    Console.WriteLine($"Tarea agregada: {NombreTarea}");
    return RedirectToPage("Tareas");
}



    // Método para eliminar una tarea
   public IActionResult OnPostEliminarTarea(int id)
{
    Console.WriteLine($"Intentando eliminar tarea con ID: {id}");

    if (id <= 0)
    {
        MensajeError = "El ID de la tarea no es válido.";
        OnGet();
        return Page();
    }

    var tarea = _context.Tareas.Find(id);
    if (tarea == null)
    {
        MensajeError = "La tarea no existe.";
        OnGet();
        return Page();
    }

    if (_context.ActividadTareaPersonas.Any(atp => atp.TareaId == id))
    {
        MensajeError = "No se puede eliminar la tarea porque está asociada a una actividad.";
        OnGet();
        return Page();
    }

    _context.Tareas.Remove(tarea);
    _context.SaveChanges();

    Console.WriteLine($"Tarea eliminada con ID: {id}");
    return RedirectToPage("Tareas");
}



}


