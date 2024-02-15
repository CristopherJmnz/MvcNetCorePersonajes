using Microsoft.AspNetCore.Http.HttpResults;
using MvcNetCorePersonajes.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MvcNetCorePersonajes.Repositories
{
    #region PROCEDURES
//    create or replace procedure SP_INSERT_PERSONAJE
//(p_id personajes. idpersonaje%type, p_nombre personajes. personaje%type, p_imagen personajes.imagen%type)
//as
//begin
//  insert into personajes values(p_id, p_nombre, p_imagen);
//    commit;
//end SP_INSERT_PERSONAJE;
    #endregion
    public class PersonajeRepositoryOracle :IPersonajesRepository
    {
        private OracleConnection cn;
        private OracleCommand com;
        private DataTable tablaPersonajes;

        public PersonajeRepositoryOracle()
        {
            string connectionString = @"User Id=SYSTEM;Password=oracle;Data Source=LOCALHOST:1521/XE; Persist Security Info=True";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from personajes";
            this.tablaPersonajes = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(sql,this.cn);
            adapter.Fill(this.tablaPersonajes);
        }

        public void DeletePersonaje(int idPersonaje)
        {
            string sql = "delete from personajes where idPersonaje=:idpersonaje";
            OracleParameter pamId= new OracleParameter(":idPersonaje",idPersonaje);
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
            throw new NotImplementedException();
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
                    IdPersonaje = row.Field<int>("IDPERSONAJE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Nombre = row.Field<string>("PERSONAJE")
                };
                personajes.Add(pers);
            }
            return personajes;
        }

        public void InsertPersonaje(int idPersonaje, string nombre, string imagen)
        {
            OracleParameter pamId=new OracleParameter(":?",idPersonaje);
            this.com.Parameters.Add(pamId);
            OracleParameter pamNombre = new OracleParameter(":?", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":?", imagen);
            this.com.Parameters.Add(pamImagen);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_PERSONAJE";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void UpdatePersonaje(int idPersonaje, string nombre, string imagen)
        {
            string sql = "update personajes set personaje=:nombre,imagen=:imagen where idPersonaje=:id";
            OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamId=new OracleParameter(":id", idPersonaje);
            this.com.Parameters.Add(pamId);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
