using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PowerBI.Data;
using PowerBI.Models;

namespace PowerBI.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Agregar()
        {
            // Si necesitas cargar datos adicionales, puedes hacerlo aquí
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(User u)
        {
            if (ModelState.IsValid)
            {
                // Define los parámetros de salida
                var registradoParam = new MySqlParameter
                {
                    ParameterName = "@Registrado",
                    MySqlDbType = MySqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };
                var mensajeParam = new MySqlParameter
                {
                    ParameterName = "@Mensaje",
                    MySqlDbType = MySqlDbType.VarChar,
                    Size = 100,
                    Direction = System.Data.ParameterDirection.Output
                };

                // Ejecuta el procedimiento almacenado
                await dbContext.Database.ExecuteSqlRawAsync(
                    "CALL AddUser(@name, @lastname, @nickname, SHA2(@password, 256), @Registrado, @Mensaje)",
                    new MySqlParameter("@name", u.name),
                    new MySqlParameter("@lastname", u.lastname),
                    new MySqlParameter("@nickname", u.nickname),
                    new MySqlParameter("@password", u.password), // Contraseña se encripta dentro del procedimiento
                    registradoParam,
                    mensajeParam
                );

                // Verifica el resultado del registro
                bool registrado = Convert.ToBoolean(registradoParam.Value);
                string mensaje = mensajeParam.Value.ToString() ?? "Error desconocido";

                if (registrado)
                {
                    return RedirectToAction("Inicio");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, mensaje);
                    ViewData["Mensaje"] = mensaje;
                }
            }

            return View(u);
        }

        [HttpGet]
        public async Task<IActionResult> Actualizar(int id)
        {
            User? aux = await dbContext.Users.FindAsync(id);
            if (aux == null)
            {
                return NotFound();
            }

            return View(aux);
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(User u)
        {
            if (ModelState.IsValid)
            {
                // Verifica si la contraseña se cambió, encripta solo si es necesario
                var originalUser = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.idUser == u.idUser);
                if (originalUser != null && originalUser.password != u.password)
                {
                    u.password = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(u.password))).Replace("-", "").ToLower();
                }

                dbContext.Entry(u).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();

                return RedirectToAction("Inicio");
            }

            return View(u);
        }


    }
}