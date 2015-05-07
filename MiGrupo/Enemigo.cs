using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Enemigo
    {
        TgcBox enemigo;
        float velocidad = 30f;
        float velocidadAngular = 1f;
        public Enemigo(Vector3 posicion)
        {
            enemigo = TgcBox.fromSize(posicion, new Vector3(10f, 10f, 20f), Color.Blue);
            
        }


        public void actualizar(Vector3 posicionPersonaje,float elapsedTime)
        {   
            Vector3 direccionMovimiento = posicionPersonaje-enemigo.Position;
            direccionMovimiento.Normalize();
            enemigo.move(direccionMovimiento*velocidad*elapsedTime);

            enemigo.Rotation = new Vector3((float)(Math.PI / 2), 0, 0);
            enemigo.render();
        }
    
    }


}
