using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Personaje
    {   
        int vida = 100;
        Arma arma;
        TgcText2d textoVida;
        TgcText2d textoEnemigos;
        TgcText2d textoFinJuego;
        TgcText2d textoSinBalas;
        TgcSprite atacadoSprite;
        TgcBoundingBox boundingCamara;
        Vector3 ultimaPos = GuiController.Instance.CurrentCamera.getPosition();
        int cantidadEnemigos = 20;
        int cantidadBalas = 10;


        public Personaje(Arma armaUsada, TgcBoundingBox camaraColision)
        {
            arma = armaUsada;
            boundingCamara = camaraColision;

            int anchoPantalla = GuiController.Instance.D3dDevice.Viewport.Width;
            int altoPantalla = GuiController.Instance.D3dDevice.Viewport.Height;

            //Crear texto 2, especificando color, alineación, posición, tamaño y fuente.
            textoVida = new TgcText2d();
            textoVida.Text = vida.ToString();
            textoVida.Color = Color.Green;
            textoVida.Align = TgcText2d.TextAlign.LEFT;
            textoVida.Position = new Point(50, altoPantalla - 40);
            textoVida.changeFont(new System.Drawing.Font("TimesNewRoman", 25, FontStyle.Bold));

            textoEnemigos = new TgcText2d();
            textoEnemigos.Text = "20";
            textoEnemigos.Color = Color.NavajoWhite;
            textoEnemigos.Align = TgcText2d.TextAlign.RIGHT;
            textoEnemigos.Position = new Point(0, 30);
            textoEnemigos.changeFont(new System.Drawing.Font("TimesNewRoman", 25, FontStyle.Bold));

            textoFinJuego = new TgcText2d();
            textoFinJuego.Text = "VICTORIA";
            textoFinJuego.Color = Color.MediumBlue;
            textoFinJuego.Align = TgcText2d.TextAlign.CENTER;
            textoFinJuego.Position = new Point(anchoPantalla/2, altoPantalla/2);
            textoFinJuego.changeFont(new System.Drawing.Font("TimesNewRoman", 40, FontStyle.Bold));

            textoSinBalas = new TgcText2d();
            textoSinBalas.Text = "10";
            textoSinBalas.Color = Color.NavajoWhite;
            textoSinBalas.Align = TgcText2d.TextAlign.LEFT;
            textoSinBalas.Position = new Point(60,30);
            textoSinBalas.changeFont(new System.Drawing.Font("TimesNewRoman", 25, FontStyle.Bold));



            atacadoSprite = new TgcSprite();
            string pathTexturaAtacado = GuiController.Instance.AlumnoEjemplosMediaDir + "Sprites\\ataque.png";
            atacadoSprite.Texture = TgcTexture.createTexture(pathTexturaAtacado);
            int relacionAncho = GuiController.Instance.D3dDevice.Viewport.Width / 300;
            int relacionAlto = GuiController.Instance.D3dDevice.Viewport.Height / 300;
            atacadoSprite.Scaling = new Vector2(relacionAncho, relacionAlto);
        }


        public void sacarVida()
        {
            vida -= 5;
            if (vida <= 0)
            {
                vida = 0;
                morir();
            }
            else if(vida <= 25){
               textoVida.Color = Color.Red;
            }
            else if(vida <= 50){
                textoVida.Color = Color.Yellow;
            }
            textoVida.Text = vida.ToString();
            
        }

        private void morir()
        {
            CamaraSniper nuevaCamara = (CamaraSniper)GuiController.Instance.CurrentCamera;
            nuevaCamara.MovementSpeed = 0f;
            GuiController.Instance.CurrentCamera = nuevaCamara;
            GuiController.Instance.CurrentCamera.updateCamera();
            arma.eliminar();
        }
        public void actualizar()
        {   
            GuiController.Instance.Drawer2D.beginDrawSprite();
            textoSinBalas.render();
            if (muerto())
            {
                atacadoSprite.render();
            }
            textoVida.render();
            if (cantidadEnemigos > 0)
            {
                textoEnemigos.render();
            }
            else
            {
                textoFinJuego.render();
            }
            GuiController.Instance.Drawer2D.endDrawSprite();

            boundingCamara.move(posicion() - boundingCamara.Position + new Vector3(-2, 0, -2));
            
        }

        public bool muerto()
        {
            return vida == 0;
        }

        public void unaBalaMenos()
        {
            cantidadBalas--;
            textoSinBalas.Text = cantidadBalas.ToString();
        }

        public bool tieneBalas()
        {
            return cantidadBalas > 0;
        }

        public void cargarBalas()
        {
            cantidadBalas = 10;
            textoSinBalas.Text = cantidadBalas.ToString();
        }

        public void unEnemigoMenos()
        {
            cantidadEnemigos--;
            textoEnemigos.Text = cantidadEnemigos.ToString();
            
        }

        public Vector3 posicion()
        {
            return GuiController.Instance.CurrentCamera.getPosition();
        }
        public Vector3 direccionEnLaQueMira()
        {
            return new Vector3(GuiController.Instance.Frustum.NearPlane.A, GuiController.Instance.Frustum.NearPlane.B, GuiController.Instance.Frustum.NearPlane.C);
        }
    }
}
