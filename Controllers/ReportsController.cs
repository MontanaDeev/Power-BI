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
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ReportsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reports = await dbContext.Reports
                .Include(r => r.Category)
                .ToListAsync();

            return View(reports);
        }

        [HttpGet]
        public async Task<IActionResult> AddReport()
        {
            var categories = await dbContext.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddReport([FromForm] ReportRequest request)
        {
            Category? category = null;
            if (request.CategoryId.HasValue)
            {
                category = await dbContext.Categories.FindAsync(request.CategoryId.Value);
                if (category == null)
                {
                    return BadRequest("La categoría especificada no existe.");
                }
            }

            if (string.IsNullOrEmpty(request.PublicLink) && request.File == null)
            {
                return BadRequest("Debe proporcionar una URL o subir un archivo");
            }
            string? filePath = null;

            if (request.File != null && request.File.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
                filePath = Path.Combine("uploads", fileName);
                if (!Directory.Exists("uploads"))
                {
                    Directory.CreateDirectory("uploads");
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }
            }


            var report = new Report
            {
                Name = request.Name,
                idCategory = request.CategoryId,
                publicLink = request.PublicLink ?? filePath
            };

            dbContext.Reports.Add(report);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetReportsByCategory(int categoryId)
        {
            var reports = await dbContext.Reports
                .Where(r => r.idCategory == categoryId)
                .ToListAsync();

            return Ok(reports);
        }

        [HttpGet]
        public async Task<IActionResult> ViewReport(int id)
        {
            var report = await dbContext.Reports.FindAsync(id);
            if (report == null || (string.IsNullOrEmpty(report.publicLink) && string.IsNullOrEmpty(report.FilePath)))
            {
                return NotFound("El reporte no está disponible.");
            }

            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = await dbContext.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound("El reporte no existe.");
            }

            dbContext.Reports.Remove(report);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index"); 
        }
    }
}
