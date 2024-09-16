using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteca
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          

            Conexion conexion = new Conexion(); // Crear una instancia de la clase Conexion
                                                //  Application.Run(new Form1(conexion)); // Pasar la instancia de Conexion al constructor de Form1
            Application.Run(new loguin(conexion));
        }
    }
}
