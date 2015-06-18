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
using TgcViewer.Utils.Particles;

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
        List<Objeto> objetosMapa;
        List<TgcBox> bordes = new List<TgcBox>();
        Microsoft.DirectX.Direct3D.Effect efecto;
        List<Celda> celdas = new List<Celda>();
        List<Objeto> objetosColisionables = new List<Objeto>();
        Mapa mapa;
        bool collide = false;
        TgcBoundingBox camaraColision;
        List<Enemigo> enemigosMuertos = new List<Enemigo>();
        Nieve nieve;
        ClimaNieve clima;
        float tiempo;
        Interfaz interfaz;
        CamaraSniper camaraMenu;

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
            //GuiController.Instance.FpsCounterEnable = true;

            //Nieve
            GuiController.Instance.Modifiers.addBoolean("nieve", "Mostrar nieve", true);

            //***** Inicializar Camara ******//
            camara = new CamaraSniper();
            camara.Enable = true;
            camara.MovementSpeed = 50f;
            camara.JumpSpeed = 0;
            camara.setCamera(new Vector3(0, 10, 0), new Vector3(5, 5, 5));
            camara.RotateMouseButton = TgcD3dInput.MouseButtons.BUTTON_MIDDLE;
            camara.updateCamera();

            GuiController.Instance.CurrentCamera = new TgcRotationalCamera();
            interfaz = new Interfaz(camara, camaraMenu);



            camaraColision = new TgcBoundingBox(new Vector3(-2, 0, -2), new Vector3(-2, 0, -2) + (new Vector3(4, 15, 4)));

            //**** Inicializar Nivel ******//
            mapa = new Mapa();
            //Piso
            piso = Mapa.nuevoPiso(new Vector2(2000, 2000), "pasto");
            //Skybox
            skyBox = new SkyBoxSniper(10000, "Mar");
            //Bordes
            bordes = Mapa.crearBordes();
            //Arma
            arma = new Arma("counter", "mira");
            //Personaje
            personaje = new Personaje(arma, camaraColision,interfaz);
            //Enemigos
            enemigos = Mapa.crearEnemigos(cantidadEnemigos, personaje, "Robot");
            //Vegetacion
            objetosMapa = mapa.crearObjetosMapa(enemigos,500,250,100,personaje);
            //nieve
            nieve = new Nieve(3000, 3000, 200);
            clima = new ClimaNieve(nieve);

            

            //***** Cursor *****//
            Cursor.Hide();


            objetosColisionables = colisionables();

            //******* Inicializar grilla *******//
            crearGrilla();



            //Cargar Shader personalizado
            efecto = TgcShaders.loadEffect(GuiController.Instance.AlumnoEjemplosMediaDir + "BasicShader.fx");


            piso.Effect = efecto;
            piso.Technique = "RenderScene";

            tiempo = 0;


            

        }



        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {

            if (interfaz.enTitulo)
            {
                interfaz.mostrarTitulo();

                //Actualizacion velocidad viento cada 40 segundos
                mapa.actualizar(objetosMapa, celdas);
                //***** Renders ******//
                nieve.renderNieve(elapsedTime);
                clima.alternarClima();
                piso.render();
                skyBox.render();
                renderizarObjetos(elapsedTime);
            }
            else if (interfaz.enControles)
            {
                interfaz.mostrarControles();

                //Actualizacion velocidad viento cada 40 segundos
                mapa.actualizar(objetosMapa, celdas);
                //***** Renders ******//
                nieve.renderNieve(elapsedTime);
                clima.alternarClima();
                piso.render();
                skyBox.render();
                renderizarObjetos(elapsedTime);
            }
            else
            {
                //***** Asignaciones ******//
                Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

                // tiempo += elapsedTime;
                d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);

                // Cargar variables de shader, por ejemplo el tiempo transcurrido.
                efecto.SetValue("time", tiempo);


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
                        arma.disparar(personaje, enemigos, objetosColisionables);
                    }
                    ultimoTiro = System.DateTime.Now.TimeOfDay.TotalMilliseconds;

                }
                //Zoom
                else if (input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_RIGHT))
                {
                    if (System.DateTime.Now.TimeOfDay.TotalMilliseconds - ultimoZoom > 500)
                    {
                        arma.hacerZoom();
                        ultimoZoom = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
                    }
                }

                //Actualizacion velocidad viento cada 40 segundos
                mapa.actualizar(objetosMapa, celdas);

                //***** Renders ******//
                nieve.renderNieve(elapsedTime);
                clima.alternarClima();
                piso.render();
                skyBox.render();
                personaje.actualizar();

                renderizarObjetos(elapsedTime);

                enemigosMuertos.Clear();
                foreach (Enemigo enemigo in enemigos)
                {
                    enemigo.actualizar(elapsedTime, personaje, objetosColisionables, colisionaConFrustum(enemigo.enemigo.BoundingBox));
                    if (enemigo.murio)
                    {
                        enemigosMuertos.Add(enemigo);
                    }
                }
                foreach (Enemigo enemigo in enemigosMuertos)
                {
                    enemigos.Remove(enemigo);
                }



                foreach (TgcBox borde in bordes)
                {
                    borde.render();
                }




                //Detectar colisiones de BoundingBox utilizando herramienta TgcCollisionUtils


                foreach (Objeto objeto in objetosColisionables)
                {
                    TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(camaraColision, objeto.colisionFisica);
                    if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                    {
                        collide = true;

                    }

                }

                //Si hubo colision, restaurar la posicion anterior
                if (collide)
                {
                    Vector3 direccion = personaje.direccionEnLaQueMira();
                    camara.moveLaCamara(direccion);
                    collide = false;
                }

                arma.actualizar();


                //d3dDevice.EndScene();
                //GuiController.Instance.CurrentCamera = camaraAnterior;

                if (interfaz.finJuego)
                {
                    if (interfaz.gano)
                    {
                        interfaz.ganoJuego();
                    }
                    else
                    {
                        interfaz.perdioJuego();
                    }
                    
                }

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
            enemigos.Clear();
            objetosMapa.Clear();
        }

        private bool colisionaConFrustum(TgcBoundingBox objeto)
        {
            TgcFrustum frustum = GuiController.Instance.Frustum;
            TgcCollisionUtils.FrustumResult resultado = TgcCollisionUtils.classifyFrustumAABB(frustum, objeto);
            return resultado == TgcCollisionUtils.FrustumResult.INSIDE || resultado == TgcCollisionUtils.FrustumResult.INTERSECT;
        }

        private void renderizarObjetos(float elapsedTime)
        {
            List<Objeto> yaRenderizados = new List<Objeto>();
            foreach(Celda celda in celdas){
                if (colisionaConFrustum(celda.cajaColision))
                {
                    foreach (Objeto objeto in celda.objetos)
                    {
                        if (!yaRenderizados.Contains(objeto))
                        {
                            if (colisionaConFrustum(objeto.mesh.BoundingBox))
                            {
                                if (((objeto.mesh.Position - personaje.posicion()).Length()) < 1000)
                                {
                                    objeto.render(elapsedTime, mapa.velocidadViento);
                                    yaRenderizados.Add(objeto);
                                }
                                
                            }
                        }
                        
                    }
                }
                
            }
            yaRenderizados.Clear();
        }

        private void crearGrilla()
        {
            int valor = 8;
            float desplazamiento = 2000 / valor;
            Vector2 inicial = new Vector2(-1000,-1000);
            for (int i = 0; i < valor; i++)
            {
                for (int j = 0; j < valor; j++)
                {
                    Celda estaCelda;
                    celdas.Add(estaCelda = new Celda(new Vector3(i * desplazamiento + inicial.X, 0, j * desplazamiento + inicial.Y), desplazamiento));
                    foreach (Objeto objeto in objetosMapa)
                    {
                        TgcCollisionUtils.BoxBoxResult resultado = (TgcCollisionUtils.classifyBoxBox(objeto.mesh.BoundingBox, estaCelda.cajaColision));
                        if (resultado == TgcCollisionUtils.BoxBoxResult.Adentro || resultado == TgcCollisionUtils.BoxBoxResult.Atravesando)
                        {
                            estaCelda.agregarObjeto(objeto);
                        }
                    }
                }
            }
        }

        private List<Objeto> colisionables()
        {
            return objetosMapa.FindAll(esColisionable);
        }

        private bool esColisionable(Objeto objeto)
        {
            return objeto.colisionable;
        }

    }
}
