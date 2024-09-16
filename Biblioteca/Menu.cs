using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteca
{
    public partial class Menu : Form
    {
        Conexion conexion;

        public Menu(Conexion conexion)
        {
            InitializeComponent();
            this.conexion = conexion;
            // Suscribirse al evento FormClosed
            this.FormClosed += new FormClosedEventHandler(Menu_FormClosed);

        }

        // Evento para cerrar la aplicación cuando el formulario de préstamos se cierre
        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
         

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 prestamo = new Form1(conexion);
            prestamo.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GestionarLibros libros = new GestionarLibros(conexion);
            libros.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           GestionarUsuarios usuarios = new GestionarUsuarios(conexion);
            usuarios.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Confirmar si el usuario realmente desea salir
            var result = MessageBox.Show("¿Estás seguro de que deseas salir?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit(); // Cierra la aplicación
            }
        }

        private void button5_Click(object sender, EventArgs e)

        {
            // Ejemplo en algún método
                Conexion miConexion = new Conexion(); // Asegúrate de inicializar correctamente tu objeto Conexion
                Grafico grafico = new Grafico(miConexion);
                grafico.Show();

        }
    }
}
