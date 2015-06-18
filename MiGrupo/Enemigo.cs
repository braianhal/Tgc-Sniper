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
        public TgcSkeletalMesh enemigo;
        int vida;
        public bool murio = false;
        float velocidadRotacionCaida = 0.005f;
        float aceleracionCaida = 0.005f;
        Double desdeQueMurio = -1;

        enum Estado {PERSIGUIENDO,PARADO,ATACANDO,MUERTO,ESQUIVANDO};
        Estado estado = Estado.PARADO;
        Objeto objetoConElQueChoco;

        Vector3 direccionMovimiento;

        public Enemigo(Vector3 posicion,  Personaje elPersonaje,bool gigante)
        {
            TgcSkeletalLoader skeletalLoader = new TgcSkeletalLoader();

            enemigo = skeletalLoader.loadMeshAndAnimationsFromFile(
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\" + "CS_Arctic-TgcSkeletalMesh.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\",
                    new string[] { 
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "StandBy-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Run-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "HighKick-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + "Walk-TgcSkeletalAnim.xml"
                });

            enemigo.Position = posicion;
            
            personaje = elPersonaje;
            
            

            if (gigante)
            {
                vida = 500;
                velocidad = 10f + (float)Randomizar.Instance.NextDouble() * 20;
                enemigo.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else
            {
                velocidad = 25f + (float)Randomizar.Instance.NextDouble() * 30;
                vida = 100;
                enemigo.Scale = new Vector3(0.25f, 0.25f, 0.25f);
            }
            enemigo.BoundingBox.scaleTranslate(enemigo.Position, new Vector3(0.25f,1,0.25f));
        }

        public void actualizar(float elapsedTime,Personaje personaje, List<Objeto> objetos, bool renderizar)
        {
            switch(estado){
                case Estado.PARADO:
                    enemigo.playAnimation("StandBy", true);
                    if ((personaje.posicion() - enemigo.Position).Length() < 250f)
                    {
                        estado = Estado.PERSIGUIENDO;
                    }
                    break;
                case Estado.PERSIGUIENDO:
                    enemigo.playAnimation("Run", true);
                        
                        Vector3 lastPos = enemigo.Position;
                        Vector3 posicionPersonaje = personaje.posicion();
                        direccionMovimiento = posicionPersonaje-lastPos;
                        direccionMovimiento.Y = 0;
                        direccionMovimiento.Normalize();
        
                        enemigo.move(direccionMovimiento*velocidad*elapsedTime);
            
                        float angulo = (float)Math.Atan2(enemigo.Position.Z - posicionPersonaje.Z, enemigo.Position.X - posicionPersonaje.X);
                        enemigo.rotateY(anguloAnterior-angulo);
                        anguloAnterior = angulo;
            
                        //Detectar colisiones de BoundingBox utilizando herramienta TgcCollisionUtils

                        detectarColisiones( objetos);
                        

                        //Si hubo colision, restaurar la posicion anterior
                        if (collide)
                        {
                            estado = Estado.ESQUIVANDO;
                            float unAngulo = (float)Math.PI / 2;
                            direccionMovimiento = new Vector3((float)Math.Cos(unAngulo + enemigo.Rotation.Y), 0, (float) Math.Sin(unAngulo + enemigo.Rotation.Y));
                            direccionMovimiento.Normalize();
                            collide = false;
                        }
            
                        atacarAPersonaje(personaje);
                        

                    break;
                case Estado.ESQUIVANDO:
                    enemigo.playAnimation("Walk", true);


                    if(objetoConElQueChoco == null){
                        estado = Estado.PERSIGUIENDO;
                    }
                    else
                    {
                        TgcCollisionUtils.BoxBoxResult resultado = TgcCollisionUtils.classifyBoxBox(enemigo.BoundingBox, objetoConElQueChoco.colisionFisica);
                        if (resultado == TgcCollisionUtils.BoxBoxResult.Adentro || resultado == TgcCollisionUtils.BoxBoxResult.Atravesando)
                        {
                            enemigo.move(direccionMovimiento * velocidad * elapsedTime);
                            
                        }
                        else
                        {

                            estado = Estado.PERSIGUIENDO;
                        }
                    }
                    
                    break;
                case Estado.MUERTO:
                    
                    if (enemigo.Rotation.Z > (Math.PI / 2))
                    {
                        if (desdeQueMurio < 0)
                        {
                            desdeQueMurio = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
                        }
                        else
                        {
                            if (System.DateTime.Now.TimeOfDay.TotalMilliseconds - desdeQueMurio > 5000)
                            {
                                murio = true;
                            }
                        } 
                        
                    }
                    else
                    {
                        enemigo.playAnimation("StandBy", false);
                        enemigo.rotateZ(velocidadRotacionCaida * elapsedTime);
                        velocidadRotacionCaida += aceleracionCaida;
                    }
                    break;
            }
            if (renderizar)
            {
                enemigo.animateAndRender();
            }
           
        }

        private void atacarAPersonaje(Personaje personaje)
        {   
            float distanciaAPersonaje = (enemigo.Position - GuiController.Instance.CurrentCamera.getPosition()).Length();
            if (distanciaAPersonaje < 20)
            {
                if (System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoAtaque > 500)
                {
                    ultimoAtaque = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
                    personaje.sacarVida();
                    enemigo.playAnimation("HighKick", true);
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
            else
            {
                estado = Estado.PERSIGUIENDO;
            }
        }

        internal void recibioExplosion(Personaje personaje)
        {
            vida = 0;
            estado = Estado.MUERTO;
            personaje.unEnemigoMenos();
        }


        private void detectarColisiones(List<Objeto> objetos)
        {
            foreach (Objeto objeto in objetos)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(enemigo.BoundingBox, objeto.colisionFisica);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    collide = true;
                    objetoConElQueChoco = objeto;
                }
            }
        }
    }

}
