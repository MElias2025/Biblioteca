using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Biblioteca.Conexion;

namespace Biblioteca
{
    public partial class GestionarUsuarios : Form
    {
        private Conexion conexion; // Instancia de la clase Conexion
        private int idUsuarioSeleccionado = -1; // Para almacenar el ID del usuario seleccionado

        public GestionarUsuarios(Conexion conexion)
        {
            InitializeComponent();
            this.conexion = conexion;
            CargarUsuarios();
            this.FormClosed += new FormClosedEventHandler(FormUsuarios_FormClosed);
        }

        // Evento para cerrar la aplicación cuando el formulario de usuarios se cierre
        private void FormUsuarios_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Método para cargar usuarios en el DataGridView
        private void CargarUsuarios()
        {
            dataGridViewUsuarios.DataSource = null; // Limpia la fuente de datos actual
            dataGridViewUsuarios.DataSource = conexion.ObtenerUsuarios(); // Vuelve a cargar los datos
        }

        // Manejo de errores en el DataGridView
        private void dataGridViewUsuarios_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Se produjo un error al procesar los datos del DataGridView.");
        }

        // Botón para agregar un nuevo usuario
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string direccion = txtDireccion.Text;
            string telefono = txtTelefono.Text;
            string dui = txtDUI.Text;

            conexion.AgregarUsuario(nombre, direccion, telefono, dui);
            CargarUsuarios();
            LimpiarControles();
        }

        // Botón para modificar un usuario existente
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == -1)
            {
                MessageBox.Show("Debe seleccionar un usuario para modificar.");
                return;
            }

            string nuevoNombre = txtNombre.Text;
            string nuevaDireccion = txtDireccion.Text;
            string nuevoTelefono = txtTelefono.Text;
            string nuevoDUI = txtDUI.Text;

            conexion.ModificarUsuario(idUsuarioSeleccionado, nuevoNombre, nuevaDireccion, nuevoTelefono, nuevoDUI);
            CargarUsuarios();
            LimpiarControles();
        }

        // Botón para eliminar un usuario
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un usuario para eliminar.");
                return;
            }

            int rowIndex = dataGridViewUsuarios.SelectedRows[0].Index;
            if (rowIndex < 0 || rowIndex >= dataGridViewUsuarios.Rows.Count)
            {
                MessageBox.Show("Índice de fila no válido.");
                return;
            }

            // Obtener el ID del usuario a eliminar
            int idUsuario = (int)dataGridViewUsuarios.Rows[rowIndex].Cells["IdUsuario"].Value;

            // Llamar al método para eliminar el usuario
            conexion.EliminarUsuario(idUsuario);

            // Limpiar el DataGridView y recargar los datos
            CargarUsuarios();
            LimpiarControles();
        }

        // Método para limpiar los controles
        private void LimpiarControles()
        {
            txtIdUsuario.Clear();
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtDUI.Clear();
            idUsuarioSeleccionado = -1;
        }

        // Evento de selección de un usuario en el DataGridView
        private void dataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count > 0)
            {
                var row = dataGridViewUsuarios.SelectedRows[0];
                idUsuarioSeleccionado = Convert.ToInt32(row.Cells["IdUsuario"].Value);

                txtIdUsuario.Text = idUsuarioSeleccionado.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value != null ? row.Cells["Nombre"].Value.ToString() : string.Empty;
                txtDireccion.Text = row.Cells["Direccion"].Value != null ? row.Cells["Direccion"].Value.ToString() : string.Empty;
                txtTelefono.Text = row.Cells["Telefono"].Value != null ? row.Cells["Telefono"].Value.ToString() : string.Empty;
                txtDUI.Text = row.Cells["DUI"].Value != null ? row.Cells["DUI"].Value.ToString() : string.Empty;
            }
        }

        // Botón para limpiar los controles
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        // Botón para volver al menú principal
        private void btnMenu_Click(object sender, EventArgs e)
        {
            Menu men = new Menu(conexion); // Pasamos la conexión al formulario de menú
            men.Show();
            this.Hide(); // En lugar de cerrar, ocultamos el formulario actual
        }

        private void GestionarUsuarios_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                // Validar que el campo de texto no esté vacío
                if (string.IsNullOrEmpty(textbuscarus.Text))
                {
                    MessageBox.Show("Por favor, ingrese un ID de usuario para buscar.");
                    return;
                }

                // Intentar convertir el texto del campo de búsqueda a un número entero
                if (int.TryParse(textbuscarus.Text, out int idUsuario))
                {
                    // Buscar el usuario por su ID
                    Usuario usuario = conexion.ObtenerUsuarios().FirstOrDefault(u => u.IdUsuario == idUsuario);

                    if (usuario != null)
                    {
                        // Si se encuentra el usuario, mostrar los detalles en los controles del formulario
                        txtIdUsuario.Text = usuario.IdUsuario.ToString();
                        txtNombre.Text = usuario.Nombre;
                        txtDireccion.Text = usuario.Direccion;
                        txtTelefono.Text = usuario.Telefono;
                        txtDUI.Text = usuario.DUI;
                    }
                    else
                    {
                        // Si no se encuentra el usuario, mostrar un mensaje al usuario
                        MessageBox.Show("Usuario no encontrado.");
                    }
                }
                else
                {
                    MessageBox.Show("ID de usuario inválido. Debe ingresar un número entero.");
                }
            }
        }
    }
}
