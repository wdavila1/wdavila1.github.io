using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlanificadorActividades.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanificadorActividades.Pages
{
    public class ActividadesModel : PageModel
    {
        private readonly PlanificadorActividadesContext _context;

        public ActividadesModel(PlanificadorActividadesContext context)
        {
            _context = context;
        }

        public List<Actividad> Actividades { get; set; } = new List<Actividad>();

        [BindProperty]
        public DateTime? FechaFiltro { get; set; } // Fecha opcional

        public void OnGet()
        {
            // Carga inicial de actividades sin filtrar
            CargarActividades(null);
             ViewData["year"] = DateTime.Now.Year;
        }

        public IActionResult OnPostFiltrarActividades()
{
    // Verificar si la fecha de filtro no es nula
    if (FechaFiltro.HasValue)
    {
        // Eliminar la parte de la hora para comparar solo la fecha (sin hora)
        var fechaFiltroSinHora = FechaFiltro.Value.Date;  // Usamos Value para acceder a la fecha del Nullable

        // Filtra actividades que sean iguales a la fecha seleccionada o posteriores
        Actividades = _context.Actividades
            .Where(a => a.Fecha.Date >= fechaFiltroSinHora)  // Comparar solo la fecha (sin hora)
            .OrderByDescending(a => a.Fecha)  // Ordenar de menor a mayor fecha
            .ToList();
    }
    else
    {
        // Si no se seleccionó fecha, mostrar todas las actividades
        Actividades = _context.Actividades
            .OrderByDescending(a => a.Fecha)
            .ToList();
    }

    return Page();
}




        public IActionResult OnPostAgregarActividad(string NombreActividad, DateTime FechaActividad)
        {
            if (!string.IsNullOrWhiteSpace(NombreActividad) && FechaActividad != default)
            {
                var nuevaActividad = new Actividad
                {
                    NombreActividad = NombreActividad,
                    Fecha = FechaActividad
                };

                _context.Actividades.Add(nuevaActividad);
                _context.SaveChanges();
            }

            return RedirectToPage(); // Redirige para recargar la página
        }

        public IActionResult OnPostEliminarActividad(int actividadId)
        {
            var actividad = _context.Actividades.Find(actividadId);
            if (actividad != null)
            {
                _context.Actividades.Remove(actividad);
                _context.SaveChanges();
            }

            return RedirectToPage(); // Redirige para recargar la página
        }

        private void CargarActividades(DateTime? filtroFecha)
{
    // Consultar actividades con o sin filtro
    var query = _context.Actividades.AsQueryable();

    if (filtroFecha.HasValue)
    {
        // Filtrar por la fecha seleccionada o posterior
        query = query.Where(a => a.Fecha >= filtroFecha.Value);
    }

    // Ordenar de mayor a menor fecha
    Actividades = query.OrderByDescending(a => a.Fecha).ToList();
}

    }
}



