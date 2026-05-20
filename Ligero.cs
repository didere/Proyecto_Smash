using System;
using Proyecto_Smash.Models;

namespace TeamAltF4.SuperSmash
{
    /// <summary>
    /// Clase que representa a un personaje ligero en el juego Super Smash.
    /// Este tipo de personaje se caracteriza por su alta probabilidad de esquivar ataques 
    /// y su habilidad para realizar combos rápidos, aunque su ataque base es menor que el de personajes más pesados.
    /// </summary>
    class Ligero : Personaje
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
            int dano1 = random.Next(0, jugador.Ataque + 1);
            int dano2 = random.Next(0, jugador.Ataque + 1);
            objetivo.Vida -= dano1 + dano2;
            Console.WriteLine(jugador.Nombre + " usa Jab Doble: " + dano1 + " + " + dano2 + " = " + (dano1 + dano2) + " de dano");
        }
        
        public override void Smash(Personaje jugador, Personaje objetivo)
        {
            double tirada = random.NextDouble();
            if (tirada < objetivo.ProbEsquivar)
            {
                Console.WriteLine(objetivo.Nombre + " esquivo el Smash de " + jugador.Nombre + "!");
                return;
            }
            int dano = random.Next(jugador.Ataque, jugador.Ataque * 2);
            objetivo.Vida -= dano;
            Console.WriteLine(jugador.Nombre + " usa Smash Lateral y causa " + dano + " de dano");
        }
        public override void Salto(Personaje jugador)
        {
            double bonificacion = (double)(150 - jugador.Peso) / 1000;
            jugador.ProbEsquivar += bonificacion;
            if (jugador.ProbEsquivar > 1.0)
                jugador.ProbEsquivar = 1.0;
            Console.WriteLine(jugador.Nombre + " salta alto gracias a su poco peso! Esquive aumentado a " + (jugador.ProbEsquivar * 100) + "%");
        }
        public override void FinalSmash(Personaje jugador, Personaje objetivo)
        {
            Console.WriteLine(jugador.Nombre + " activa COMBO RELAMPAGO (3 golpes):");
            int total = 0;
            for (int i = 1; i <= 3; i++)
            {
                double tirada = random.NextDouble();
                if (tirada < objetivo.ProbEsquivar)
                {
                    Console.WriteLine("  Golpe " + i + ": esquivado!");
                    continue;
                }
                int dano = random.Next(0, jugador.Ataque + 1);
                objetivo.Vida -= dano;
                total += dano;
                Console.WriteLine("  Golpe " + i + ": " + dano + " de dano");
            }
            Console.WriteLine("  Total del combo: " + total + " de dano");
        }
        #endregion
    }
}
