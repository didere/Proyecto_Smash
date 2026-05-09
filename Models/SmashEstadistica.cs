using System;
using System.Collections.Generic;
using Proyecto_Smash.Models;

namespace TeamAltF4.SuperSmash
{
    /// <summary>
    /// Clase estatica que se encarga de mostrar el historial de partidas y estadisticas de los personajes. 
    /// Contiene un metodo para mostrar el historial de partidas, que recibe una lista de personajes con sus 
    /// respectivas victorias y muestra el personaje con mas victorias. Esta clase es responsable de la parte 
    /// de estadistica del juego, permitiendo al jugador ver su progreso y comparar su rendimiento con otros personajes.
    /// </summary>
    static class SmashEstadistica
    {
        /// <summary>
        /// Muestra el historial de partidas, ordenando la lista de personajes por victorias y mostrando el personaje con mas victorias.
        /// </summary>
        /// <param name="historial">Lista de personajes con sus respectivas victorias.</param>
        public static void MostrarHistorial(List<Personaje> historial)
        {
            if (historial.Count == 0)
            {
                Console.WriteLine("No hay registros.");
                return;
            }

            historial.Sort();
            historial.Reverse();

            Console.WriteLine("\n=== HISTORIAL DE PARTIDAS ===");
            for (int i = 0; i < historial.Count; i++)
                Console.WriteLine($"{i + 1}. {historial[i].Nombre} ({historial[i].Tipo}) - Victorias: {historial[i].Victorias}");

            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Personaje con más victorias: {historial[0].Nombre} ({historial[0].Victorias} victorias)");

        }
    }
}
