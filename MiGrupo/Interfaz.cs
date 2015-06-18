using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Interfaz
    {
        public bool enTitulo = true;
        public bool enControles = false;
        public bool finJuego = false;
        public bool gano = false;

        TgcSprite titulo;
        TgcSprite controles;
        TgcSprite victoria;
        TgcSprite gameOver;
        TgcD3dInput input = GuiController.Instance.D3dInput;
        CamaraSniper camara;
        CamaraSniper camaraMenu;


        public Interfaz(CamaraSniper camaraX, CamaraSniper camaraMenuX)
        {
            string pathTextura;
            int anchoPantalla = GuiController.Instance.D3dDevice.Viewport.Width;
            int altoPantalla = GuiController.Instance.D3dDevice.Viewport.Height;

            titulo = new TgcSprite();
            pathTextura = GuiController.Instance.AlumnoEjemplosMediaDir + "Interfaz\\titulo2.png";
            titulo.Texture = TgcTexture.createTexture(pathTextura);
            titulo.Position = new Vector2(anchoPantalla / 2 - titulo.Texture.Width / 2, altoPantalla / 2 - titulo.Texture.Height / 2);

            controles = new TgcSprite();
            pathTextura = GuiController.Instance.AlumnoEjemplosMediaDir + "Interfaz\\controles2.png";
            controles.Texture = TgcTexture.createTexture(pathTextura);
            controles.Position = new Vector2(anchoPantalla / 2 - controles.Texture.Width / 2, altoPantalla / 2 - controles.Texture.Height / 2);

            victoria = new TgcSprite();
            pathTextura = GuiController.Instance.AlumnoEjemplosMediaDir + "Interfaz\\victoria1.png";
            victoria.Texture = TgcTexture.createTexture(pathTextura);
            victoria.Position = new Vector2(anchoPantalla / 2 - victoria.Texture.Width / 2, altoPantalla / 2 - victoria.Texture.Height / 2);

            gameOver = new TgcSprite();
            pathTextura = GuiController.Instance.AlumnoEjemplosMediaDir + "Interfaz\\fin1.png";
            gameOver.Texture = TgcTexture.createTexture(pathTextura);
            gameOver.Position = new Vector2(anchoPantalla / 2 - gameOver.Texture.Width / 2, altoPantalla / 2 - gameOver.Texture.Height / 2);

            camara = camaraX;
            camaraMenu = camaraMenuX;
        }

        public void mostrarTitulo()
        {
            if (input.keyPressed(Key.Space))
            {
                enTitulo = false;
                enControles = true;
            }

            GuiController.Instance.Drawer2D.beginDrawSprite();

            titulo.render();

            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public void mostrarControles()
        {
            if (input.keyPressed(Key.Space))
            {
                GuiController.Instance.CurrentCamera = camara;
                enControles = false;
                
            }

            GuiController.Instance.Drawer2D.beginDrawSprite();

            controles.render();

            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public void ganoJuego()
        {
            if (input.keyPressed(Key.Space))
            {
                //enControles = false;
            }

            GuiController.Instance.Drawer2D.beginDrawSprite();

            victoria.render();

            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public void perdioJuego()
        {
            if (input.keyPressed(Key.Space))
            {
                //enControles = false;
            }

            GuiController.Instance.Drawer2D.beginDrawSprite();

            gameOver.render();

            GuiController.Instance.Drawer2D.endDrawSprite();
        }

    }
}
