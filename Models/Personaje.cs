using System;
using System.ComponentModel.Design;
using TeamAltF4.SuperSmash;

namespace Proyecto_Smash.Models
{
    /// <summary>
    /// Clase Plantilla que dicta las propiedades y metodos basicos de un personaje en el juego Super Smash.
    /// </summary>
    class Personaje : IComparable<Personaje>
    {
        #region Atributos
        private string nombre;
        private string tipo;
        private int ataque;
        private double rapidez;
        private int peso;
        private int vida;
        private int vidaMaxima;
        private double probEsquivar;
        private int victorias;
        #endregion
        #region Propiedades
        public string Nombre
        {
            get => nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new SmashException("El nombre del personaje no puede estar vacio.");
                nombre = value;
            }
        }
        public string Tipo
        {
            get => tipo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new SmashException("El tipo del personaje no puede estar vacio.");
                tipo = value;
            }
        }
        public int Ataque
        {
            get => ataque;
            set
            {
                if (value < 0)
                    throw new SmashException("El ataque no puede ser negativo.");
                ataque = value;
            }
        }
        public double Rapidez
        {
            get => rapidez;
            set
            {
                if (value < 0)
                    throw new SmashException("La rapidez no puede ser negativa.");
                rapidez = value;
            }
        }
        public int Peso
        {
            get => peso;
            set
            {
                if (value <= 0)
                    throw new SmashException("El peso debe ser mayor que cero.");
                peso = value;
            }
        }
        public int Vida
        {
            get => vida;
            set => vida = value;
        }
        public int VidaMaxima
        {
            get => vidaMaxima;
            set
            {
                if (value <= 0)
                    throw new SmashException("La vida maxima debe ser mayor que cero.");
                vidaMaxima = value;
            }
        }
        public double ProbEsquivar
        {
            get => probEsquivar;
            set
            {
                if (value < 0.0 || value > 1.0)
                    throw new SmashException("La probabilidad de esquivar debe estar entre 0.0 y 1.0.");
                probEsquivar = value;
            }
        }
        public int Victorias
        {
            get => victorias;
            set
            {
                if (value < 0)
                    throw new SmashException("Las victorias no pueden ser negativas.");
                victorias = value;
            }
        }
        #endregion
        #region Constructores
        /// <summary>
        /// Clase constructora del personaje, que recibe los atributos basicos del personaje y los asigna a las propiedades correspondientes.
        /// </summary>
        /// <param name="nombre">Nombre del personaje</param>
        /// <param name="tipo">Tipo del personaje</param>
        /// <param name="ataque">Valor de ataque del personaje</param>
        /// <param name="rapidez">Valor de rapidez del personaje</param>
        /// <param name="peso">Valor de peso del personaje</param>
        /// <param name="vida">Valor de vida del personaje</param>
        /// <param name="probEsquivar">Probabilidad de esquivar del personaje</param>
        public Personaje(string nombre, string tipo, int ataque, double rapidez, int peso, int vida, double probEsquivar)
        {
            Nombre = nombre;
            Tipo = tipo;
            Ataque = ataque;
            Rapidez = rapidez;
            Peso = peso;
            Vida = vida;
            VidaMaxima = vida;
            ProbEsquivar = probEsquivar;
            Victorias = 0;
        }
        /// <summary>
        /// Ckase constructora vacia para el uso correcto de el JsonSerializer, 
        /// que requiere un constructor sin parametros para poder deserializar los objetos correctamente.
        /// </summary>
        public Personaje() { }
        #endregion
        #region Metodos
        /// <summary>
        /// Clase que devuelve un resumen de las estadisticas del personaje, incluyendo su tipo, nombre, ataque, rapidez, peso, vida actual y maxima, probabilidad de esquivar y victorias.
        /// </summary>
        /// <returns>Resumen de las estadisticas del personaje</returns>
        public string Resumen()
        {
            return Tipo + " | " + Nombre + " | ATK:" + Ataque + " | VEL:" + Rapidez +
                   " | PESO:" + Peso + " | VIDA:" + Vida + "/" + VidaMaxima +
                   " | ESQ:" + ProbEsquivar * 100 + "% | VIC:" + Victorias;
        }
        /// <summary>
        /// Metodo que compara dos personajes por su cantidad de victorias, para poder ordenarlos en una tabla de lideres. Hace uso de la Interfaz iComparable 
        /// </summary>
        /// <param name="other">Usado para comparar con el personaje actual</param>
        /// <returns>Regresa un valor que indica la relación de orden entre los personajes</returns>
        public int CompareTo(Personaje other)
        {
            if (other == null) return 1;
            return Victorias.CompareTo(other.Victorias);
        }
        /// <summary>
        /// Metodo virtual que representa el ataque basico del personaje, 
        /// que puede ser sobreescrito por las clases hijas para implementar ataque basico 
        /// Recibe como parametros el personaje que realiza el ataque y el objetivo del ataque
        /// </summary>
        /// <param name="personaje">El personaje que realiza el ataque</param>
        /// <param name="objetivo">El personaje que recibe el ataque</param>
        public virtual void Jab(Personaje personaje, Personaje objetivo) { }
        /// <summary>
        /// Metodo virtual que representa el ataque fuerte del personaje,
        /// que puede ser sobreescrito por las clases hijas para implementar ataque fuerte
        /// Recibe como parametros el personaje que realiza el ataque y el objetivo del ataque
        /// </summary>
        /// <param name="personaje">El personaje que realiza el ataque</param>
        /// <param name="objetivo">El personaje que recibe el ataque</param>
        public virtual void Smash(Personaje personaje, Personaje objetivo) { }
        /// <summary>
        /// Metodo virtual que representa el salto del personaje,
        /// Aumenta la probabilidad de esquivar del personaje por un turno,
        /// recebe como parametro el personaje que realiza el salto
        /// </summary>
        /// <param name="personaje">El personaje que realiza el salto</param>
        public virtual void Salto(Personaje personaje) { }
        /// <summary>
        /// Metodo virtual que representa el ataque final del personaje,
        /// que puede ser sobreescrito por las clases hijas para implementar ataque final
        /// Recibe como parametros el personaje que realiza el ataque y el objetivo del ataque
        /// </summary>
        /// <param name="personaje">El personaje que realiza el ataque</param>
        /// <param name="objetivo">El personaje que recibe el ataque</param>
        public virtual void FinalSmash(Personaje personaje, Personaje objetivo) { }
        #endregion
    }
}
