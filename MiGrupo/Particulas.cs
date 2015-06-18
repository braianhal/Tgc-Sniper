using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace AlumnoEjemplos.MiGrupo
{
    class Particulas
    {

        Vector3 velocity;
        float weight;
        Color color;

        public Particulas(Vector3 vel, float peso, Color col)
        {
            velocity = vel;
            weight = peso;
            color = col;

        }

        public void update(float elapsedTime)
        {

        }
    }
}
