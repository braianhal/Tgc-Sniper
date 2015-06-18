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
        float maximaVelocidadViento = 0.2f;
        List<Barril> barriles = new List<Barril>();
        ParticleEmitter nieve;

        public Mapa()
        {
            ultimoCambioViento = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
            nieve = new ParticleEmitter(GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\nieve.png", 100000);
            nieve.Position = new Vector3(0, 5, 0);
        }

        public static TgcBox nuevoPiso(Vector2 tamanio,string textura)
        {
            /*string texturePath = GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\" + textura + ".jpg";
            TgcTexture texturaPiso = TgcTexture.createTexture(GuiController.Instance.D3dDevice, texturePath);*/
            TgcBox nuevoPiso = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(tamanio.X, 0.1f, tamanio.Y));
            nuevoPiso.UVTiling = new Vector2(50, 50);
            nuevoPiso.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\nieve.png"));
            nuevoPiso.updateValues();
            return nuevoPiso;
        }


        public List<Objeto> crearObjetosMapa(List<Enemigo> enemigos,int cantidadArboles,int cantidadPasto,int cantidadBarriles,Personaje personaje)
        {

            TgcSceneLoader loader = new TgcSceneLoader();
            List<Objeto> objetos = new List<Objeto>();
            TgcMesh arbol = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pino\\Pino-TgcScene.xml").Meshes[0];
            TgcMesh pasto = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Arbusto\\Arbusto-TgcScene.xml").Meshes[0];
            TgcMesh barril = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "Modelos\\Barril\\BarrilPolvora-TgcScene.xml").Meshes[0];

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

            barril.Scale = new Vector3(0.25f, 0.25f, 0.25f);
            for (int i = 0; i < cantidadBarriles; i++)
            {
                Barril nuevoBarril;
                Vector3 moverA = new Vector3(-1000 + (float)Randomizar.Instance.NextDouble() * 2000, 0, -1000 + (float)Randomizar.Instance.NextDouble() * 2000);
                objetos.Add(nuevoBarril = new Barril(moverA, barril.clone(""),personaje));
                barriles.Add(nuevoBarril);
            }

            return objetos;
        }

        public static List<Enemigo> crearEnemigos(int cantidadEnemigos,Personaje personaje, string tipo)
        {

            float xAleatorio ;
            float yAleatorio;


            List<Enemigo> enemigos = new List<Enemigo>();
            

            for (int i = 0; i < cantidadEnemigos-1; i++)
            {
                xAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                yAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                while (xAleatorio < 50 || yAleatorio < 50)
                {
                    xAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                    yAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                }
                enemigos.Add(new Enemigo(new Vector3(xAleatorio, 0, yAleatorio),  personaje,false));
            }
            xAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
            yAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
            while (xAleatorio < 50 || yAleatorio < 50)
            {
                xAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
                yAleatorio = (float)(Randomizar.Instance.NextDouble() * 2000 - 1000);
            }
            enemigos.Add(new Enemigo(new Vector3(xAleatorio, 0, yAleatorio), personaje, true));
            return enemigos;
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

        public void actualizar(List<Objeto> objetosMapa,List<Celda> celdas)
        {
            if ((System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoCambioViento)> tasaCambioViento)
            {
                velocidadViento =  (float)Randomizar.Instance.NextDouble() * maximaVelocidadViento;
                ultimoCambioViento = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
            foreach (Barril barril in barriles)
            {
                if (barril.destruir)
                {
                    objetosMapa.Remove(barril);
                    foreach (Celda celda in celdas)
                    {
                        if (celda.objetos.Contains(barril))
                        {
                            celda.objetos.Remove(barril);
                        }
                    }
                }
                
            }
            barriles.RemoveAll(aDestruir);
        }

        private bool aDestruir(Barril barril)
        {
            return barril.destruir;
        }


    }
}
