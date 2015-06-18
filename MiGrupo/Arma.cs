using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Arma
    {
        public TgcSprite miraSprite;
        public TgcSprite miraZoomSprite;
        public TgcSprite armaSprite;
        bool eliminado = false;
        public TgcMesh armaMesh;
        public bool zoomActivado = false;

        public Arma(string arma,string mira)
        {
            /*TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "Modelos\\Sniper_Arma\\arma-TgcScene.xml");
            armaMesh = scene.Meshes[1];
            armaMesh.Scale = new Vector3(10, 10, 10);
            armaMesh.Position = new Vector3(5, 0, 5);*/

            int anchoPantalla = GuiController.Instance.D3dDevice.Viewport.Width;
            int altoPantalla = GuiController.Instance.D3dDevice.Viewport.Height;
            miraSprite = new TgcSprite();
            string pathTexturaMira = GuiController.Instance.AlumnoEjemplosMediaDir + "Sprites\\" + mira + ".png";
            miraSprite.Texture = TgcTexture.createTexture(pathTexturaMira);
            miraSprite.Position = new Vector2(anchoPantalla / 2 - miraSprite.Texture.Width / 2, altoPantalla / 2 - miraSprite.Texture.Height / 2);

            armaSprite = new TgcSprite();
            string pathTexturaArma = GuiController.Instance.AlumnoEjemplosMediaDir + "Sprites\\" + arma + ".png";
            armaSprite.Texture = TgcTexture.createTexture(pathTexturaArma);
            armaSprite.Scaling = new Vector2(0.55f, 0.55f);
            armaSprite.Position = new Vector2(anchoPantalla / 2 - 50, altoPantalla / 2  + 40);



            miraZoomSprite = new TgcSprite();
            string pathTexturaMiraZoom = GuiController.Instance.AlumnoEjemplosMediaDir + "Sprites\\mira_zoom.png";
            miraZoomSprite.Texture = TgcTexture.createTexture(pathTexturaMiraZoom);
            miraZoomSprite.Scaling = new Vector2(0.5f, 0.63f);
            miraZoomSprite.Position = new Vector2(0 , 0);
            
        }


        public void actualizar()
        {
            GuiController.Instance.Drawer2D.beginDrawSprite();
            if (!eliminado)
            {
                
                if (!zoomActivado)
                {
                    miraSprite.render();
                    armaSprite.render();
                }
                else
                {
                    miraZoomSprite.render();
                }
                
            }
            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public void eliminar()
        {
            eliminado = true;
        }

        public void disparar(Personaje personaje, List<Enemigo> enemigos,List<Objeto> objetos)
        {
            TgcRay disparo = new TgcRay(personaje.posicion(),personaje.direccionEnLaQueMira());
            Enemigo enemigoQueRecibeDisparo = null;
            Vector3 puntoImpacto;
            Vector3 menorPuntoImpacto = new Vector3(10000,10000,10000);
            Objeto objetoQueRecibeDisparo = null;

            foreach(Enemigo enemigo in enemigos){
                if (TgcCollisionUtils.intersectRayAABB(disparo, enemigo.enemigo.BoundingBox, out puntoImpacto))
                {   
                    if (((puntoImpacto - (personaje.posicion())).Length()) < ((menorPuntoImpacto-(personaje.posicion())).Length()))
                    {
                        menorPuntoImpacto = puntoImpacto;
                        enemigoQueRecibeDisparo = enemigo;
                    }
                }
            }
            foreach (Objeto objeto in objetos)
            {
                if (TgcCollisionUtils.intersectRayAABB(disparo, objeto.colisionFisica, out puntoImpacto))
                {
                    if (((puntoImpacto - (personaje.posicion())).Length()) < ((menorPuntoImpacto - (personaje.posicion())).Length()))
                    {
                        objetoQueRecibeDisparo = objeto;
                        enemigoQueRecibeDisparo = null;
                        break;
                    }
                }
            }
            if (enemigoQueRecibeDisparo != null)
            {
                enemigoQueRecibeDisparo.loAtaco(personaje);
            }
            else if(objetoQueRecibeDisparo != null)
            {
                objetoQueRecibeDisparo.recibirDisparo(enemigos);
            }
        }

        internal void hacerZoom()
        {
            zoomActivado = !zoomActivado;

            CamaraSniper camara = (CamaraSniper)GuiController.Instance.CurrentCamera;
            if (camara.zoom == 1)
            {
                camara.zoom = 0.1f;
            }
            else
            {
                camara.zoom = 1f;
            }
            GuiController.Instance.CurrentCamera = camara;
        }
    }
}
