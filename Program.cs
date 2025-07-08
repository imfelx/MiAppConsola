using System;
//HOLAAAAAAAA //MMGVOOOOOOOO
class Program
{
    static string[] Estudiantes = new string[] { "Felix", "Juan", "Pedro", "Maria" };
    static string[] Roles = new string[] { "Desarrollador en Vivo", "Facilitador de Ejercicio a Desarrollar" };
    static string[] EstudiantesSeleccionados = new string[30];
    static int contadorSeleccionados = 0;
    static string[] HistorialSeleccion = new string[30];
    static int contadorHistorial = 0;

    static void Main()
    {
        MostrarMenu();
    }

    static void MostrarMenu()
    {
        int opcion;
        do
        {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║       M E N U   P R I N C I P A L      ║");
        Console.WriteLine("╠════════════════════════════════════════╣");
        Console.WriteLine("║ 1. Iniciar Selección                   ║");
        Console.WriteLine("║ 2. Editar Estudiantes                  ║");
        Console.WriteLine("║ 3. Ver historial de selección          ║");
        Console.WriteLine("║ 4. Salir                               ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
            Console.Write(" Seleccione una opción: ");
            opcion = Convert.ToInt32(Console.ReadLine());




            switch (opcion)
            {
                case 1:
                    IniciarSeleccion();
                    Console.WriteLine("\nPresione una tecla para volver al menú...");
                    Console.ReadKey();
                    break;

                case 2:
                case 3:
                    Console.WriteLine("Esta opción aún no está disponible. Presione una tecla para continuar...");
                    Console.ReadKey();
                    break;

                case 4:
                    Console.WriteLine("Saliendo del programa...");
                    break;

                default:
                    Console.WriteLine("Opción inválida. Presione una tecla para continuar...");
                    Console.ReadKey();
                    break;
            }

        } while (opcion != 4);
    }

    static void IniciarSeleccion()
    {

        // Limpiar seleccionados de esta vuelta (pero NO el historial completo)
        Array.Clear(EstudiantesSeleccionados, 0, EstudiantesSeleccionados.Length);
        contadorSeleccionados = 0;

        for (int i = 0; i < Roles.Length; i++)
        {
            string rol = Roles[i];
            string estudiante = SeleccionarEstudianteDisponible(rol);
            if (estudiante != null)
            {
                Console.WriteLine($"El estudiante seleccionado para '{rol}' es: {estudiante}");
                HistorialSeleccion[contadorHistorial] = $"{rol}: {estudiante}";
                contadorHistorial++;
                EstudiantesSeleccionados[contadorSeleccionados] = estudiante;
                contadorSeleccionados++;
            }
            else
            {
                Console.WriteLine($"No hay estudiantes disponibles para el rol '{rol}'.");
            }
        }
    }

    static string SeleccionarEstudianteDisponible(string rol)
    {
        for (int i = 0; i < Estudiantes.Length; i++)
        {
            string estudiante = Estudiantes[i];
            bool yaTuvoEsteRol = false;

            // Verificamos si este estudiante ya tuvo este rol antes
            for (int j = 0; j < contadorHistorial; j++)
            {
                // Formato guardado: "ROL: ESTUDIANTE"
                string[] partes = HistorialSeleccion[j].Split(":");
                if (partes.Length == 2)
                {
                    string rolPrevio = partes[0].Trim();
                    string estudiantePrevio = partes[1].Trim();

                    if (estudiantePrevio == estudiante && rolPrevio == rol)
                    {
                        yaTuvoEsteRol = true;
                        break;
                    }
                }
            }

            // Si no ha tenido este rol en ninguna vuelta, lo podemos seleccionar
            bool yaSeleccionadoEstaVuelta = false;
            for (int j = 0; j < contadorSeleccionados; j++)
            {
                if (EstudiantesSeleccionados[j] == estudiante)
                {
                    yaSeleccionadoEstaVuelta = true;
                    break;
                }
            }

            if (!yaTuvoEsteRol && !yaSeleccionadoEstaVuelta)
            {
                return estudiante;
            }
        }

        return null!;
    }
}