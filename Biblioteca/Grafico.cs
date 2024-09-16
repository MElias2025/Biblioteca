using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Biblioteca
{
    public partial class Grafico : Form
    {
        private Conexion conexion;

        public Grafico(Conexion conexion)
        {
            InitializeComponent();
            this.conexion = conexion ?? throw new ArgumentNullException(nameof(conexion), "La instancia de Conexion no puede ser nula.");
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // Actualizar el gráfico cuando se hace clic en él
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
                // Obtener la lista completa de libros
                List<Conexion.Libro> libros = conexion.ObtenerLibros();

                if (libros == null || libros.Count == 0)
                {
                    MessageBox.Show("No se encontraron libros.");
                    return;
                }

                var autoresCount = new Dictionary<string, int>();

                foreach (var libro in libros)
                {
                    if (libro == null) continue; // Ignorar libros nulos

                    if (autoresCount.ContainsKey(libro.Autor))
                    {
                        autoresCount[libro.Autor]++;
                    }
                    else
                    {
                        autoresCount[libro.Autor] = 1;
                    }
                }

                chart1.Palette = ChartColorPalette.Grayscale;

                // Limpiar el gráfico existente
                chart1.Series.Clear();
                chart1.Titles.Clear();

                // Añadir título al gráfico
                chart1.Titles.Add("Libros por autor");

                // Crear una serie única para todos los autores
                Series serie = new Series("Autores")
                {
                    ChartType = SeriesChartType.Column // Tipo de gráfico de columnas
                };

                // Agregar puntos de datos a la serie
                foreach (var autor in autoresCount)
                {
                    serie.Points.AddXY(autor.Key, autor.Value);
                }

                // Añadir la serie al gráfico
                chart1.Series.Add(serie);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el gráfico: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActualizarGrafico();
        }

        private void Grafico_Load_1(object sender, EventArgs e)
        {

        }
    }
}

