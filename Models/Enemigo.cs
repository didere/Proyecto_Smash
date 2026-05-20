using System;
using System.Collections.Generic;
using Proyecto_Smash.Models;

namespace TeamAltF4.SuperSmash
{
    /// <summary>
    /// Clase estatica que representa a los enemigos
    /// y ceontiene metodos para obtener una lista de 
    /// enemigos predefinidos y para realizar ataques 
    /// de los enemigos hacia el jugador.
    /// </summary>
    static class Enemigo
    {
        private static Random random = new Random();
        #region Metodos
        /// <summary>
        /// Metodo que devuelve una lista de enemigos predefinidos con sus atributos.
        /// </summary>
        /// <returns>Lista de enemigos predefinidos</returns>
        public static List<Personaje> ObtenerEnemigos()
        {
            List<Personaje> enemigos = new List<Personaje>();
            enemigos.Add(new Personaje("Pikachu",  "Ligero",   ataque: 45, rapidez: 9.0, peso: 25,  vida: 85,  probEsquivar: 0.35));
            enemigos.Add(new Personaje("Bowser",   "Pesado", ataque: 80, rapidez: 2.0, peso: 150, vida: 160, probEsquivar: 0.05));
            enemigos.Add(new Personaje("Mega Man", "Zoner", ataque: 60, rapidez: 6.0, peso: 65,  vida: 110, probEsquivar: 0.20));
            return enemigos;
        }
        /// <summary>
        /// Metodo que simula un ataque de un enemigo hacia el jugador, teniendo en cuenta 
        /// la probabilidad de esquivar del jugador y el ataque del enemigo para calcular 
        /// el daño causado. Si el jugador esquiva, no recibe daño.
        /// </summary>
        /// <param name="enemigo">El enemigo que realiza el ataque</param>
        /// <param name="objetivo">El jugador que recibe el ataque</param>
        public static void Atacar(Personaje enemigo, Personaje objetivo)
        {
            double tirada = random.NextDouble();
            if (tirada < objetivo.ProbEsquivar)
            {
                Console.WriteLine(objetivo.Nombre + " esquivo el ataque de " + enemigo.Nombre + "!");
                return;
            }
            int dano = random.Next(0, enemigo.Ataque + 1);
            objetivo.Vida -= dano;
            Console.WriteLine(enemigo.Nombre + " ataca a " + objetivo.Nombre + " y causa " + dano + " de dano");
        }
        #endregion
    }
}
