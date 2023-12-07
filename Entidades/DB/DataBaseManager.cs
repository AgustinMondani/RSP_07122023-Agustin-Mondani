using System.Data.SqlClient;
using System.Linq.Expressions;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static  SqlConnection connection;
        private static string stringConnection;

        static DataBaseManager()
        {
            //Modificar el nombre del server para que funcione
            DataBaseManager.stringConnection = "Server = DESKTOP-NP3CB0L\\MSSQLSERVER777; Database = 20230622SP; Trusted_Connection = True";
        }

        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (connection = new SqlConnection(stringConnection))
                {
                    string query = $"SELECT * FROM comidas WHERE tipo_comida = @tipo";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@tipo", tipo);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        throw new ComidaInvalidaExeption("Comida no encontrada");
                    }
                    if (reader.Read())

                        return reader.GetString(2);
                }
                throw new ComidaInvalidaExeption("Comida no encontrada");

            }catch (ComidaInvalidaExeption ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", false);
                throw ex;
            }
            //catch (Exception)
            //{
            //    string mensajeError = "Error al leer de la base de datos";
            //    FileManager.Guardar(mensajeError, "logs.txt", false);
            //    throw new DataBaseManagerException(mensajeError);
            //}
        }

        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
        {
            try
            {
                using (connection = new SqlConnection(stringConnection))
                {
                    
                    string query = "INSERT INTO tickets (empleado, ticket) VALUES (@empleado, @ticket)";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@empleado", nombreEmpleado);
                    cmd.Parameters.AddWithValue("@ticket", comida.Ticket);

                    connection.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    return filasAfectadas > 0;
                }

                }catch (Exception ex)
                {
                    string mensajeError = "Error al escribir En la base de datos";
                    FileManager.Guardar(mensajeError, "logs.txt", true);
                    throw new DataBaseManagerException(mensajeError, ex);
                }

        }
    }
}
