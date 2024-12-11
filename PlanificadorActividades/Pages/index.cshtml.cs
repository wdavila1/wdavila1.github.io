using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlanificadorActividades.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanificadorActividades.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PlanificadorActividadesContext _context;

        public IndexModel(PlanificadorActividadesContext context)
        {
            _context = context;
        }

        public List<Actividad> ActividadesProximas { get; set; } = new List<Actividad>();


        public void OnGet()
        {
            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Obtener actividades dentro del rango de la fecha actual a 1 mes adelante
            ActividadesProximas = _context.Actividades
                .Where(a => a.Fecha >= fechaActual && a.Fecha <= fechaActual.AddMonths(1))
                .OrderBy(a => a.Fecha) // Ordenar por fecha, de mayor a menor
                .ToList();

                // Establecer la bandera en ViewData para indicar que estamos en la página de inicio
            ViewData["IsIndexPage"] = true;
            ViewData["year"] = DateTime.Now.Year;

        }

    }
}



