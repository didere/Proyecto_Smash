using System;

namespace TeamAltF4.SuperSmash
{
    /// <summary>
    /// Excepcion personalizada para errores relacionados con el juego Super Smash. Se lanza cuando se intenta asignar valores no validos a las propiedades de los personajes, como un ataque negativo o un nombre vacio. 
    /// Permite manejar de forma clara y especifica los errores de validacion en la creacion de personajes
    /// </summary>
    class SmashException : Exception
    {
        /// <summary>
        /// Constructor de la excepcion que recibe un mensaje de error y lo pasa al constructor base de la clase Exception.
        /// </summary>
        /// <param name="mensaje">El parametro del mensaje a mostrar</param>
        public SmashException(string mensaje) : base(mensaje)
        {
        }
    }
}
