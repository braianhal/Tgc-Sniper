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
        Personaje personaje;
        Arma arma;
        TgcBox piso;
        TgcMesh arbolOriginal;
        SkyBoxSniper skyBox;
        CamaraSniper camara;
        TgcBox bala;
        Vector3 direccionBala;
        List<Enemigo> enemigos = new List<Enemigo>();
        int cantidadEnemigos = 20;
        int nBalas = 0;
        public struct dataBala
        {
            public TgcBox bala;
            public Vector3 direccionBala;
        };
        Double ultimoTiro = 0;
        List<dataBala> balas = new List<dataBala>();
        
        public List<TgcMesh> meshes;
        TgcScene[] meshesVegetacion = new TgcScene[4];
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

            //Enemigos
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "Modelos\\Robot\\Robot-TgcScene.xml");
            TgcMesh enemigo = scene.Meshes[0];
            enemigo.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            camara = new CamaraSniper();
            camara.Enable = true;
            camara.MovementSpeed = 50f;
            camara.JumpSpeed = 0;
            camara.setCamera(new Vector3(0, 10, 0), new Vector3(1, 0, 0));
            GuiController.Instance.CurrentCamera = camara;

            piso = Mapa.nuevoPiso(new Vector2(2000, 2000), "pasto");

            skyBox = new SkyBoxSniper(10000, "Mar");

            arma = new Arma("counter","mira");

             Cursor.Hide();
           

           bala = TgcBox.fromSize(new Vector3(0,0,0), new Vector3(1f, 1f, 1f), Color.Red);

           //Creando enemigos
           for (int i = 0; i < cantidadEnemigos; i++)
           {
               float xAleatorio =  (float)(Randomizar.Instance.NextDouble() * 2000-1000);
               float yAleatorio =  (float)(Randomizar.Instance.NextDouble() * 2000-1000);
               enemigos.Add(new Enemigo(new Vector3(xAleatorio, 0, yAleatorio),enemigo));
           }

            
            
            GuiController.Instance.FpsCamera.RotateMouseButton = TgcD3dInput.MouseButtons.BUTTON_MIDDLE;

            personaje = new Personaje(arma);

            //Cargar modelo de vegetacion
            meshesVegetacion[0] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Planta\\Planta-TgcScene.xml");            
            meshesVegetacion[1] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera3\\Palmera3-TgcScene.xml");
            meshesVegetacion[2] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Roca\\Roca-TgcScene.xml");            
            meshesVegetacion[3] = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pasto\\Pasto-TgcScene.xml");
            createVegetation();


            Vector3 lastPos = enemigo.Position;
            bool collide = false;
            foreach (Enemigo unEnemigo in enemigos)
            {
                TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(unEnemigo.enemigo.BoundingBox, enemigo.BoundingBox);
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



        }


        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {

            arma.actualizar();
            piso.render();
            skyBox.render();

            foreach (TgcMesh planta in meshes)
            {
                planta.render();
            }

           
            TgcD3dInput input = GuiController.Instance.D3dInput;
            if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT) && !personaje.muerto())
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

            foreach (Enemigo enemigo in enemigos)
            {
                enemigo.actualizar(GuiController.Instance.CurrentCamera.getPosition(), elapsedTime, balas,personaje,meshes);
            }
            enemigos.RemoveAll(enemigoElminable);
            balas.RemoveAll(estaLejos);
           /* if (TgcCollisionUtils.testSphereSphere(boundingBall, collisionableList[j].boundingBall))
            {
                colisionador.cantColisiones--;
                vecAux = colisionador.direction;
                colisionador.direction = collisionableList[j].direction;
                collisionableList[j].direction = vecAux;
            }*/
            personaje.actualizar();


        }

        


        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
        {
            piso.dispose();
            skyBox.dispose();
        }


        private bool enemigoElminable(Enemigo enemigo)
        {
            return enemigo.eliminado;
        }

        private bool estaLejos(dataBala bala)
        {   
            float distanciaAPersonaje = (bala.bala.Position - GuiController.Instance.CurrentCamera.getPosition()).Length();
            return distanciaAPersonaje > 3000;
        }

        private void createVegetation()
        {
            meshes = new List<TgcMesh>();
            for (int i = 0; i < 500; i++)
            { //Se puede aumentar el numero pero si no se utiliza la grilla regular decae mucho la performance
                int numero = (int)(Randomizar.Instance.NextDouble() * 4);
                TgcMesh planta = (TgcMesh)meshesVegetacion[numero].Meshes[0].clone("");
                planta.move(new Vector3(-1000 + (float)Randomizar.Instance.NextDouble() * 2000, 0, -1000 + (float)Randomizar.Instance.NextDouble() * 2000));

                if (numero == 1)
                {
                    planta.BoundingBox.scaleTranslate(planta.Position, new Vector3(0.04f, 1, 0.04f));
                }

                if (numero == 2)
                {
                    planta.BoundingBox.scaleTranslate(planta.Position, new Vector3(0.8f, 1, 0.8f));
                }

                if (numero == 3)
                {
                    planta.BoundingBox.scaleTranslate(planta.Position, new Vector3(0f, 0, 0f));
                }

                if (numero == 0)
                {
                    planta.BoundingBox.scaleTranslate(planta.Position, new Vector3(0.01f, 1, 0.01f));
                }


                meshes.Add(planta);

            }
        }

    }
}
