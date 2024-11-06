using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PowerBI.Data;

namespace PowerBI.Controllers
{
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AccessController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Método para mostrar la vista de login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string nickname, string password)
        {
            if (ModelState.IsValid)
            {
                var resultado = await dbContext.Users
                    .FromSqlRaw("CALL ValidateUserLogin({0}, {1})", nickname, password)
                    .ToListAsync();

                bool usuarioValido = resultado.Any();

                if (usuarioValido)
                {
                    HttpContext.Session.SetString("Nickname", nickname);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nickname o contraseña incorrectos.");
                }
            }

            return View();
        }



        // Método para cerrar sesión
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}