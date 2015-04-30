using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Arma
    {
        public TgcSprite armaSprite;

        public Arma(string arma)
        {
            armaSprite = new TgcSprite();

            string pathTexturaArma = GuiController.Instance.AlumnoEjemplosMediaDir + "Sprites\\" + arma + ".png";
            armaSprite.Texture = TgcTexture.createTexture(pathTexturaArma);
            armaSprite.Scaling = new Vector2(0.7f, 0.75f);

            int anchoPantalla = GuiController.Instance.D3dDevice.Viewport.Width;
            int altoPantalla = GuiController.Instance.D3dDevice.Viewport.Height;
            armaSprite.Position = new Vector2(anchoPantalla/2,altoPantalla/2);
            
        }
  
    }
}
