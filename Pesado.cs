using System;
using Proyecto_Smash.Models;

namespace TeamAltF4.SuperSmash
{
    /// <summary>
    /// Clase que representa a un personaje pesado en el juego Super Smash.
    /// </summary>
    class Pesado : Personaje
    {
        private Random random = new Random();
        #region Metodos sobrescritos
        public override void Jab(Personaje jugador, Personaje objetivo)
        {
            double tirada = random.NextDouble();
            if (tirada < objetivo.ProbEsquivar)
            {
                Console.WriteLine(objetivo.Nombre + " esquivo el Jab de " + jugador.Nombre + "!");
                return;
            }
            int dano = random.Next(jugador.Ataque / 2, jugador.Ataque + 1);
            objetivo.Vida -= dano;
            Console.WriteLine(jugador.Nombre + " usa Jab Aplastante y causa " + dano + " de dano");
        }
        public override void Smash(Personaje jugador, Personaje objetivo)
        {
            double tirada = random.NextDouble();
            if (tirada < objetivo.ProbEsquivar)
            {
                Console.WriteLine(objetivo.Nombre + " esquivo el Smash de " + jugador.Nombre + "!");
                return;
            }

            int dano = random.Next(jugador.Ataque, jugador.Ataque * 3);
            objetivo.Vida -= dano;
            Console.WriteLine(jugador.Nombre + " usa Smash Descendente y causa " + dano + " de dano");
        }
        public override void Salto(Personaje jugador)
        {
            double bonificacion = (double)(150 - jugador.Peso) / 1000;
            if (bonificacion < 0)
                bonificacion = 0;
            jugador.ProbEsquivar += bonificacion;
            Console.WriteLine(jugador.Nombre + " intenta saltar... apenas se mueve. Esquive: " + (jugador.ProbEsquivar * 100) + "%");
        }
        public override void FinalSmash(Personaje jugador, Personaje objetivo)
        {
            double tirada = random.NextDouble();
            if (tirada < objetivo.ProbEsquivar)
            {
                Console.WriteLine(objetivo.Nombre + " esquivo el Smash Devastador de " + jugador.Nombre + "!");
                return;
            }
            int dano = random.Next(jugador.Ataque * 2, jugador.Ataque * 3);
            objetivo.Vida -= dano;
            Console.WriteLine(jugador.Nombre + " activa SMASH DEVASTADOR y causa " + dano + " de dano brutal!");
        }
        #endregion
    }
}
