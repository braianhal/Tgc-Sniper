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
    static class Mapa
    {
        public static TgcBox nuevoPiso(Vector2 tamanio,string textura)
        {
            string texturePath = GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\" + textura + ".jpg";
            TgcTexture texturaPiso = TgcTexture.createTexture(GuiController.Instance.D3dDevice, texturePath);
            TgcBox nuevoPiso = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(tamanio.X, 0.1f, tamanio.Y), texturaPiso);
            nuevoPiso.UVTiling = new Vector2(50, 50);
            nuevoPiso.updateValues();
            return nuevoPiso;
        }
    }
}
