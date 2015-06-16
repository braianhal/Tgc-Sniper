using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    static class Mapa
    {
        public static TgcBox nuevoPiso(Vector2 tamanio,string textura)
        {
            string texturePath = GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\" + textura + ".jpg";
            TgcTexture texturaPiso = TgcTexture.createTexture(GuiController.Instance.D3dDevice, texturePath);
            TgcBox nuevoPiso = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(tamanio.X, 0.1f, tamanio.Y), texturaPiso);
            nuevoPiso.UVTiling = new Vector2(50, 50);
            nuevoPiso.updateValues();
            return nuevoPiso;
        }


        public static List<TgcMesh> crearObjetosMapa(List<Enemigo> enemigos)
        {
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene[] meshesObjetos = new TgcScene[4];

            meshesObjetos[0] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Planta\\Planta-TgcScene.xml");
            meshesObjetos[1] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera3\\Palmera3-TgcScene.xml");
            meshesObjetos[2] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Roca\\Roca-TgcScene.xml");
            meshesObjetos[3] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pasto\\Pasto-TgcScene.xml");

            return crearVegetacion(meshesObjetos,enemigos);
        }

        public static List<Enemigo> crearEnemigos(int cantidadEnemigos,Personaje personaje)
        {
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "Modelos\\Robot\\Robot-TgcScene.xml");
            TgcMesh enemigo = scene.Meshes[0];
            List<Enemigo> enemigos = new List<Enemigo>();
            enemigo.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < cantidadEnemigos; i++)
            {
                float xAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                float yAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                while (xAleatorio < 50 || yAleatorio < 50)
                {
                    xAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                    yAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                }
                enemigos.Add(new Enemigo(new Vector3(xAleatorio, 0, yAleatorio), enemigo, personaje));
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

        public static List<TgcMesh> crearPasto()
        {
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcMesh unPasto = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Flores\\Flores.max-TgcScene.xml").Meshes[0];

            List<TgcMesh> pasto = new List<TgcMesh>();
            unPasto.Scale = new Vector3(unPasto.Scale.X,0.25f,unPasto.Scale.Z);

            for (int i = -1000; i < 1000; i+=10)
            {
                for (int j = -1000; j < 1000; j += 10)
                {
                    TgcMesh otroPasto = unPasto.clone("");
                    otroPasto.Position = new Vector3(i,0,j);
                    pasto.Add(otroPasto);
                }
            }
            return pasto;
        
        }
    }
}
