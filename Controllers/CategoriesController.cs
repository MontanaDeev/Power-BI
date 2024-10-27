using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PowerBI.Data;
using PowerBI.Models;

namespace PowerBI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Filtrar categorías con al menos un reporte
            var categories = await dbContext.Categories
                .Include(c => c.Reports)
                .Where(c => c.Reports.Any())
                .ToListAsync();

            // Obtener los reportes sin categoría
            var reportsWithoutCategory = await dbContext.Reports
                .Where(r => r.idCategory == null)
                .ToListAsync();

            ViewBag.ReportsWithoutCategory = reportsWithoutCategory;
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var category = await dbContext.Categories.Include(c => c.Reports).FirstOrDefaultAsync(c => c.idCategory == id);
            if (category == null)
            {
                return NotFound("La categoría no existe.");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.Categories.Add(category);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("La categoría no existe.");
            }

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}