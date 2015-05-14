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

namespace AlumnoEjemplos.MiGrupo
{
    class Enemigo
    {
        float anguloAnterior = (float)Math.PI / 2;
        public TgcMesh enemigo;
        float velocidad = 30f;
        public bool eliminado = false;
        bool collide = false;
        Double ultimoAtaque = 0;
        List<AlumnoEjemplos.MiGrupo.EjemploAlumno.dataBala> balasAEliminar = new List<EjemploAlumno.dataBala>();
        public Enemigo(Vector3 posicion,TgcMesh meshEnemigo)
        {
           /*//Paths para archivo XML de la malla
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

            enemigo.playAnimation("Correr", true);*/
            //enemigo = TgcBox.fromSize(posicion, new Vector3(10f, 10f, 20f), Color.Blue);

            enemigo = meshEnemigo.clone("");
            enemigo.Position = posicion;
            
        }

        public void actualizar(Vector3 posicionPersonaje,float elapsedTime,List<AlumnoEjemplos.MiGrupo.EjemploAlumno.dataBala> balas,Personaje personaje, List<TgcMesh> meshes)
        {
            Vector3 lastPos = enemigo.Position;
            balasAEliminar = new List<EjemploAlumno.dataBala>(); 
            Vector3 direccionMovimiento = posicionPersonaje-lastPos;
            direccionMovimiento.Y = 0;
            direccionMovimiento.Normalize();
        
            enemigo.move(direccionMovimiento*velocidad*elapsedTime);
            
            float angulo = (float)Math.Atan2(enemigo.Position.Z - posicionPersonaje.Z, enemigo.Position.X - posicionPersonaje.X);
            enemigo.rotateY(anguloAnterior-angulo);
            anguloAnterior = angulo;
            
            foreach (AlumnoEjemplos.MiGrupo.EjemploAlumno.dataBala bala in balas)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(enemigo.BoundingBox, bala.bala.BoundingBox);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {  
                    eliminado = true;
                    balasAEliminar.Add(bala);
                }
            }
            //Detectar colisiones de BoundingBox utilizando herramienta TgcCollisionUtils
         
            
            foreach (TgcMesh planta in meshes)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(planta.BoundingBox, enemigo.BoundingBox);
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
            
            balas.RemoveAll(seDebeEliminar);
            //enemigo.Rotation = new Vector3((float)(Math.PI / 2), 0, 0);
            atacarAPersonaje(personaje);
            enemigo.render();
            enemigo.BoundingBox.render();
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
                }
            }
        }

        private bool seDebeEliminar(AlumnoEjemplos.MiGrupo.EjemploAlumno.dataBala bala){
            return balasAEliminar.Contains(bala);
        }
    }

}
