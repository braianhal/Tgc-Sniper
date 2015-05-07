using System;

namespace AlumnoEjemplos.MiGrupo
{
    class Randomizar
    {

        // Variable estática para la instancia, se necesita utilizar una función lambda ya que el constructor es privado
        //private static readonly Lazy<Randomizer> instance = new Lazy<Randomizer>(() => new Randomizer());
        private static readonly Random rand = new Random();

        // Constructor privado para evitar la instanciación directa
        private Randomizar()
        {
        }

        // Propiedad para acceder a la instancia
        public static Random Instance
        {
            get
            {
                return rand;
            }
        }
    }
}
