using System;
using System.Collections.Generic;

namespace Biblioteca
{
    public class Conexion
    {
        // Listas para almacenar usuarios, libros y préstamos
        private List<Usuario> usuarios = new List<Usuario>();
        private List<Libro> libros = new List<Libro>();
        private List<Prestamo> prestamos = new List<Prestamo>();
        private Administrador administrador;  // Usuario administrador
        private bool sesionIniciada = false;  // Estado de la sesión
        private int siguienteIdLibro = 4; // Para generar IDs automáticamente
        private int siguienteIdUsuario = 4;

        // Clases para representar Usuario, Libro, Prestamo y Administrador
        public class Usuario
        {
            public int IdUsuario { get; set; }
            public string Nombre { get; set; }
            public string Direccion { get; set; }   
            public string Telefono { get; set; }    
            public string DUI { get; set; }         
        }

        public class Libro
        {
            public int IdLibro { get; set; }
            public string Titulo { get; set; }
            public string Autor { get; set; }
            public string ISBN { get; set; }
            public bool Disponible { get; set; }
        }

        // Método para cargar 3 libros y 3 usuarios
        private void CargarRegistrosIniciales()
        {
         
        }

        public class Prestamo
        {
            public int IdPrestamo { get; set; }
            public int IdUsuario { get; set; }
            public DateTime FechaPrestamo { get; set; }
            public DateTime FechaDevolucion { get; set; }
            public List<int> LibrosPrestados { get; set; }
            public string Estado { get; set; } // 'Pendiente' o 'Devuelto'
        }

        public class Administrador
        {
            public string Usuario { get; set; }
            public string Contraseña { get; set; }
        }

        // Constructor para inicializar el administrador
        public Conexion()
        {
            // Crear el administrador con un usuario y contraseña predefinidos
            administrador = new Administrador
            {
                Usuario = "admin",
                Contraseña = "admin123"
            };

            // Agregar usuarios pre-registrados
            usuarios.Add(new Usuario { IdUsuario = 1, Nombre = "Juan Pérez", Direccion = "Av. Central 123", Telefono = "555-1234", DUI = "01234567-8" });
            usuarios.Add(new Usuario { IdUsuario = 2, Nombre = "María García", Direccion = "Calle 5, Casa 4", Telefono = "555-5678", DUI = "87654321-9" });
            usuarios.Add(new Usuario { IdUsuario = 3, Nombre = "Carlos López", Direccion = "Residencial Las Palmas", Telefono = "555-9876", DUI = "12345678-0" });

            // Agregar 3 libros con autor y ISBN
            libros.Add(new Libro
            {
                IdLibro = 1,
                Titulo = "Cien años de soledad",
                Autor = "Gabriel García Márquez",
                ISBN = "978-0307474728",
                Disponible = true
            });

            libros.Add(new Libro
            {
                IdLibro = 2,
                Titulo = "Don Quijote de la Mancha",
                Autor = "Miguel de Cervantes",
                ISBN = "978-8491050264",
                Disponible = true
            });
            libros.Add(new Libro
            {
                IdLibro = 3,
                Titulo = "Orgullo y prejuicio",
                Autor = "Jane Austen",
                ISBN = "978-1503290563",
                Disponible = true
            });
        }

        // Método para iniciar sesión como administrador
        public bool IniciarSesion(string usuario, string contraseña)
        {
            if (administrador.Usuario == usuario && administrador.Contraseña == contraseña)
            {
                sesionIniciada = true;
                Console.WriteLine("Sesión iniciada correctamente.");
                return true;
            }
            else
            {
                Console.WriteLine("Usuario o contraseña incorrectos.");
                return false;
            }
        }

        // Método para cerrar sesión
        public void CerrarSesion()
        {
            if (sesionIniciada)
            {
                sesionIniciada = false;
                Console.WriteLine("Sesión cerrada correctamente.");
            }
            else
            {
                Console.WriteLine("No hay ninguna sesión iniciada.");
            }
        }

        // Métodos para libros

        private int ObtenerSiguienteIdLibro()
        {
            return siguienteIdLibro++;
        }

        private int ObtenerSiguienteIdUsuario()
        {
            return siguienteIdUsuario++;
        }

        // Método para agregar un nuevo libro
        public void AgregarLibro(string titulo, string autor, string isbn)
        {
            int idLibro = ObtenerSiguienteIdLibro(); // Genera un nuevo ID

            libros.Add(new Libro
            {
                IdLibro = idLibro,
                Titulo = titulo,
                Autor = autor,
                ISBN = isbn,
                Disponible = true
            });
            Console.WriteLine("Libro agregado correctamente.");
        }

        // Método para modificar un libro existente
        public void ModificarLibro(int idLibro, string nuevoTitulo, string nuevoAutor, string nuevoISBN)
        {
            Libro libro = libros.Find(l => l.IdLibro == idLibro);
            if (libro != null)
            {
                libro.Titulo = nuevoTitulo;
                libro.Autor = nuevoAutor;
                libro.ISBN = nuevoISBN;
                // No se modifica la disponibilidad
                Console.WriteLine("Libro modificado correctamente.");
            }
            else
            {
                Console.WriteLine("Libro no encontrado.");
            }
        }

