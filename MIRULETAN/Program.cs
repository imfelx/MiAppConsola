using System;
using System.IO;
using System.Threading;
using NAudio.Wave;

class Program
{
    static string[] Estudiantes = new string[3];
    static string[] Roles = new string[] { "Desarrollador en Vivo", "Facilitador de Ejercicio a Desarrollar" };
    static string[] EstudiantesSeleccionados = new string[30];
    static int contadorSeleccionados = 0;
    static string[] HistorialSeleccion = new string[30];
    static int contadorHistorial = 0;

    static Random rnd = new Random();

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

            if (!int.TryParse(Console.ReadLine(), out opcion)) opcion = 0;

            switch (opcion)
            {
                case 1:
                    IniciarSeleccion();
                    Console.WriteLine(" Presione una tecla para volver al menú...");
                    Console.ReadKey();
                    break;
                case 2:
                    EditarEstudiantes();
                    Console.WriteLine(" Presione una tecla para volver al menú...");
                    Console.ReadKey();
                    break;
                case 3:
                    MostrarHistorialSeleccion();
                    Console.WriteLine(" Presione una tecla para volver al menú...");
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
        Array.Clear(EstudiantesSeleccionados, 0, EstudiantesSeleccionados.Length);
        contadorSeleccionados = 0;

        ReproducirAudio("LuzVerde.mp3");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("🟢 Jugaremos... Muévete... ¡Luz verde!");
        Console.ResetColor();

        Console.WriteLine("\n🔄 Seleccionando aleatoriamente... espera la señal de alto...");
        bool detener = false;
        Thread hilo = new Thread(() => SimularRuletaEnVivo(ref detener));
        hilo.Start();
 
 
        ReproducirAudio("LuzRoja.mp3");
        detener = true;
        hilo.Join();


        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n🔴 ¡Luz roja! La selección se ha detenido.");
        Console.ResetColor();
       
    }

    static void SimularRuletaEnVivo(ref bool detener)
    {
        var rand = new Random();
        while (!detener)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int j = 0; j < Roles.Length; j++)
            {
                string rol = Roles[j];
                string estudiante = Estudiantes[rand.Next(Estudiantes.Length)];
                Console.WriteLine($"🔄 {rol}: {estudiante}");
            }
            Console.ResetColor();
            Thread.Sleep(150);

