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
    class Celda
    {
        public List<Celda> celdas = new List<Celda>();
        public List<Objeto> objetos = new List<Objeto>();
        public TgcBoundingBox cajaColision;

        public Celda(Vector3 posicion, float tamanio)
        {
            cajaColision = new TgcBoundingBox(posicion, posicion + (new Vector3(tamanio, tamanio, tamanio)));
        }

        public void agregarObjeto(Objeto objeto){
            objetos.Add(objeto);
        }

        public void agregarCeldas(List<Celda> celdasQueContiene){
            foreach (Celda celda in celdasQueContiene)
            {
                celdas.Add(celda);
            }
        }

       /* private bool colisionaConFrustum()
        {
            TgcFrustum frustum = GuiController.Instance.Frustum;
            bool enFrustum = (TgcCollisionUtils.classifyFrustumAABB(frustum, objeto) == TgcCollisionUtils.FrustumResult.INSIDE || TgcCollisionUtils.classifyFrustumAABB(frustum, objeto) == TgcCollisionUtils.FrustumResult.INTERSECT) || !optimizacion;
            return enFrustum && (((objeto.Position) - (personaje.posicion())).Length() < limiteRenderizado);
        }*/

    }
}
