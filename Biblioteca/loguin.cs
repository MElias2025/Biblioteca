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
    public partial class loguin : Form
    {
        Conexion conexion = new Conexion();
        public loguin(Conexion conexion)
        {

            InitializeComponent();
            this.conexion = conexion;
            this.FormClosed += new FormClosedEventHandler(loguin_FormClosed);
        }

        private void loguin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se hace clic en el PictureBox
            
        }
        private void btnAcceder_Click(object sender, EventArgs e)
        {

            // Obtener los valores de los TextBox
            string usuario = txtusuario.Text;
            string contraseña = txtcontra.Text;

            // Validar las credenciales utilizando el método IniciarSesion
            if (conexion.IniciarSesion(usuario, contraseña))
            {
                // Si las credenciales son correctas, abrir el formulario Menu
                Menu menu = new Menu(conexion);
                menu.Show();

                // Ocultar el formulario de inicio de sesión
                this.Hide();
            }
            else
            {
                // Si las credenciales son incorrectas, mostrar un mensaje de error
                MessageBox.Show("Usuario o contraseña incorrectos", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtcontra_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
