using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace AlumnoEjemplos.MiGrupo
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploAlumno : TgcExample
    {
        Arma arma;
        TgcBox piso;
        TgcMesh arbolOriginal;
        SkyBoxSniper skyBox;
        TgcFpsCamera camara;
        TgcBox bala;
        List<TgcMesh> meshes;
        Vector3 direccionBala;
        int nBalas = 0;
        struct dataBala
        {
            public TgcBox bala;
            public Vector3 direccionBala;
        };
        List<dataBala> balas = new List<dataBala>();
        /// <summary>
        /// Categoría a la que pertenece el ejemplo.
        /// Influye en donde se va a haber en el árbol de la derecha de la pantalla.
        /// </summary>
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "BEFS";
        }

        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return "Sniper";
        }

        /// <summary>
        /// Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        /// Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void init()
        {   
            //Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcSceneLoader loader = new TgcSceneLoader();
            
            CamaraSniper.nuevaCamara(camara, 5, 50);

            piso = Mapa.nuevoPiso(new Vector2(1000, 1000), "pasto");

            skyBox = new SkyBoxSniper(10000, "Mar");

            arma = new Arma("counter");

            //Cargar modelo del arbol original
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pino\\Pino-TgcScene.xml");
            arbolOriginal = scene.Meshes[0];

            //Crear varias instancias del modelo original, pero sin volver a cargar el modelo entero cada vez
            int rows = 5;
            int cols = 6;
            float offset = 100;
            meshes = new List<TgcMesh>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    //Crear instancia de modelo
                    TgcMesh instance = arbolOriginal.createMeshInstance(arbolOriginal.Name + i + "_" + j);

                    //Desplazarlo
                    instance.move(i * offset, 0, j * offset);

                    meshes.Add(instance);
                }
            }
           // Cursor.Hide();

           bala = TgcBox.fromSize(new Vector3(0,0,0), new Vector3(1f, 1f, 1f), Color.Red);
            

        }


        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {
            
            GuiController.Instance.Drawer2D.beginDrawSprite();
            arma.armaSprite.render();
            GuiController.Instance.Drawer2D.endDrawSprite();
            piso.render();
            skyBox.render();

            //Renderizar instancias
            foreach (TgcMesh mesh in meshes)
            {
                mesh.render();
            }
           
            TgcD3dInput input = GuiController.Instance.D3dInput;
            if (input.keyDown(Key.E))
            {

                
                dataBala datosDeBala;
                datosDeBala.bala = bala.clone();
                datosDeBala.bala.setPositionSize(GuiController.Instance.FpsCamera.Position, bala.Size);
                datosDeBala.direccionBala = GuiController.Instance.FpsCamera.getLookAt() - GuiController.Instance.FpsCamera.getPosition(); 
                balas.Add(datosDeBala);
                nBalas++;
            }
            foreach(dataBala datosBala in balas)
            {
                datosBala.bala.move(datosBala.direccionBala * elapsedTime * 50);
                datosBala.bala.render();
            }

            
        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
        {

            piso.dispose();
            skyBox.dispose();
            arbolOriginal.dispose();
        }

    }
}
