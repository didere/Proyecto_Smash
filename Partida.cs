using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Proyecto_Smash.Models;

namespace TeamAltF4.SuperSmash
{
    /// <summary>
    /// La clase Partida es el núcleo del juego, encargada de gestionar el flujo principal de la aplicación.
    /// </summary>
    class Partida
    {
        private string rutaCsv  = "historial.csv";
        private string rutaJson = "historial.json";
        private List<Personaje> historial = new List<Personaje>();
        public Partida()
        {
            if (File.Exists(rutaCsv))
            {
                CargarDesdeCSV();
                historial.Sort();
                Console.WriteLine("Historial cargado desde CSV.");
            }
            else if (File.Exists(rutaJson))
            {
                CargarDesdeJSON();
                historial.Sort();
                Console.WriteLine("Historial cargado desde JSON.");
            }
            else
            {
                Console.WriteLine("No hay historial previo.");
            }

            int opcion = 0;
            do
            {
                Console.WriteLine("\n=== SUPER SMASH TEXT ===");
                Console.WriteLine("1. Iniciar partida");
                Console.WriteLine("2. Administrar historial");
                Console.WriteLine("0. Salir");
                Console.Write("Opcion: ");

                try
                {
                    string entrada = Console.ReadLine();
                    if (!int.TryParse(entrada, out opcion))
                        throw new SmashException("Entrada no valida. Escribe un numero.");

                    switch (opcion)
                    {
                        case 1:
                            IniciarPartida();
                            break;
                        case 2:
                            MenuHistorial();
                            break;
                        case 0:
                            Console.WriteLine("Fin del juego");
                            break;
                        default:
                            throw new SmashException("Opcion no valida. Elige 0, 1 o 2.");
                    }
                }
                catch (SmashException ex)
                {
                    Console.WriteLine(ex.Message);
                    opcion = -1;
                }
            }
            while (opcion != 0);
        }
        /// <summary>
        /// Enseña el menu de el historial, donde el usuario puede manipular el historial de el juego
        /// </summary>
        private void MenuHistorial()
        {
            int opcion = 0;
            do
            {
                Console.WriteLine("\n=== ADMINISTRAR HISTORIAL ===");
                Console.WriteLine("1. Ver historial");
                Console.WriteLine("2. Usar personaje del historial para jugar");
                Console.WriteLine("3. Eliminar personaje del historial");
                Console.WriteLine("4. Filtrar historial por clase");
                Console.WriteLine("5. Guardar cambios (CSV)");
                Console.WriteLine("6. Exportar historial (JSON)");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                try
                {
                    string entrada = Console.ReadLine();
                    if (!int.TryParse(entrada, out opcion))
                        throw new SmashException("Entrada no valida. Escribe un numero.");

                    switch (opcion)
                    {
                        case 1:
                            SmashEstadistica.MostrarHistorial(historial);
                            break;
                        case 2:
                            UsarPersonajeDelHistorial();
                            break;
                        case 3:
                            EliminarPersonajeDelHistorial();
                            break;
                        case 4:
                            FiltrarHistorialPorClase();
                            break; 
                        case 5:
                            GuardarCSV();
                            break;
                        case 6:
                            GuardarJSON();
                            break;
                        case 0:
                            Console.WriteLine("Volviendo al menu principal...");
                            break;
                        default:
                            throw new SmashException("Opcion no valida. Elige entre 0 y 4.");
                    }
                }
                catch (SmashException ex)
                {
                    Console.WriteLine(ex.Message);
                    opcion = -1;
                }
            }
            while (opcion != 0);
        }

