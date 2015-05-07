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
        CamaraSniper camara;
        TgcBox bala;
        List<TgcMesh> meshes;
        Vector3 direccionBala;
        int nBalas = 0;
        struct dataBala
        {
            public TgcBox bala;
            public Vector3 direccionBala;
        };
        Double ultimoTiro = 0;
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

            camara = new CamaraSniper();
            camara.Enable = true;
            camara.MovementSpeed = 50f;
            camara.JumpSpeed = 0;
            camara.setCamera(new Vector3(0, 5, 0), new Vector3(1, 0, 0));
            GuiController.Instance.CurrentCamera = camara;

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
           Cursor.Hide();
           

           bala = TgcBox.fromSize(new Vector3(0,0,0), new Vector3(1f, 1f, 1f), Color.Red);

           GuiController.Instance.FpsCamera.RotateMouseButton = TgcD3dInput.MouseButtons.BUTTON_MIDDLE;
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
            if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                if (System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoTiro > 500)
                {
                    ultimoTiro = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
                    dataBala datosDeBala;
                    datosDeBala.bala = bala.clone();
                    datosDeBala.bala.setPositionSize(GuiController.Instance.CurrentCamera.getPosition(), bala.Size);
                    datosDeBala.direccionBala = GuiController.Instance.CurrentCamera.getLookAt() - GuiController.Instance.CurrentCamera.getPosition();
                    balas.Add(datosDeBala);
                    nBalas++;
                }
               /*rotacion arma con camara
                * tmplookAt = camera.getLookAt() - camera.getPosition();
                tmpRotationY = MathUtil.getDegree(tmplookAt.X, tmplookAt.Z) + initRotation.X;
                weaponDrawing.setRotationY(tmpRotationY);
                tmpRotationXZ = Vector3.Cross(tmplookAt, axisY);
                weaponDrawing.setRotationXZ(tmpRotationXZ, initRotation.Y - (float)Math.Acos(Vector3.Dot(tmplookAt, axisY)));*/
                
            }
            foreach(dataBala datosBala in balas)
            {
                datosBala.bala.move(datosBala.direccionBala * elapsedTime * 1000);
                datosBala.bala.render();
            }
            Control focusWindows = GuiController.Instance.D3dDevice.CreationParameters.FocusWindow;
            Cursor.Position = focusWindows.PointToScreen(new Point(focusWindows.Width / 2, focusWindows.Height / 2));


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
