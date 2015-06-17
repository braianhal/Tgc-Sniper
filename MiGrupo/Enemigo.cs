using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcKeyFrameLoader;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{
    class Enemigo
    {
        float anguloAnterior = (float)Math.PI / 2;
        float velocidad = 25f;
        bool collide = false;
        Double ultimoAtaque = 0;
        Personaje personaje;
        public TgcKeyFrameMesh enemigo;
        int vida;

        enum Estado {PERSIGUIENDO,PARADO,ATACANDO,MUERTO};
        Estado estado = Estado.PERSIGUIENDO;


        public Enemigo(Vector3 posicion,  Personaje elPersonaje)
        {
            //Paths para archivo XML de la malla
            string pathMesh = GuiController.Instance.ExamplesMediaDir + "KeyframeAnimations\\Robot\\Robot-TgcKeyFrameMesh.xml";

            //Path para carpeta de texturas de la malla
            string mediaPath = GuiController.Instance.ExamplesMediaDir + "KeyframeAnimations\\Robot\\";

            //Lista de animaciones disponibles
            string[] animationList = new string[]{
                "Parado",
                "Caminando",
                "Correr",
                "PasoDerecho",
                "PasoIzquierdo",
                "Empujar",
                "Patear",
                "Pegar",
                "Arrojar",
            };

            //Crear rutas con cada animacion
            string[] animationsPath = new string[animationList.Length];
            for (int i = 0; i < animationList.Length; i++)
            {
                animationsPath[i] = mediaPath + animationList[i] + "-TgcKeyFrameAnim.xml";
            }

            //Cargar mesh y animaciones
            TgcKeyFrameLoader loader = new TgcKeyFrameLoader();
            enemigo = loader.loadMeshAndAnimationsFromFile(pathMesh, mediaPath, animationsPath);

            enemigo.playAnimation("Correr", true);
            //enemigo = TgcBox.fromSize(posicion, new Vector3(10f, 10f, 20f), Color.Blue);

            enemigo.Position = posicion;
            enemigo.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            personaje = elPersonaje;
            vida = 100;
            enemigo.BoundingBox.scaleTranslate(enemigo.Position, new Vector3(0.25f,1,0.25f));
        }

        public void actualizar(float elapsedTime,Personaje personaje, List<Objeto> objetos, bool renderizar)
        {
            switch(estado){
                case Estado.PERSIGUIENDO:
                        Vector3 lastPos = enemigo.Position;
                        Vector3 posicionPersonaje = personaje.posicion();
                        Vector3 direccionMovimiento = posicionPersonaje-lastPos;
                        direccionMovimiento.Y = 0;
                        direccionMovimiento.Normalize();
        
                        enemigo.move(direccionMovimiento*velocidad*elapsedTime);
            
                        float angulo = (float)Math.Atan2(enemigo.Position.Z - posicionPersonaje.Z, enemigo.Position.X - posicionPersonaje.X);
                        enemigo.rotateY(anguloAnterior-angulo);
                        anguloAnterior = angulo;
            
                        //Detectar colisiones de BoundingBox utilizando herramienta TgcCollisionUtils


                        foreach (Objeto objeto in objetos)
                        {
                            TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(enemigo.BoundingBox,objeto.colisionFisica);
                            if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                            {
                                collide = true;
                  
                            }
               
                        }

                        //Si hubo colision, restaurar la posicion anterior
                        if (collide)
                        {
                            enemigo.Position = lastPos;
                            collide = false;
                        }
            
                        atacarAPersonaje(personaje);
                        if (renderizar)
                        {
                            enemigo.animateAndRender();

                        }

                    break;
            }
           
        }

        private void atacarAPersonaje(Personaje personaje)
        {   
            float distanciaAPersonaje = (enemigo.Position - GuiController.Instance.CurrentCamera.getPosition()).Length();
            if (distanciaAPersonaje < 12)
            {
                if (System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoAtaque > 500)
                {
                    ultimoAtaque = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
                    personaje.sacarVida();
                    if (Randomizar.Instance.NextDouble() > 0.5f)
                    {
                        enemigo.playAnimation("Correr", true);
                    }
                    else
                    {
                        enemigo.playAnimation("Pegar", true);
                    }
                }
            }
        }

        public void loAtaco(Personaje personaje)
        {
            vida -= 50;
            if (vida <= 0)
            {
                estado = Estado.MUERTO;
                personaje.unEnemigoMenos();
            }
        }

        internal void recibioExplosion(Personaje personaje)
        {
            vida = 0;
            estado = Estado.MUERTO;
            personaje.unEnemigoMenos();

        }
    }

}
