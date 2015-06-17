using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Particles;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcKeyFrameLoader;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{
    class Mapa
    {
        Double ultimoCambioViento;
        Double tasaCambioViento = 1000 * 40;
        public float velocidadViento = 0.1f;
        float maximaVelocidadViento = 0.5f;
        ParticleEmitter nieve;


        public Mapa()
        {
            ultimoCambioViento = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
            nieve = new ParticleEmitter(GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\nieve.png", 100000);
            nieve.Position = new Vector3(0, 5, 0);
        }

        public static TgcBox nuevoPiso(Vector2 tamanio,string textura)
        {
            string texturePath = GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\" + textura + ".jpg";
            TgcTexture texturaPiso = TgcTexture.createTexture(GuiController.Instance.D3dDevice, texturePath);
            TgcBox nuevoPiso = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(tamanio.X, 0.1f, tamanio.Y), texturaPiso);
            nuevoPiso.UVTiling = new Vector2(50, 50);
            nuevoPiso.updateValues();
            return nuevoPiso;
        }


        public static List<Objeto> crearObjetosMapa(List<Enemigo> enemigos,int cantidadArboles,int cantidadPasto)
        {
            //TgcSceneLoader loader = new TgcSceneLoader();
            /*TgcScene[] meshesObjetos = new TgcScene[4];

            meshesObjetos[0] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Planta\\Planta-TgcScene.xml");
            meshesObjetos[1] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera3\\Palmera3-TgcScene.xml");
            meshesObjetos[2] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Roca\\Roca-TgcScene.xml");
            meshesObjetos[3] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pasto\\Pasto-TgcScene.xml");

            return crearVegetacion(meshesObjetos,enemigos);*/
            TgcSceneLoader loader = new TgcSceneLoader();
            List<Objeto> objetos = new List<Objeto>();
            TgcMesh arbol = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pino\\Pino-TgcScene.xml").Meshes[0];
            TgcMesh pasto = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Arbusto\\Arbusto-TgcScene.xml").Meshes[0];
            
            for (int i = 0; i < cantidadArboles; i++)
            {
                int numero = (int)(Randomizar.Instance.NextDouble() * 4);
                Vector3 moverA = new Vector3(-1000 + (float)Randomizar.Instance.NextDouble() * 2000, -0.5f, -1000 + (float)Randomizar.Instance.NextDouble() * 2000);
                foreach (Enemigo enemigo in enemigos)
                {
                    if ((((moverA - (enemigo.enemigo.Position)).Length()) < 3))
                    {
                        moverA = new Vector3(-1000 + (float)Randomizar.Instance.NextDouble() * 2000, -0.5f, -1000 + (float)Randomizar.Instance.NextDouble() * 2000);
                    }
                }
                objetos.Add(new Arbol(moverA, arbol.clone(""), "pino"));
            }
            pasto.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            for (int i = 0; i < cantidadPasto; i++)
            {
                Vector3 moverA = new Vector3(-500 + (float)Randomizar.Instance.NextDouble() * 1000, 0, -500 + (float)Randomizar.Instance.NextDouble() * 1000);
                objetos.Add(new Pasto(moverA, pasto.clone("")));
            }

            return objetos;
        }

        public static List<Enemigo> crearEnemigos(int cantidadEnemigos,Personaje personaje, string tipo)
        {

            //************************ VER *****************//


            List<Enemigo> enemigos = new List<Enemigo>();
            //Cargar malla original
            //Paths para archivo XML de la malla
            /*string pathMesh = GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Trooper\\Trooper-TgcSkeletalMesh.xml";

            //Path para carpeta de texturas de la malla
            string mediaPath = GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Trooper\\";

            //Lista de animaciones disponibles
            string[] animationList = new string[]{
                "TrooperStandBy",
                "TrooperWalk",
                "TrooperRun",
                "TrooperHighKick",
            };

            //Crear rutas con cada animacion
            string[] animationsPath = new string[animationList.Length];
            for (int i = 0; i < animationList.Length; i++)
            {
                animationsPath[i] = mediaPath + animationList[i] + "-TgcSkeletalAnim.xml";
            }

            //Cargar mesh y animaciones
            TgcSkeletalLoader loader = new TgcSkeletalLoader();
            TgcSkeletalMesh original = loader.loadMeshAndAnimationsFromFile(pathMesh, mediaPath, animationsPath);

            //Crear esqueleto a modo Debug
            original.buildSkletonMesh();*/

            

            for (int i = 0; i < cantidadEnemigos; i++)
            {
                float xAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                float yAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                while (xAleatorio < 50 || yAleatorio < 50)
                {
                    xAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                    yAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                }
                enemigos.Add(new Enemigo(new Vector3(xAleatorio, 0, yAleatorio),  personaje));
            }
            return enemigos;
        }


        private static List<TgcMesh> crearVegetacion(TgcScene[] meshesObjetos, List<Enemigo> enemigos)
        {
            List<TgcMesh> objetosMapa = new List<TgcMesh>();
            for (int i = 0; i < 500; i++)
            { //Se puede aumentar el numero pero si no se utiliza la grilla regular decae mucho la performance
                int numero = (int)(Randomizar.Instance.NextDouble() * 4);
                TgcMesh objeto = (TgcMesh)meshesObjetos[numero].Meshes[0].clone("");
                Vector3 moverA = new Vector3(-1000 + (float)Randomizar.Instance.NextDouble() * 2000, 0, -1000 + (float)Randomizar.Instance.NextDouble() * 2000);
                foreach (Enemigo enemigo in enemigos)
                {
                    if((((moverA-(enemigo.enemigo.Position)).Length()) < 3)){
                        moverA = new Vector3(-1000 + (float)Randomizar.Instance.NextDouble() * 2000, 0, -1000 + (float)Randomizar.Instance.NextDouble() * 2000);
                    }
                }
                objeto.move(moverA);
                if (numero == 1)
                {
                    //objeto.BoundingBox.scaleTranslate(objeto.Position, new Vector3(0.04f, 1, 0.04f));
                    objeto.Name = "palmera";
                }

                if (numero == 2)
                {
                    //objeto.BoundingBox.scaleTranslate(objeto.Position, new Vector3(0.8f, 1, 0.8f));
                    objeto.Name = "roca";
                }

                if (numero == 3)
                {
                    //objeto.BoundingBox.scaleTranslate(objeto.Position, new Vector3(0f, 0, 0f));
                    objeto.Name = "pasto";
                }

                if (numero == 0)
                {
                    //objeto.BoundingBox.scaleTranslate(objeto.Position, new Vector3(0.01f, 1, 0.01f));
                    objeto.Name = "planta";
                }

                objetosMapa.Add(objeto);
            }
            return objetosMapa;
        }

        public static List<TgcBox> crearBordes()
        {
            List<TgcBox> bordes = new List<TgcBox>();
            string texturePath = GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\pared.jpg";
            TgcTexture texturaBorde = TgcTexture.createTexture(GuiController.Instance.D3dDevice, texturePath);

            bordes.Add(TgcBox.fromSize(new Vector3(-1000, 10, 0), new Vector3(1,20,2000), texturaBorde));
            bordes.Add(TgcBox.fromSize(new Vector3(1000, 10, 0), new Vector3(1, 20, 2000), texturaBorde));
            bordes.Add(TgcBox.fromSize(new Vector3(0, 10, -1000), new Vector3(2000, 20, 1), texturaBorde));
            bordes.Add(TgcBox.fromSize(new Vector3(0, 10, 1000), new Vector3(2000, 20, 1), texturaBorde));

            foreach (TgcBox borde in bordes)
            {
                borde.UVTiling = new Vector2(50, 2);
                borde.updateValues();
            }
            
            return bordes;
        }

        public void actualizar()
        {
            if ((System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoCambioViento)> tasaCambioViento)
            {
                velocidadViento =  (float)Randomizar.Instance.NextDouble() * maximaVelocidadViento;
                ultimoCambioViento = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
            nieve.render();
        }


    }
}
