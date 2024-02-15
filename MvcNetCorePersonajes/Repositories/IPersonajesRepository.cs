using MvcNetCorePersonajes.Models;

namespace MvcNetCorePersonajes.Repositories
{
    public interface IPersonajesRepository
    {
        List<Personaje> getAllPersonajes();
        Personaje FindPersonajeById(int idPersonaje);
        void InsertPersonaje(int idPersonaje,string nombre,string imagen);
        void UpdatePersonaje(int idPersonaje, string nombre, string imagen);
        void DeletePersonaje(int idPersonaje);
    }
}
