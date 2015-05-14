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
        public TgcSprite miraSprite;
        bool eliminado = false;
        TgcMesh armaMesh;
        TgcMesh miraMesh;

        public Arma(string arma,string mira)
        {
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "Modelos\\Sniper_Arma\\arma-TgcScene.xml");
            armaMesh = scene.Meshes[1];
            miraMesh = scene.Meshes[0];
            armaMesh.Scale = new Vector3(10, 10, 10);
            miraMesh.Scale = new Vector3(10, 10, 10);
            
            armaSprite = new TgcSprite();

            string pathTexturaArma = GuiController.Instance.AlumnoEjemplosMediaDir + "Sprites\\" + arma + ".png";
            armaSprite.Texture = TgcTexture.createTexture(pathTexturaArma);
            armaSprite.Scaling = new Vector2(0.7f, 0.75f);

            int anchoPantalla = GuiController.Instance.D3dDevice.Viewport.Width;
            int altoPantalla = GuiController.Instance.D3dDevice.Viewport.Height;
            armaSprite.Position = new Vector2(anchoPantalla/2,altoPantalla/2);


            miraSprite = new TgcSprite();

            string pathTexturaMira = GuiController.Instance.AlumnoEjemplosMediaDir + "Sprites\\" + mira + ".png";
            miraSprite.Texture = TgcTexture.createTexture(pathTexturaMira);

            miraSprite.Position = new Vector2(anchoPantalla / 2 - miraSprite.Texture.Width / 2, altoPantalla / 2 - miraSprite.Texture.Height / 2);


        }


        public void actualizar()
        {
            if (!eliminado)
            {
                armaMesh.render();
                miraMesh.render();
                armaMesh.move(GuiController.Instance.CurrentCamera.getPosition() - armaMesh.Position);
                GuiController.Instance.Drawer2D.beginDrawSprite();
                armaSprite.render();
                miraSprite.render();
                GuiController.Instance.Drawer2D.endDrawSprite();
            }
        }

        internal void eliminar()
        {
            eliminado = true;
        }
    }
}
