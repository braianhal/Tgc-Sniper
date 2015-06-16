using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Objeto
    {
        public bool colisionable;
        public TgcBoundingBox colisionFisica;
        public TgcMesh mesh;

        public virtual void render(float elapsedTime,float velocidadViento)
        {
            this.mesh.render();
            /*if (colisionable)
            {
                this.colisionFisica.render();
            }
            this.mesh.BoundingBox.render();*/
        }

    }
}
