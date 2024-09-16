using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Biblioteca.Conexion;

namespace Biblioteca
{
    public partial class GestionarLibros : Form
    {
        private Conexion conexion; // Instancia de la clase Conexion
        private int idLibroSeleccionado = -1; // Para almacenar el ID del libro seleccionado


        public GestionarLibros(Conexion conexion)
        {
            InitializeComponent();
            this.conexion = conexion;
            CargarLibros();
            this.FormClosed += new FormClosedEventHandler(FormLibros_FormClosed);


            this.conexion = conexion ?? throw new ArgumentNullException(nameof(conexion), "La instancia de Conexion no puede ser nula.");

        }

        // Evento para cerrar la aplicación cuando el formulario de préstamos se cierre
        private void FormLibros_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void CargarLibros()
        {
           

            dataGridViewLibros.DataSource = null; // Limpia la fuente de datos actual
            dataGridViewLibros.DataSource = conexion.ObtenerLibrosDisponibles(); // Vuelve a cargar los datos

            // Configurar la columna 'Disponible' para que no sea editable
            if (dataGridViewLibros.Columns["Disponible"] != null)
            {
                dataGridViewLibros.Columns["Disponible"].ReadOnly = true;
            }
        }

        private void dataGridViewLibros_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Se produjo un error al procesar los datos del DataGridView.");
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string titulo = txtTitulo.Text;
            string autor = txtAutor.Text;
            string isbn = txtISBN.Text;

            conexion.AgregarLibro(titulo, autor, isbn);
            CargarLibros();
            LimpiarControles();
            ActualizarGrafico();
        }

        private void GestionarLibros_Load(object sender, EventArgs e)
        {

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idLibroSeleccionado == -1)
            {
                MessageBox.Show("Debe seleccionar un libro para modificar.");
                return;
            }

            string nuevoTitulo = txtTitulo.Text;
            string nuevoAutor = txtAutor.Text;
            string nuevoISBN = txtISBN.Text;

