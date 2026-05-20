using System;
using Proyecto_Smash.Models;

namespace TeamAltF4.SuperSmash
{
    /// <summary>
    /// Clase que representa a un personaje zoner en el juego Super Smash.
    /// </summary>
    class Zoner : Personaje
    {
        private Random random = new Random();
        #region Metodos sobreescritos
        public override void Jab(Personaje jugador, Personaje objetivo)
        {
            double tirada = random.NextDouble();
            if (tirada < objetivo.ProbEsquivar)
            {
                Console.WriteLine(objetivo.Nombre + " esquivo el proyectil de " + jugador.Nombre + "!");
                return;
            }
            int dano = random.Next(0, jugador.Ataque + 1);
            objetivo.Vida -= dano;
            Console.WriteLine(jugador.Nombre + " lanza un proyectil y causa " + dano + " de dano");
        }
        public override void Smash(Personaje jugador, Personaje objetivo)
        {
            double tirada = random.NextDouble();
            if (tirada < objetivo.ProbEsquivar)
            {
                Console.WriteLine(objetivo.Nombre + " esquivo el Smash Aereo de " + jugador.Nombre + "!");
                return;
            }
            int dano = random.Next(jugador.Ataque, jugador.Ataque * 2);
            objetivo.Vida -= dano;
            Console.WriteLine(jugador.Nombre + " usa Smash Aereo y causa " + dano + " de dano");
        }
        public override void Salto(Personaje jugador)
        {
            double bonificacion = (double)(150 - jugador.Peso) / 1000;
            jugador.ProbEsquivar += bonificacion;
            if (jugador.ProbEsquivar > 1.0)
                jugador.ProbEsquivar = 1.0;
            Console.WriteLine(jugador.Nombre + " flota y controla distancia. Esquive aumentado a " + (jugador.ProbEsquivar * 100) + "%");
        }
        public override void FinalSmash(Personaje jugador, Personaje objetivo)
        {
            double tirada = random.NextDouble();
            if (tirada < objetivo.ProbEsquivar)
            {
                Console.WriteLine(objetivo.Nombre + " esquivo el Smash Final de " + jugador.Nombre + "!");
                return;
            }
            int dano = random.Next(jugador.Ataque, jugador.Ataque * 2);
            objetivo.Vida -= dano;
            int curacion = dano / 5;
            jugador.Vida += curacion;
            Console.WriteLine(jugador.Nombre + " activa Smash Final: " + dano + " de dano y recupera " + curacion + " de vida.");
        }
        #endregion
    }
}
