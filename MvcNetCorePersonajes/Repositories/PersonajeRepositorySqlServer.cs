using Microsoft.AspNetCore.Http.HttpResults;
using MvcNetCorePersonajes.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MvcNetCorePersonajes.Repositories
{
    #region PROCEDURES

//    create procedure sp_insert_personaje
//(@id int, @nombre nvarchar(50), @imagen nvarchar(500))
//    as
//    	insert into personajes values(@id, @nombre, @imagen);
//    go
    #endregion
    public class PersonajeRepositorySqlServer : IPersonajesRepository
    {
        private SqlConnection cn;
        private SqlCommand com;
        private DataTable tablaPersonajes;
        public PersonajeRepositorySqlServer()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=NetCoreP1;Persist Security Info=True;User ID=sa;Password=''";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            string sql = "Select * from personajes";
            SqlDataAdapter adapter = new SqlDataAdapter(sql,cn);
            this.tablaPersonajes = new DataTable();
            adapter.Fill(tablaPersonajes);
        }
   
        public void DeletePersonaje(int idPersonaje)
        {
            string sql = "delete from personajes where idPersonaje=@idpersonaje";
            SqlParameter pamId = new SqlParameter("@idPersonaje", idPersonaje);
            this.com.Parameters.Add(pamId);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Personaje FindPersonajeById(int idPersonaje)
        {
            var consulta = from datos in this.tablaPersonajes.AsEnumerable()
                           where datos.Field<int>("IDPERSONAJE")==idPersonaje
                           select datos;
            var row = consulta.First();
            Personaje personaje = new Personaje
            {
                IdPersonaje = row.Field<int>("IDPERSONAJE"),
                Imagen = row.Field<string>("IMAGEN"),
                Nombre = row.Field<string>("PERSONAJE")
            };
            return personaje;
        }

        public List<Personaje> getAllPersonajes()
        {
            var consulta = from datos in this.tablaPersonajes.AsEnumerable()
                           select datos;
            List<Personaje> personajes = new List<Personaje>();
            foreach (var row in consulta)
            {
                Personaje pers = new Personaje 
                {
                    IdPersonaje=row.Field<int>("IDPERSONAJE"),
                    Imagen=row.Field<string>("IMAGEN"),
                    Nombre=row.Field<string>("PERSONAJE")
                };
                personajes.Add(pers);
            }
            return personajes;
        }

        public void InsertPersonaje(int idPersonaje, string nombre, string imagen)
        {
            this.com.Parameters.AddWithValue("@id", idPersonaje);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_PERSONAJE";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void UpdatePersonaje(int idPersonaje, string nombre, string imagen)
        {
            string sql = "update personajes set personaje=@nombre,imagen=@imagen where idPersonaje=@id";
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@id", idPersonaje);
            this.com.CommandText = sql;
            this.com.CommandType= CommandType.Text;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
