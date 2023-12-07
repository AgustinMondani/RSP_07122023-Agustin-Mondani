using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{
    public static class FileManager
    {
        private static string path;

        /// <summary>
        /// Es el path a la carpeta 20231207_Agustin.Mondani.2C en el escritorio
        /// </summary>
        static FileManager()
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(path, "RSP_07122023_MondaniAgustin.2C");

            ValidaExitenciaDeDirectorio();
        }

        private static void ValidaExitenciaDeDirectorio()
        {
            try
            {
                if (!Directory.Exists(path)) 
                {
                    Directory.CreateDirectory(path);
                }
                FileInfo fileInfo = new FileInfo(path);
            }catch (Exception ex)
            {
                string mensajeError = "Error en el directorio";
                FileManager.Guardar(mensajeError, "logs.txt", true);
                throw new FileManagerException(mensajeError, ex);
            }
        }

        /// <summary>
        /// Guarda la data en un archivo
        /// </summary>
        /// <param name="data">Informacion a guardar</param>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <param name="append">Anexar</param>
        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                string archivoPath = Path.Combine(path, nombreArchivo);
                using(StreamWriter streamWriter = new StreamWriter(archivoPath,append))
                {
                    streamWriter.WriteLine(data);
                }

            }catch(Exception ex)
            {
                string mensajeError = "Error ruta";
                FileManager.Guardar(mensajeError, "logs.txt", true);
                throw new FileManagerException(mensajeError, ex);
            }
        }

        public static bool Serializar<T>(T elemento, string nombreArchivo) where T : class
        {
            try
            {
                string json = JsonSerializer.Serialize(elemento);
                Guardar(json, nombreArchivo, false);
                return true;
            }catch (Exception ex)
            {
                string mensajeError = "No se pudo realiazar la serialización";
                FileManager.Guardar(mensajeError, "logs.txt", true);
                throw new FileManagerException(mensajeError, ex);
            }
            
        }
    }
}