        // Métodos para obtener libros
        public List<Libro> ObtenerLibros()
        {
            return libros; // Devuelve la lista completa de libros
        }

        // Método para ver todos los libros
        public void VerLibros()
        {
            foreach (Libro libro in libros)
            {
                Console.WriteLine($"ID: {libro.IdLibro}, Título: {libro.Titulo}, Disponible: {libro.Disponible}");
            }
        }

        // Método para eliminar un libro
        public void EliminarLibro(int idLibro)
        {
            Libro libro = libros.Find(l => l.IdLibro == idLibro);
            if (libro != null)
            {
                libros.Remove(libro);
                Console.WriteLine("Libro eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("Libro no encontrado.");
            }
        }

        // Métodos para usuarios

        // Método para agregar un nuevo usuario
        public void AgregarUsuario(string nombre, string direccion, string telefono, string dui)
        {
            int idUsuario = ObtenerSiguienteIdUsuario();
            usuarios.Add(new Usuario
            {
                IdUsuario = idUsuario,
                Nombre = nombre,
                Direccion = direccion,  // Asigna dirección
                Telefono = telefono,    // Asigna teléfono
                DUI = dui               // Asigna DUI
            });
            Console.WriteLine("Usuario agregado correctamente.");
        }

        // Método para modificar un usuario existente
        public void ModificarUsuario(int idUsuario, string nuevoNombre, string nuevaDireccion, string nuevoTelefono, string nuevoDUI)
        {
            Usuario usuario = usuarios.Find(u => u.IdUsuario == idUsuario);
            if (usuario != null)
            {
                usuario.Nombre = nuevoNombre;
                usuario.Direccion = nuevaDireccion;  // Modifica dirección
                usuario.Telefono = nuevoTelefono;    // Modifica teléfono
                usuario.DUI = nuevoDUI;              // Modifica DUI
                Console.WriteLine("Usuario modificado correctamente.");
            }
            else
            {
                Console.WriteLine("Usuario no encontrado.");
            }
        }

        // Método para ver todos los usuarios
        public void VerUsuarios()
        {
            foreach (Usuario usuario in usuarios)
            {
                Console.WriteLine($"ID: {usuario.IdUsuario}, Nombre: {usuario.Nombre}, Dirección: {usuario.Direccion}, Teléfono: {usuario.Telefono}, DUI: {usuario.DUI}");
            }
        }

        // Método para eliminar un usuario
        public void EliminarUsuario(int idUsuario)
        {
            Usuario usuario = usuarios.Find(u => u.IdUsuario == idUsuario);
            if (usuario != null)
            {
                usuarios.Remove(usuario);
                Console.WriteLine("Usuario eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("Usuario no encontrado.");
            }
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return usuarios; // Devuelve la lista de usuarios
        }

        public List<Libro> ObtenerLibrosDisponibles()
        {
            // Devuelve solo los libros disponibles
            return libros.FindAll(libro => libro.Disponible);
        }

        public bool TienePrestamoPendiente(int idUsuario)
        {
            // Busca préstamos pendientes para el usuario
            return prestamos.Exists(prestamo => prestamo.IdUsuario == idUsuario && prestamo.Estado == "Pendiente");
        }

        public bool RegistrarPrestamo(int idUsuario, DateTime fechaPrestamo, DateTime fechaDevolucion, List<int> librosPrestados)
        {
            // Crear un nuevo préstamo y agregarlo a la lista de préstamos
            Prestamo nuevoPrestamo = new Prestamo
            {
                IdPrestamo = prestamos.Count + 1, // Genera un nuevo ID de préstamo
                IdUsuario = idUsuario,
                FechaPrestamo = fechaPrestamo,
                FechaDevolucion = fechaDevolucion,
                LibrosPrestados = new List<int>(librosPrestados),
                Estado = "Pendiente"
            };

            prestamos.Add(nuevoPrestamo);

            // Marcar los libros como no disponibles
            foreach (int idLibro in librosPrestados)
            {
                Libro libro = libros.Find(l => l.IdLibro == idLibro);
                if (libro != null)
                {
                    libro.Disponible = false;
                }
            }

            return true;
        }
        public Prestamo ObtenerPrestamoPendiente(int idUsuario)
        {
            // Devuelve el préstamo pendiente del usuario si existe
            return prestamos.Find(p => p.IdUsuario == idUsuario && p.Estado == "Pendiente");
        }

        public Libro ObtenerLibroPorId(int idLibro)
        {
            // Devuelve el libro correspondiente al ID
            return libros.Find(l => l.IdLibro == idLibro);
        }

        public bool DevolverPrestamo(int idUsuario)
        {
            Prestamo prestamo = ObtenerPrestamoPendiente(idUsuario);
            if (prestamo != null)
            {
                // Cambiar el estado del préstamo a 'Devuelto'
                prestamo.Estado = "Devuelto";

                // Hacer que los libros estén disponibles nuevamente
                foreach (int idLibro in prestamo.LibrosPrestados)
                {
                    Libro libro = ObtenerLibroPorId(idLibro);
                    if (libro != null)
                    {
                        libro.Disponible = true;
                    }
                }
                return true;
            }
            return false;
        }




        public List<Prestamo> ObtenerPrestamos()
        {
            return prestamos; // Devuelve la lista completa de préstamos
        }
    }
}