            for (int j = 0; j < Roles.Length; j++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
            }
        }

        for (int i = 0; i < Roles.Length; i++)
        {
            string rol = Roles[i];
            string estudiante = SeleccionarEstudianteAleatorio(rol);
            if (estudiante != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"🎯 {rol}: {estudiante}");
                Console.ResetColor();

                HistorialSeleccion[contadorHistorial++] = $"{rol}: {estudiante}";
                EstudiantesSeleccionados[contadorSeleccionados++] = estudiante;
            }
            else
            {
                Console.WriteLine($"❌ {rol}: (No disponible)");
                HistorialSeleccion[contadorHistorial++] = $"{rol}: (No disponible)";
            }
            Thread.Sleep(700);
        }
    }

    static string SeleccionarEstudianteAleatorio(string rol)
    {
        string[] disponibles = new string[Estudiantes.Length];
        int countDisponibles = 0;

        for (int i = 0; i < Estudiantes.Length; i++)
        {
            string estudiante = Estudiantes[i];
            if (string.IsNullOrWhiteSpace(estudiante)) continue;

            bool yaTuvoEsteRol = false;
            for (int j = 0; j < contadorHistorial; j++)
            {
                string[] partes = HistorialSeleccion[j].Split(':');
                if (partes.Length == 2 && partes[1].Trim() == estudiante && partes[0].Trim() == rol)
                {
                    yaTuvoEsteRol = true;
                    break;
                }
            }

            bool yaSeleccionadoEstaVuelta = false;
            for (int j = 0; j < contadorSeleccionados; j++)
            {
                if (EstudiantesSeleccionados[j] == estudiante)
                {
                    yaSeleccionadoEstaVuelta = true;
                    break;
                }
            }

            bool yaEstaEnHistorialEstaRonda = false;
            for (int j = contadorHistorial - Roles.Length; j < contadorHistorial; j++)
            {
                if (j >= 0 && HistorialSeleccion[j] != null)
                {
                    string[] partes = HistorialSeleccion[j].Split(':');
                    if (partes.Length == 2 && partes[1].Trim() == estudiante && partes[0].Trim() == rol)
                    {
                        yaEstaEnHistorialEstaRonda = true;
                        break;
                    }
                }
            }

            if (!yaTuvoEsteRol && !yaSeleccionadoEstaVuelta && !yaEstaEnHistorialEstaRonda)
            {
                disponibles[countDisponibles++] = estudiante;
            }
        }

        return countDisponibles == 0 ? null : disponibles[rnd.Next(countDisponibles)];
    }

    static void ReproducirAudio(string archivo)
    {
        if (!File.Exists(archivo))
        {
            Console.WriteLine($"⚠️ Archivo '{archivo}' no encontrado.");
            return;
        }
        try
        {
            using var audioFile = new AudioFileReader(archivo);
            using var output = new WaveOutEvent();
            output.Init(audioFile);
            output.Play();
            while (output.PlaybackState == PlaybackState.Playing)
                Thread.Sleep(100);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al reproducir el audio: {ex.Message}");
        }
    }
       static void EditarEstudiantes()
    {


        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║    E D I T A R  E S T U D I A N T E    ║");
        Console.WriteLine("╠════════════════════════════════════════╣");
        Console.WriteLine("║ 1. Agregar Nuevo                       ║");
        Console.WriteLine("║ 2. Modificar Estudiante                ║");
        Console.WriteLine("║ 3. Eliminar Estudiante                 ║");
        Console.WriteLine("║ 4. Volver al menu principal            ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.ResetColor();
        Console.Write(" Seleccione una opción: ");
        int opcion = Convert.ToInt32(Console.ReadLine());

        switch (opcion)
        {
            case 1:
                AgregarEstudiante();
                break;
            case 2:
                ModificarEstudiante();
                break;
            case 3:
                EliminarEstudiante();
                break;
            case 4:
                MostrarMenu();

                Console.WriteLine("Volviendo al menú principal...");
                break;
        }
    }

    static void AgregarEstudiante()
    {
        int espaciosDisponibles = 0;
        for (int i = 0; i < Estudiantes.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(Estudiantes[i]))
                espaciosDisponibles++;
        }
        if (espaciosDisponibles == 0)
        {
            Console.WriteLine("No hay espacio disponible para más estudiantes.");
            return;
        }

        while (espaciosDisponibles > 0)
        {
            Console.Write($"Ingrese el nombre del nuevo estudiante (quedan {espaciosDisponibles}) o presione Enter para cancelar: ");
            string? nuevo = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nuevo))
            {
                Console.WriteLine("Ingreso cancelado.");
                break;
            }

            // Validar duplicado (sin foreach ni métodos)
            bool duplicado = false;
            for (int j = 0; j < Estudiantes.Length; j++)
            {
                if (Estudiantes[j] == nuevo)
                {
                    duplicado = true;
                    break;
                }
            }

            if (duplicado)
            {
                Console.WriteLine("Ese estudiante ya existe.");
                continue;
            }

            // Buscar el primer espacio vacío
            bool agregado = false;
            for (int i = 0; i < Estudiantes.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(Estudiantes[i]))
                {
                    Estudiantes[i] = nuevo;
                    agregado = true;
                    espaciosDisponibles--;
                    Console.WriteLine("Estudiante agregado exitosamente.");
                    break;
                }
            }
            if (!agregado)
            {
                Console.WriteLine("No hay espacio disponible para más estudiantes.");
                break;
            }
        }
    }
    static void ModificarEstudiante()
    {
        Console.WriteLine("Lista de estudiantes:");
        for (int i = 0; i < Estudiantes.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(Estudiantes[i]))
                Console.WriteLine((i + 1) + ". " + Estudiantes[i]);
        }

        Console.Write("Número del estudiante a modificar (o 0 para cancelar): ");
        string? input = Console.ReadLine();
        int seleccion;
        if (!int.TryParse(input, out seleccion) || seleccion < 0 || seleccion > Estudiantes.Length)
        {
            Console.WriteLine("Número inválido.");
            return;
        }
        if (seleccion == 0)
        {
            Console.WriteLine("Cancelado.");
            return;
        }
        int posicion = seleccion - 1;
        if (posicion >= 0 && posicion < Estudiantes.Length && !string.IsNullOrWhiteSpace(Estudiantes[posicion]))
        {
            Console.Write("Ingrese el nuevo nombre: ");
            string? nuevoNombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                Console.WriteLine("Modificación cancelada.");
                return;
            }
            // Verificar duplicado simple
            for (int i = 0; i < Estudiantes.Length; i++)
            {
                if (i != posicion && Estudiantes[i] == nuevoNombre)
                {
                    Console.WriteLine("Ese nombre ya existe.");
                    return;
                }
            }
            Estudiantes[posicion] = nuevoNombre;
            Console.WriteLine("Estudiante modificado.");
        }
        else
        {
            Console.WriteLine("Número inválido.");
        }
    }

    static void EliminarEstudiante()
    {
        Console.WriteLine("Lista de estudiantes:");
        for (int i = 0; i < Estudiantes.Length; i++)
        {
            if (Estudiantes[i] != null && Estudiantes[i] != "")
                Console.WriteLine((i + 1) + ". " + Estudiantes[i]);
        }

        Console.Write("Número del estudiante a eliminar: ");
        int seleccion = Convert.ToInt32(Console.ReadLine());
        int posicion = seleccion - 1;

        if (seleccion == 0)
        {
            Console.WriteLine("Cancelado.");
            return;
        }

        if (posicion >= 0 && posicion < Estudiantes.Length && Estudiantes[posicion] != null && Estudiantes[posicion] != "")
        {
            Estudiantes[posicion] = "";
            Console.WriteLine("Estudiante eliminado.");
        }
        else
        {
            Console.WriteLine("Número inválido.");
        }
    }



static void MostrarHistorialSeleccion()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔═════════════════════════════════════════════════╗");
    Console.WriteLine("║   H I S T O R I A L   D E   S E L E C C I Ó N   ║");
    Console.WriteLine("╠═════════════════════════════════════════════════╣");

    if (contadorHistorial == 0)
    {
        Console.WriteLine("No hay historial de selección.");
    }
    else
    {
        for (int i = 0; i < contadorHistorial; i++)
        {
            Console.WriteLine(HistorialSeleccion[i]);
        }
    }

    Console.ResetColor();
    Console.WriteLine("Presione una tecla para volver al menú...");
    Console.ReadKey();
}

}