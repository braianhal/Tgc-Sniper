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
using TgcViewer.Utils;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils._2D;

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
        SkyBoxSniper skyBox;
        CamaraSniper camara;
        TgcBox bala;
        List<Enemigo> enemigos = new List<Enemigo>();
        int cantidadEnemigos = 20;
        int nBalas = 0;
        Double ultimoTiro = 0;
        Double ultimoZoom = 0;
        public List<TgcMesh> objetosMapa;
        bool optimizacion = false;
        int limiteRenderizado = 500;
        List<TgcBox> bordes = new List<TgcBox>();
        Microsoft.DirectX.Direct3D.Effect efecto;
        int nivelesGrilla = 3;
        List<Celda> celdas = new List<Celda>();
        List<TgcMesh> pasto = new List<TgcMesh>();

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
            //**** Asignaciones *****//
            TgcSceneLoader loader = new TgcSceneLoader();
            //GuiController.Instance.CustomRenderEnabled = true;
            GuiController.Instance.FpsCounterEnable = true;
            

            //Cargar shader de este ejemplo
            //efecto = TgcShaders.loadEffect(GuiController.Instance.ExamplesMediaDir + "Shaders\\EjemploGetZBuffer.fx");


            //**** Inicializar Nivel ******//
            //Piso
            piso = Mapa.nuevoPiso(new Vector2(2000, 2000), "pasto");
            //Skybox
            skyBox = new SkyBoxSniper(10000, "Mar");
            //Bordes
            bordes = Mapa.crearBordes();
            //Personaje
            personaje = new Personaje(arma);
            //Arma
            arma = new Arma("counter", "mira");
            //Enemigos
            enemigos = Mapa.crearEnemigos(cantidadEnemigos,personaje);
            //Vegetacion
            objetosMapa = Mapa.crearObjetosMapa(enemigos);
            pasto = Mapa.crearPasto();


            //***** Inicializar Camara ******//
            camara = new CamaraSniper();
            camara.Enable = true;
            camara.MovementSpeed = 50f;
            camara.JumpSpeed = 0;
            camara.setCamera(new Vector3(0, 10, 0), new Vector3(5, 5, 5));
            camara.RotateMouseButton = TgcD3dInput.MouseButtons.BUTTON_MIDDLE;
            camara.updateCamera();
            GuiController.Instance.CurrentCamera = camara;

            //***** Cursor *****//
            Cursor.Hide();


            //******* Inicializar grilla *******//
            crearGrilla();


            ///agregado
            /*foreach (Enemigo enemigo in enemigos)
            {
                enemigo.enemigo.Effect = efecto;
            }
            foreach (TgcMesh objeto in objetosMapa)
            {
                objeto.Effect = efecto;
            }
            piso.Effect = efecto;*/
            
        }



        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {
            //***** Asignaciones ******//
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            
            //****** Render de mapa ********//
            //d3dDevice.BeginScene();

            //***** Mouse *****//
            //Puntero en el centro de la pantalla
            Control focusWindows = GuiController.Instance.D3dDevice.CreationParameters.FocusWindow;
            Cursor.Position = focusWindows.PointToScreen(new Point(focusWindows.Width / 2, focusWindows.Height / 2));
            TgcD3dInput input = GuiController.Instance.D3dInput;
            //Disparo
            if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT) && !personaje.muerto())
            {
                if (System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoTiro > 500)
                {
                    arma.disparar(personaje,enemigos,objetosMapa);
                }
                ultimoTiro = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
                
            }
            //Zoom
            else if(input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_RIGHT)){
                if (System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoZoom > 500)
                {
                    arma.hacerZoom();
                    ultimoZoom = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
                }
            }

            //***** Renders ******//
            piso.render();
            skyBox.render();
            personaje.actualizar();

            //quadtree.render(GuiController.Instance.Frustum, false);
            renderizarObjetos();
            foreach (TgcMesh pastoX in pasto)
            {
                if (((pastoX.Position - GuiController.Instance.CurrentCamera.getPosition()).Length()) < 100)
                {
                    pastoX.render();
                }
                
            }


            foreach (Enemigo enemigo in enemigos)
            {
                enemigo.actualizar(elapsedTime, personaje, objetosMapa,colisionaConFrustum(enemigo.enemigo.BoundingBox));
            }
            foreach (TgcBox borde in bordes)
            {
                borde.render();
            }
            //d3dDevice.EndScene();

            //******** Render de arma********//
            //d3dDevice.BeginScene();
            /*CamaraSniper camaraAnterior = (CamaraSniper)GuiController.Instance.CurrentCamera;
            CamaraSniper camaraNueva = new CamaraSniper();
            camaraNueva.Enable = true;
            camaraNueva.setCamera(new Vector3(0, 0, 0), new Vector3(5, 5, 5));
            camaraNueva.Velocity = new Vector3(0,0,0);
            camaraNueva.JumpSpeed = 0f;
            camaraNueva.updateCamera();
            GuiController.Instance.CurrentCamera = camaraNueva;
            
            arma.armaMesh.render();*/
            arma.actualizar();

            

            //d3dDevice.EndScene();
            //GuiController.Instance.CurrentCamera = camaraAnterior;
        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
        {
            piso.dispose();
            skyBox.dispose();
            enemigos.Clear();
            objetosMapa.Clear();
        }

        private bool colisionaConFrustum(TgcBoundingBox objeto)
        {
            TgcFrustum frustum = GuiController.Instance.Frustum;
            TgcCollisionUtils.FrustumResult resultado = TgcCollisionUtils.classifyFrustumAABB(frustum, objeto);
            return resultado == TgcCollisionUtils.FrustumResult.INSIDE || resultado == TgcCollisionUtils.FrustumResult.INTERSECT;
        }

        private void renderizarObjetos()
        {
            foreach(Celda celda in celdas){
                if (colisionaConFrustum(celda.cajaColision))
                {
                    foreach (TgcMesh objeto in celda.objetos)
                    {
                        if (colisionaConFrustum(objeto.BoundingBox))
                        {
                            objeto.render();
                        }
                    }
                }
            }
        }

        /*private List<Celda> crearGrilla(Vector2 puntoInicial, int iteracion,List<Celda> lasCeldas)
        {
            float desplazamiento = (float)Math.Pow(10,iteracion);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector3 posicion = new Vector3(desplazamiento * i + puntoInicial.X, 0, desplazamiento * i + puntoInicial.Y);
                    Celda estaCelda;
                    lasCeldas.Add(estaCelda = new Celda(posicion,desplazamiento));
                    if (iteracion == 1)
                    {
                        foreach (TgcMesh objeto in objetosMapa)
                        {   
                            TgcCollisionUtils.BoxBoxResult resultado = (TgcCollisionUtils.classifyBoxBox(objeto.BoundingBox,estaCelda.cajaColision));
                            if (resultado == TgcCollisionUtils.BoxBoxResult.Adentro || resultado == TgcCollisionUtils.BoxBoxResult.Atravesando)
                            {
                                estaCelda.agregarObjeto(objeto);
                            }
                        }
                    }
                    else
                    {
                        estaCelda.agregarCeldas(crearGrilla(new Vector2(posicion.X, posicion.Z), iteracion - 1, estaCelda.celdas));
                    }
                }
            }
            return lasCeldas;
        }*/

        private void crearGrilla()
        {
            int valor = 8;
            float desplazamiento = 2000 / valor;
            Vector2 inicial = new Vector2(-1000,-1000);
            List<TgcMesh> objetosRestantes = new List<TgcMesh>();
            objetosRestantes = objetosMapa;
            List<TgcMesh> aEliminar = new List<TgcMesh>();
            for (int i = 0; i < valor; i++)
            {
                for (int j = 0; j < valor; j++)
                {
                    Celda estaCelda;
                    celdas.Add(estaCelda = new Celda(new Vector3(i * desplazamiento + inicial.X, 0, j * desplazamiento + inicial.Y), desplazamiento));
                    foreach (TgcMesh objeto in objetosRestantes)
                    {
                        TgcCollisionUtils.BoxBoxResult resultado = (TgcCollisionUtils.classifyBoxBox(objeto.BoundingBox, estaCelda.cajaColision));
                        if (resultado == TgcCollisionUtils.BoxBoxResult.Adentro || resultado == TgcCollisionUtils.BoxBoxResult.Atravesando)
                        {
                            estaCelda.agregarObjeto(objeto);
                            aEliminar.Add(objeto);
                        }
                    }
                    foreach (TgcMesh objeto in aEliminar)
                    {
                        objetosRestantes.Remove(objeto);
                    }
                    aEliminar.Clear();
                }
            }
        }

    }
}
