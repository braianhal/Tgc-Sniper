using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Pasto : Objeto
    {
        float deltaRotacion = 1;
        Double ultimoCambio;

        public Pasto(Vector3 posicion, TgcMesh meshPasto)
        {
            mesh = meshPasto;
            mesh.move(posicion);
            colisionable = false;
            ultimoCambio = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
        }

        public override void render(float elapsedTime,float velocidadViento)
        {

            mesh.rotateX(elapsedTime * velocidadViento * deltaRotacion);
            if ((System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoCambio) > 500)
            {
                ultimoCambio = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
                deltaRotacion = -deltaRotacion;
            }
            base.render(elapsedTime,velocidadViento);
        }

        public override void recibirDisparo(List<Enemigo> enemigos,Personaje personaje)
        {

        }

    }
}
