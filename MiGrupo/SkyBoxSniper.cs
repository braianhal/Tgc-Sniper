using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.MiGrupo
{
    class SkyBoxSniper : TgcSkyBox
    {
        public SkyBoxSniper(int arista,string nombreTextura)
        {   
            this.Center = new Vector3(0, 0, 0);
            this.Size = new Vector3(arista,arista,arista);
            string texturesPath = GuiController.Instance.AlumnoEjemplosMediaDir + "SkyBoxes\\" + nombreTextura + "\\";
            this.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "arriba.jpg");
            this.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "abajo.jpg");
            this.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "izquierda.jpg");
            this.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "derecha.jpg");
            this.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "atras.jpg");
            this.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "adelante.jpg");
            this.updateValues();
        }
    }
}
