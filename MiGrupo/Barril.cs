using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Barril : Objeto
    {
        float radio = 20f;
        public Personaje personaje;
        public bool vivo = true;
        float totalTime;
        TgcSphere explosion = new TgcSphere();
        Vector3 escala = new Vector3(1.1f, 1.1f, 1.1f);
        Double desdeQueExplota;
        public bool destruir = false;


        public Barril(Microsoft.DirectX.Vector3 moverA, TgcMesh meshBarril, Personaje unPersonaje)
        {
            mesh = meshBarril;
            mesh.move(moverA);
            colisionFisica = mesh.BoundingBox;
            colisionable = true;
            personaje = unPersonaje;
        }

        public override void recibirDisparo(List<Enemigo> enemigos)
        {
            foreach (Enemigo unEnemigo in (enemigos.FindAll(cercaDe)))
            {
                unEnemigo.recibioExplosion(personaje);
            }

            string texturePath = GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\fuego.png";
            TgcTexture texturaExplosion = TgcTexture.createTexture(GuiController.Instance.D3dDevice, texturePath);

            //explosion = new TgcSphere(10f, texturaExplosion, mesh.Position);

            explosion.AlphaBlendEnable = true;
            explosion.updateValues();
            explosion.Position = mesh.Position;
            explosion.Radius = 12.5f;
            explosion.setTexture(texturaExplosion);
            desdeQueExplota = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
            vivo = false;
        }

        public override void render(float elapsedTime, float velocidadViento)
        {

            totalTime += GuiController.Instance.ElapsedTime;

            if (vivo)
            {
                this.mesh.render();
            }
            else
            {
                if (System.DateTime.Now.TimeOfDay.TotalMilliseconds - desdeQueExplota < 500)
                {
                    explosion.UVOffset = new Vector2(1f * totalTime, 3f * totalTime);
                    explosion.Radius *= 1.25f;

                    explosion.render();
                }
                else
                {
                    destruir = true;
                }
                //explosion.Scale = escala;
            }

        }

        private bool cercaDe(Enemigo enemigo)
        {
            return ((enemigo.enemigo.Position - mesh.Position).Length()) < radio;
        }
    }
}