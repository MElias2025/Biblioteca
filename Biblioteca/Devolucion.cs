using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Biblioteca
{
    public partial class Devolucion : Form
    {
        private Conexion conexion;

        public Devolucion(Conexion conexion)
        {
            InitializeComponent();
            this.conexion = conexion; // Asignar la conexión recibida
            CargarUsuarios();
            // Suscribirse al evento FormClosed
            this.FormClosed += new FormClosedEventHandler(Devolucion_FormClosed);
        }

        // Evento para cerrar la aplicación cuando el formulario de devoluciones se cierre
        private void Devolucion_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Cargar usuarios en el ComboBox
        private void CargarUsuarios()
        {
            List<Conexion.Usuario> usuarios = conexion.ObtenerUsuarios();
            comboBoxUsuarios.DataSource = usuarios;
            comboBoxUsuarios.DisplayMember = "Nombre";
            comboBoxUsuarios.ValueMember = "IdUsuario";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (comboBoxUsuarios.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un usuario.");
                return;
            }

            int idUsuario = (int)comboBoxUsuarios.SelectedValue;
            CargarPrestamosPendientes(idUsuario);
        }

        // Cargar los préstamos pendientes del usuario
        private void CargarPrestamosPendientes(int idUsuario)
        {
            Conexion.Prestamo prestamoPendiente = conexion.ObtenerPrestamoPendiente(idUsuario);

            if (prestamoPendiente != null)
            {
                txtFechaPrestamo.Text = prestamoPendiente.FechaPrestamo.ToString("dd/MM/yyyy");
                txtFechaDevolucion.Text = prestamoPendiente.FechaDevolucion.ToString("dd/MM/yyyy");

                CargarLibros(prestamoPendiente.LibrosPrestados);
                btnDevolver.Visible = true;
            }
            else
            {
                MessageBox.Show("No se encontraron préstamos pendientes para el usuario seleccionado.");
                txtFechaPrestamo.Clear();
                txtFechaDevolucion.Clear();
                dataGridViewLibros.Rows.Clear();
                btnDevolver.Visible = false;
            }
        }

        // Cargar los libros asociados al préstamo en el DataGridView
        private void CargarLibros(List<int> idLibros)
        {
            dataGridViewLibros.Rows.Clear();
            foreach (int idLibro in idLibros)
            {
                Conexion.Libro libro = conexion.ObtenerLibroPorId(idLibro);
                if (libro != null)
                {
                    dataGridViewLibros.Rows.Add(libro.IdLibro, libro.Titulo);
                }
            }
        }

        // Registrar la devolución del préstamo
        private void btnDevolver_Click(object sender, EventArgs e)
        {
            if (comboBoxUsuarios.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un usuario.");
                return;
            }

            if (dataGridViewLibros.Rows.Count == 0)
            {
                MessageBox.Show("No hay libros para devolver.");
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro de que desea devolver el préstamo?", "Confirmar Devolución", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int idUsuario = (int)comboBoxUsuarios.SelectedValue;
                DevolverPrestamo(idUsuario);
            }
        }

        // Devolver el préstamo
        private void DevolverPrestamo(int idUsuario)
        {
            bool exito = conexion.DevolverPrestamo(idUsuario);

            if (exito)
            {
                MessageBox.Show("Préstamo devuelto exitosamente.");
                ReiniciarFormulario();
            }
            else
            {
                MessageBox.Show("Error al devolver el préstamo.");
            }
        }

        // Reiniciar el formulario
        private void ReiniciarFormulario()
        {
            comboBoxUsuarios.SelectedIndex = -1;
            txtFechaPrestamo.Clear();
            txtFechaDevolucion.Clear();
            dataGridViewLibros.Rows.Clear();
            CargarUsuarios();
        }

        private void Devolucion_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 pres = new Form1(conexion); // Pasamos la conexión al formulario de préstamos
            pres.Show();
            this.Hide(); // En lugar de cerrar, ocultamos el formulario actual
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Menu men = new Menu(conexion); // Pasamos la conexión al formulario de préstamos
            men.Show();
            this.Hide(); // En lugar de cerrar, ocultamos el formulario actual
        }
    }
}
