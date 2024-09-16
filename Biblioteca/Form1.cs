using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Biblioteca
{
    public partial class Form1 : Form
    {
        // Instancia de la clase Conexion
        Conexion conexion = new Conexion();
        List<int> detallesPrestamo = new List<int>(); // Lista para almacenar los IDs de los libros en el préstamo

        public Form1(Conexion conexion)
        {
            InitializeComponent();
            this.conexion = conexion; // Asignamos la conexión recibida
            // Suscribirse al evento FormClosed
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

        }

        // Evento para cerrar la aplicación cuando el formulario de préstamos se cierre
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Cargar usuarios y libros cuando el formulario se carga
            CargarUsuarios();
            CargarLibrosDisponibles();
        }

        // Método para cargar los usuarios en el ComboBox
        private void CargarUsuarios()
        {
            // Obtener la lista de usuarios
            List<Conexion.Usuario> usuarios = conexion.ObtenerUsuarios();

            if (usuarios.Count > 0)
            {
                comboBoxUsuarios.DataSource = usuarios;
                comboBoxUsuarios.DisplayMember = "Nombre";
                comboBoxUsuarios.ValueMember = "IdUsuario";
            }
            else
            {
                MessageBox.Show("No hay usuarios registrados.");
                comboBoxUsuarios.DataSource = null; // Asegurarse de que el ComboBox no tenga datos si la lista está vacía
            }
        }

        // Método para cargar los libros disponibles en el ComboBox
        private void CargarLibrosDisponibles()
        {
            // Obtener la lista de libros disponibles
            List<Conexion.Libro> librosDisponibles = conexion.ObtenerLibrosDisponibles();

            if (librosDisponibles.Count > 0)
            {
                comboBoxLibros.DataSource = librosDisponibles;
                comboBoxLibros.DisplayMember = "Titulo";
                comboBoxLibros.ValueMember = "IdLibro";
            }
            else
            {
                MessageBox.Show("No hay libros disponibles.");
                comboBoxLibros.DataSource = null; // Asegurarse de que el ComboBox no tenga datos si la lista está vacía
            }
        }

        // Método para reiniciar el formulario después de un préstamo
        private void ReiniciarFormulario()
        {
            // Limpiar las selecciones y restablecer el formulario a su estado inicial
            comboBoxUsuarios.SelectedIndex = -1;
            comboBoxLibros.SelectedIndex = -1;
            dateTimePickerFechaPrestamo.Value = DateTime.Now;
            dateTimePickerFechaDevolucion.Value = DateTime.Now.AddDays(7);
            comboBoxUsuarios.Enabled = true;
            listBoxDetallesPrestamo.Items.Clear();
            detallesPrestamo.Clear();

            // Volver a cargar los datos
            CargarUsuarios();
            CargarLibrosDisponibles();
        }

        // Método para agregar un libro al préstamo
        private void btnAgregarLibro_Click(object sender, EventArgs e)
        {
            if (comboBoxLibros.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un libro válido.");
                return;
            }

            // Obtener el ID del libro seleccionado
            int idLibro = (int)comboBoxLibros.SelectedValue;

            // Verificar si el libro ya ha sido agregado
            if (detallesPrestamo.Contains(idLibro))
            {
                MessageBox.Show("Este libro ya ha sido agregado al préstamo.");
                return;
            }

            // Agregar el ID del libro a la lista de detalles del préstamo
            detallesPrestamo.Add(idLibro);

            // Mostrar el título del libro en el ListBox
            listBoxDetallesPrestamo.Items.Add(comboBoxLibros.Text);

            // Deshabilitar el ComboBox de usuarios una vez agregado un libro
            comboBoxUsuarios.Enabled = false;
        }

        // Método para registrar el préstamo
        private void btnRegistrarPrestamo_Click(object sender, EventArgs e)
        {
            if (comboBoxUsuarios.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un usuario válido.");
                return;
            }

            int idUsuario = (int)comboBoxUsuarios.SelectedValue;

            if (detallesPrestamo.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un libro antes de registrar el préstamo.");
                return;
            }

            // Verificar si el usuario tiene un préstamo pendiente
            if (conexion.TienePrestamoPendiente(idUsuario))
            {
                MessageBox.Show("El usuario tiene un préstamo pendiente. Debe devolver el préstamo anterior antes de realizar uno nuevo.");
                ReiniciarFormulario();
                return;
            }

            // Obtener las fechas de préstamo y devolución
            DateTime fechaPrestamo = dateTimePickerFechaPrestamo.Value;
            DateTime fechaDevolucion = dateTimePickerFechaDevolucion.Value;

            // Registrar el préstamo en el sistema
            if (conexion.RegistrarPrestamo(idUsuario, fechaPrestamo, fechaDevolucion, detallesPrestamo))
            {
                MessageBox.Show("Préstamo registrado exitosamente.");
                ReiniciarFormulario();
            }
            else
            {
                MessageBox.Show("Error al registrar el préstamo.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Devolucion dev = new Devolucion(conexion); // Pasamos la conexión al formulario de devoluciones
            dev.Show(); // Muestra el formulario de devoluciones
            this.Hide(); // En lugar de cerrar, ocultamos el formulario actual
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Menu men = new Menu(conexion); // Pasamos la conexión al formulario de préstamos
            men.Show();
            this.Hide(); // En lugar de cerrar, ocultamos el formulario actual
        }

        private void listBoxDetallesPrestamo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
