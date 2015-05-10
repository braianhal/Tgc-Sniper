using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Personaje
    {   
        int vida = 100;
        Arma arma;
        TgcText2d textoVida;
        TgcSprite atacadoSprite;

        public Personaje(Arma armaUsada)
        {
            arma = armaUsada;
            
            //Crear texto 2, especificando color, alineación, posición, tamaño y fuente.
            textoVida = new TgcText2d();
            textoVida.Text = vida.ToString();
            textoVida.Color = Color.Green;
            textoVida.Align = TgcText2d.TextAlign.CENTER;
            textoVida.Position = new Point(300, 100);
            textoVida.Size = new Size(300, 100);
            textoVida.changeFont(new System.Drawing.Font("TimesNewRoman", 25, FontStyle.Bold | FontStyle.Italic));

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
            if (muerto())
            {
                atacadoSprite.render();
            }
            textoVida.render();
            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public bool muerto()
        {
            return vida == 0;
        }
    }
}