            conexion.ModificarLibro(idLibroSeleccionado, nuevoTitulo, nuevoAutor, nuevoISBN);
            CargarLibros();
            LimpiarControles();
        }



        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewLibros.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un libro para eliminar.");
                return;
            }

            int rowIndex = dataGridViewLibros.SelectedRows[0].Index;
            if (rowIndex < 0 || rowIndex >= dataGridViewLibros.Rows.Count)
            {
                MessageBox.Show("Índice de fila no válido.");
                return;
            }

            // Obtener el ID del libro a eliminar
            int idLibro = (int)dataGridViewLibros.Rows[rowIndex].Cells["IdLibro"].Value;

            // Llamar al método para eliminar el libro
            conexion.EliminarLibro(idLibro);

            // Limpiar el DataGridView y recargar los datos
            CargarLibros();
            LimpiarControles();
        }

        private void LimpiarControles()
        {
            txtIdLibro.Clear();
            txtTitulo.Clear();
            txtAutor.Clear();
            txtISBN.Clear();
            idLibroSeleccionado = -1;
        }

        private void dataGridViewLibros_SelectionChanged(object sender, EventArgs e)
        {
           
                if (dataGridViewLibros.SelectedRows.Count > 0)
                {
                    var row = dataGridViewLibros.SelectedRows[0];
                    idLibroSeleccionado = Convert.ToInt32(row.Cells["IdLibro"].Value);
                    txtIdLibro.Text = idLibroSeleccionado.ToString();
                    txtTitulo.Text = row.Cells["Titulo"].Value.ToString();
                    txtAutor.Text = row.Cells["Autor"].Value.ToString();
                    txtISBN.Text = row.Cells["ISBN"].Value.ToString();
                    // No se modifica la disponibilidad
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Menu men = new Menu(conexion); // Pasamos la conexión al formulario de préstamos
            men.Show();
            this.Hide(); // En lugar de cerrar, ocultamos el formulario actual
        }

        private void dataGridViewLibros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        //Iniciando con la  grafica
        private void chart1_Click(object sender, EventArgs e)
        {
            ActualizarGrafico();
        }

        private void Grafico_Load(object sender, EventArgs e)
        {
            // Actualizar el gráfico al cargar el formulario
            ActualizarGrafico();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // Solo actualiza el gráfico
            ActualizarGrafico();
        }

        private void ActualizarGrafico()
        {

            try
            {
                // Obtener la lista de libros y la de préstamos
                var libros = conexion.ObtenerLibros();
                var prestamos = conexion.ObtenerPrestamos();

                if (libros == null || libros.Count == 0)
                {
                    MessageBox.Show("No se encontraron libros.");
                    return;
                }

                if (prestamos == null || prestamos.Count == 0)
                {
                    MessageBox.Show("No se encontraron préstamos.");
                    return;
                }

                // Datos para libros por autor
                var autorLibroCount = libros.GroupBy(l => l.Autor)
                                            .Select(g => new { Autor = g.Key, Count = g.Count() })
                                            .ToList();

                // Datos para libros prestados por usuario
                var usuariosPrestamosCount = prestamos.GroupBy(p => p.IdUsuario)
                                                      .Select(g => new { UsuarioId = g.Key, Count = g.Count() })
                                                      .ToDictionary(x => x.UsuarioId, x => x.Count);

                // Configurar gráfico
                chart1.Palette = ChartColorPalette.Grayscale;
                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Análisis de Libros y Préstamos");

                // Serie para libros por autor
                Series serieLibrosPorAutor = new Series("Libros por Autor")
                {
                    ChartType = SeriesChartType.Column
                };

                // Serie para libros prestados por usuario
                Series seriePrestamosPorUsuario = new Series("Libros Prestados por Usuario")
                {
                    ChartType = SeriesChartType.Line
                };

                // Agregar datos a la serie de libros por autor
                foreach (var autor in autorLibroCount)
                {
                    serieLibrosPorAutor.Points.AddXY(autor.Autor, autor.Count);
                }

                // Agregar datos a la serie de libros prestados por usuario
                foreach (var usuarioPrestamo in usuariosPrestamosCount)
                {
                    string usuarioNombre = ObtenerNombreUsuario(usuarioPrestamo.Key); // Obtén el nombre del usuario
                    seriePrestamosPorUsuario.Points.AddXY(usuarioNombre, usuarioPrestamo.Value);
                }

                // Añadir las series al gráfico
                chart1.Series.Add(serieLibrosPorAutor);
                chart1.Series.Add(seriePrestamosPorUsuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el gráfico: {ex.Message}");
            }
        }

        private string ObtenerNombreUsuario(int idUsuario)
        {
            var usuario = conexion.ObtenerUsuarios().FirstOrDefault(u => u.IdUsuario == idUsuario);
            return usuario != null ? usuario.Nombre : "Desconocido";
        }

        private void button3_Click(object sender, EventArgs e)
        {
  
                // Validar que el campo de texto no esté vacío
                if (string.IsNullOrEmpty(txtbuscar.Text))
                {
                    MessageBox.Show("Por favor, ingrese un ID de libro para buscar.");
                    return;
                }

                // Intentar convertir el texto del campo de búsqueda a un número entero
                if (int.TryParse(txtbuscar.Text, out int idLibro))
                {
                    // Buscar el libro por su ID
                    Libro libro = conexion.ObtenerLibroPorId(idLibro);

                    if (libro != null)
                    {
                        // Si se encuentra el libro, mostrar los detalles en los controles del formulario
                        txtIdLibro.Text = libro.IdLibro.ToString();
                        txtTitulo.Text = libro.Titulo;
                        txtAutor.Text = libro.Autor;
                        txtISBN.Text = libro.ISBN;
                        // No se modifica la disponibilidad
                    }
                    else
                    {
                        // Si no se encuentra el libro, mostrar un mensaje al usuario
                        MessageBox.Show("Libro no encontrado.");
                    }
                }
                else
                {
                    MessageBox.Show("ID de libro inválido. Debe ingresar un número entero.");
                }
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
