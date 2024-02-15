using Microsoft.AspNetCore.Mvc;
using MvcNetCorePersonajes.Models;
using MvcNetCorePersonajes.Repositories;

namespace MvcNetCorePersonajes.Controllers
{
    public class PersonajesController : Controller
    {
        private IPersonajesRepository repo;
        public PersonajesController(IPersonajesRepository repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Personaje> personajes = this.repo.getAllPersonajes();
            return View(personajes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Personaje personaje)
        {
            this.repo.InsertPersonaje(personaje.IdPersonaje,personaje.Nombre,personaje.Imagen);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int idPersonaje)
        {
            this.repo.DeletePersonaje(idPersonaje);
            return RedirectToAction("Index");
        }
    }
}
