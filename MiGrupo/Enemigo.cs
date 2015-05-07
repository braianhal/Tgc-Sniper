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
        TgcBox enemigo;
        Boolean mostrar = true;
        float velocidad = 30f;
        float velocidadAngular = 1f;
        public Enemigo(Vector3 posicion)
        {
           /* //Paths para archivo XML de la malla
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
            enemigo = TgcBox.fromSize(posicion, new Vector3(10f, 10f, 20f), Color.Blue);
        }


        public void actualizar(Vector3 posicionPersonaje,float elapsedTime,List<AlumnoEjemplos.MiGrupo.EjemploAlumno.dataBala> balas)
        {   
            Vector3 direccionMovimiento = posicionPersonaje-enemigo.Position;
            direccionMovimiento.Normalize();
            enemigo.move(direccionMovimiento*velocidad*elapsedTime);

            foreach (AlumnoEjemplos.MiGrupo.EjemploAlumno.dataBala bala in balas)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(enemigo.BoundingBox, bala.bala.BoundingBox);
                if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    
                    mostrar = false;
                }
            }

            //enemigo.Rotation = new Vector3((float)(Math.PI / 2), 0, 0);
            if (mostrar)
            {
                enemigo.render();
            }
            
        }
    
    }


}
