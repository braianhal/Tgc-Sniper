using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Arbol : Objeto
    {
        float deltaRotacion = 0.5f;
        Double ultimoCambio;

        public Arbol(Vector3 posicion, TgcMesh meshArbol, string tipo)
        {
            mesh = meshArbol;
            mesh.move(posicion);
            colisionFisica = mesh.BoundingBox.clone();
            colisionable = true;

            colisionFisica.scaleTranslate(mesh.Position, new Vector3(0.08f, 1, 0.08f));

            ultimoCambio = System.DateTime.Now.TimeOfDay.TotalMilliseconds;


            /*mesh.Effect = GuiController.Instance.Shaders.TgcMeshPointLightShader;
            mesh.Technique = GuiController.Instance.Shaders.getTgcMeshTechnique(mesh.RenderType);*/
        }

        public override void render(float elapsedTime, float velocidadViento)
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