        /// <summary>
        /// Esta funcion permite al usuario filtrar el historial de personajes por su clase (Ligero, Pesado o Zoner).
        /// </summary>
        private void FiltrarHistorialPorClase()
        {
            if (historial.Count == 0)
            {
                Console.WriteLine("No hay personajes en el historial.");
                return;
            }

            Console.WriteLine("\n=== FILTRAR POR CLASE ===");
            Console.WriteLine("1. Ligero");
            Console.WriteLine("2. Pesado");
            Console.WriteLine("3. Zoner");
            Console.Write("Elige clase: ");

            try
            {
                string entrada = Console.ReadLine();
                if (!int.TryParse(entrada, out int opcion))
                    throw new SmashException("Entrada no valida. Escribe un numero.");

                string clase;
                switch (opcion)
                {
                    case 1:
                        clase = "Ligero"; 
                        break;
                    case 2: 
                        clase = "Pesado"; 
                        break;
                    case 3: 
                        clase = "Zoner"; 
                        break;
                    default: throw new SmashException("Opcion no valida. Elige 1, 2 o 3.");
                }

                List<Personaje> filtrados = new List<Personaje>();
                foreach (Personaje p in historial)
                    if (p.Tipo == clase)
                        filtrados.Add(p);

                if (filtrados.Count == 0)
                {
                    Console.WriteLine("No hay personajes de tipo " + clase + " en el historial.");
                    return;
                }

                Console.WriteLine("\n=== HISTORIAL - CLASE: " + clase.ToUpper() + " ===");
                for (int i = 0; i < filtrados.Count; i++)
                    Console.WriteLine($"{i + 1}. {filtrados[i].Resumen()}");
            }
            catch (SmashException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Elije a un personaje del historial para jugar una partida con el.
        /// El usuario puede seleccionar el personaje por su numero en la lista,
        /// y si la seleccion es valida, se inicia una partida con ese personaje. 
        /// Al finalizar la partida, se actualiza el historial con las victorias obtenidas por ese personaje.
        /// </summary>
        private void UsarPersonajeDelHistorial()
        {
            if (historial.Count == 0)
            {
                Console.WriteLine("No hay personajes en el historial.");
                return;
            }

            SmashEstadistica.MostrarHistorial(historial);
            Console.Write("Elige el numero del personaje que quieres usar: ");

            try
            {
                string entrada = Console.ReadLine();
                if (!int.TryParse(entrada, out int indice))
                    throw new SmashException("Entrada no valida. Escribe un numero.");

                indice -= 1;
                if (indice < 0 || indice >= historial.Count)
                    throw new SmashException("Numero fuera de rango.");

                Personaje elegido = historial[indice];
                elegido.Vida = elegido.VidaMaxima;
                Console.WriteLine("Has elegido a " + elegido.Nombre + " (" + elegido.Tipo + ")");            
                EjecutarCombate(elegido, indice);
            }
            catch (SmashException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Selecciona un personaje del historial para eliminarlo. 
        /// El usuario elige el personaje por su numero en la lista, 
        /// y si la seleccion es valida, se elimina ese personaje del historial.
        /// Al finalizar, se muestra un mensaje de confirmacion.
        /// </summary>
        private void EliminarPersonajeDelHistorial()
        {
            if (historial.Count == 0)
            {
                Console.WriteLine("No hay personajes en el historial.");
                return;
            }

            SmashEstadistica.MostrarHistorial(historial);
            Console.Write("Elige el numero del personaje a eliminar: ");

            try
            {
                string entrada = Console.ReadLine();
                if (!int.TryParse(entrada, out int indice))
                    throw new SmashException("Entrada no valida. Escribe un numero.");

                indice -= 1;
                if (indice < 0 || indice >= historial.Count)
                    throw new SmashException("Numero fuera de rango.");

                Console.WriteLine("Personaje eliminado: " + historial[indice].Nombre);
                historial.RemoveAt(indice);
                historial.Sort();
            }
            catch (SmashException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Inicia una nueva partida, permitiendo al usuario seleccionar un personaje 
        /// y enfrentarse a una serie de enemigos generados aleatoriamente. Al finalizar 
        /// la partida, se actualiza el historial con las victorias obtenidas y se ofrece
        /// la opción de guardar el historial en formato CSV o JSON.
        /// </summary>
        private void IniciarPartida()
        {
            Personaje jugador = SeleccionarPersonaje();
            Console.WriteLine("\nHas elegido:");
            Console.WriteLine(jugador.Resumen());
            EjecutarCombate(jugador, -1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jugador">El personaje seleccionado por el usuario para la partida.</param>
        /// <param name="indiceHistorial">El índice del personaje en el historial, -1 si es un personaje nuevo.</param>
        private void EjecutarCombate(Personaje jugador, int indiceHistorial)
        {
            List<Personaje> enemigos = Enemigo.ObtenerEnemigos();
            Random random = new Random();
            int victorias = 0;

            do
            {
                int indice = random.Next(0, enemigos.Count);
                Personaje enemigo = enemigos[indice];
                Console.WriteLine("\n¡Ha aparecido " + enemigo.Nombre + "!");
                Console.WriteLine(enemigo.Resumen());
                Console.WriteLine("Presiona una tecla para comenzar...");
                Console.ReadKey();

                do
                {
                    int accion = PedirAccion();

                    try
                    {
                        EjecutarAccion(accion, jugador, enemigo);
                    }
                    catch (SmashException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                    if (enemigo.Vida > 0)
                        Enemigo.Atacar(enemigo, jugador);

                    Console.WriteLine("--- " + jugador.Nombre + " VIDA: " + jugador.Vida + " | " + enemigo.Nombre + " VIDA: " + enemigo.Vida + " ---");
                    Console.WriteLine("Presiona una tecla para continuar...");
                    Console.ReadKey();
                }
                while (jugador.Vida > 0 && enemigo.Vida > 0);

                if (enemigo.Vida <= 0)
                {
                    Console.WriteLine("\n¡" + enemigo.Nombre + " fue derrotado!");
                    victorias++;
                    enemigos.RemoveAt(indice);
                }
            }
            while (jugador.Vida > 0 && enemigos.Count > 0);

            if (jugador.Vida > 0)
                Console.WriteLine("\n¡Ganaste el torneo! Victorias: " + victorias);
            else
                Console.WriteLine("\nFuiste derrotado...");

            
            if (indiceHistorial >= 0)
            {
                historial[indiceHistorial].Victorias += victorias;
                Console.WriteLine("Victorias acumuladas de " + jugador.Nombre + ": " + historial[indiceHistorial].Victorias);
            }
            else
            {
                jugador.Victorias = victorias;
                historial.Add(jugador);
            }
            historial.Sort();

            int opGuardar = -1;
            do
            {
                Console.WriteLine("\n1. Guardar historial (CSV)");
                Console.WriteLine("2. Exportar historial (JSON)");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                try
                {
                    string entrada = Console.ReadLine();
                    if (!int.TryParse(entrada, out opGuardar))
                        throw new SmashException("Entrada no valida. Escribe un numero.");

                    if (opGuardar == 1)
                        GuardarCSV();
                    else if (opGuardar == 2)
                        GuardarJSON();
                    else if (opGuardar != 0)
                        throw new SmashException("Opcion no valida. Elige 0, 1 o 2.");
                }
                catch (SmashException ex)
                {
                    Console.WriteLine(ex.Message);
                    opGuardar = -1;
                }
            }
            while (opGuardar != 0);
        }
        
        /// <summary>
        /// Permite al usuario seleccionar un personaje para la partida.
        /// </summary>
        /// <returns>El personaje seleccionado por el usuario.</returns>
        private Personaje SeleccionarPersonaje()
        {
            Console.WriteLine("\n=== SELECCIONA TU PERSONAJE ===");
            Console.WriteLine("1. Ligero  (ATK:35 | VEL:10.0 | PESO:20  | VIDA:90  | ESQ:25%)");
            Console.WriteLine("2. Pesado  (ATK:95 | VEL:2.5  | PESO:120 | VIDA:180 | ESQ:5%)");
            Console.WriteLine("3. Zoner   (ATK:90 | VEL:7.5  | PESO:30  | VIDA:130 | ESQ:20%)");
            int opcion = 0;
            do
            {
                Console.Write("Elige (1-3): ");
                try
                {
                    string entrada = Console.ReadLine();
                    if (!int.TryParse(entrada, out opcion))
                        throw new SmashException("Entrada no valida. Escribe un numero.");
                    if (opcion < 1 || opcion > 3)
                        throw new SmashException("Opcion no valida. Elige 1, 2 o 3.");
                }
                catch (SmashException ex)
                {
                    Console.WriteLine(ex.Message);
                    opcion = 0;
                }
            }
            while (opcion < 1 || opcion > 3);

            string nombre = "";
            do
            {
                Console.Write("Nombre de tu personaje: ");
                nombre = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nombre))
                    Console.WriteLine("El nombre no puede estar vacio.");
            }
            while (string.IsNullOrWhiteSpace(nombre));

            if (opcion == 1) return new Personaje(nombre, "Ligero", ataque: 35, rapidez: 10.0, peso: 20,  vida: 90,  probEsquivar: 0.25);
            if (opcion == 2) return new Personaje(nombre, "Pesado", ataque: 95, rapidez: 2.5,  peso: 120, vida: 180, probEsquivar: 0.05);
            return             new Personaje(nombre, "Zoner",  ataque: 90, rapidez: 7.5,  peso: 30,  vida: 130, probEsquivar: 0.20);
        }
        /// <summary>
        /// El menu de combate donde el jugador puede elegir entre diferentes acciones (Jab, Smash, Salto, Final Smash) para atacar al enemigo.
        /// </summary>
        /// <returns>La acción seleccionada por el jugador.</returns>
        private int PedirAccion()
        {
            Console.WriteLine("\n¿Que deseas hacer?");
            Console.WriteLine("1. Jab");
            Console.WriteLine("2. Smash");
            Console.WriteLine("3. Salto");
            Console.WriteLine("4. Final Smash");
            int accion = 0;
            do
            {
                Console.Write("Accion: ");
                try
                {
                    string entrada = Console.ReadLine();
                    if (!int.TryParse(entrada, out accion))
                        throw new SmashException("Entrada no valida. Escribe un numero.");
                    if (accion < 1 || accion > 4)
                        throw new SmashException("Accion no valida. Elige entre 1 y 4.");
                }
                catch (SmashException ex)
                {
                    Console.WriteLine(ex.Message);
                    accion = 0;
                }
            }
            while (accion < 1 || accion > 4);
            return accion;
        }
        /// <summary>
        /// Ejecuta la acción seleccionada por el jugador sobre el enemigo.
        /// </summary>
        /// <param name="accion">La acción seleccionada por el jugador.</param>
        /// <param name="jugador">El personaje que realiza la acción.</param>
        /// <param name="enemigo">El personaje que recibe la acción.</param>
        /// <exception cref="SmashException">Se lanza cuando la acción no es reconocida.</exception>
        private void EjecutarAccion(int accion, Personaje jugador, Personaje enemigo)
        {
            if (jugador.Tipo == "Ligero")
            {
                Ligero l = new Ligero();
                switch (accion)
                {
                    case 1: 
                        l.Jab(jugador, enemigo);        
                        break;
                    case 2: 
                        l.Smash(jugador, enemigo);      
                        break;
                    case 3: 
                        l.Salto(jugador);               
                        break;
                    case 4: 
                        l.FinalSmash(jugador, enemigo); 
                        break;
                    default: 
                        throw new SmashException("Accion no reconocida.");
                }
            }
            else if (jugador.Tipo == "Pesado")
            {
                Pesado p = new Pesado();
                switch (accion)
                {
                    case 1: 
                        p.Jab(jugador, enemigo);        
                        break;
                    case 2: 
                        p.Smash(jugador, enemigo);      
                        break;
                    case 3: 
                        p.Salto(jugador);               
                        break;
                    case 4: 
                        p.FinalSmash(jugador, enemigo); 
                        break;
                    default: 
                        throw new SmashException("Accion no reconocida.");
                }
            }
            else
            {
                Zoner z = new Zoner();
                switch (accion)
                {
                    case 1: 
                        z.Jab(jugador, enemigo);        
                        break;
                    case 2: 
                        z.Smash(jugador, enemigo);      
                        break;
                    case 3: 
                        z.Salto(jugador);               
                        break;  
                    case 4: 
                        z.FinalSmash(jugador, enemigo); 
                        break;
                    default: 
                        throw new SmashException("Accion no reconocida.");
                }
            }
        }
        /// <summary>
        /// Abre un streamwriter dentro de la ruta del csv y escribe
        /// cada personaje del historial en una nueva linea, separando 
        /// sus atributos por comas. Al finalizar, cierra el streamwriter
        /// y muestra un mensaje de confirmacion.
        /// </summary>
        private void GuardarCSV()
        {
            StreamWriter sw = new StreamWriter(rutaCsv);
            foreach (Personaje p in historial)
                sw.WriteLine($"{p.Nombre},{p.Tipo},{p.Ataque},{p.Rapidez},{p.Peso},{p.VidaMaxima},{p.ProbEsquivar},{p.Victorias}");
            sw.Flush();
            sw.Close();
            Console.WriteLine("Historial guardado en CSV.");
        }
        /// <summary>
        /// Usa la clase de JsonSerializer para convertir cada personaje del
        /// historial a formato JSON y escribirlo en una nueva linea dentro de
        /// la ruta del json. Al finalizar, cierra el streamwriter y muestra un mensaje de confirmacion.
        /// </summary>
        private void GuardarJSON()
        {
            StreamWriter sw = new StreamWriter(rutaJson);
            foreach (Personaje p in historial)
                sw.WriteLine(JsonSerializer.Serialize(p));
            sw.Flush();
            sw.Close();
            Console.WriteLine("Historial exportado a JSON.");
        }
        /// <summary>
        /// Lee cada linea del archivo CSV, separa los atributos por comas y crea un nuevo personaje con esos atributos.
        /// </summary>
        private void CargarDesdeCSV()
        {
            foreach (string linea in File.ReadLines(rutaCsv))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                string[] d = linea.Split(',');
                if (d.Length < 8) continue;

                Personaje p = new Personaje(d[0], d[1],
                    Convert.ToInt32(d[2]),
                    Convert.ToDouble(d[3]),
                    Convert.ToInt32(d[4]),
                    Convert.ToInt32(d[5]),
                    Convert.ToDouble(d[6]));
                p.Victorias = Convert.ToInt32(d[7]);
                historial.Add(p);
            }
        }
        /// <summary>
        /// Utiliza la clase JsonSerializer para leer cada linea del archivo JSON, convertirla a un objeto Personaje y agregarlo al historial.
        /// </summary>
        private void CargarDesdeJSON()
        {
            foreach (string linea in File.ReadLines(rutaJson))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                Personaje p = JsonSerializer.Deserialize<Personaje>(linea);
                if (p != null)
                    historial.Add(p);
            }
        }
    }
}
