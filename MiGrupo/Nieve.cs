using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo
{
    class Nieve
    {

        private List<CopoNieve> listaCopos;
        private float nubeMaxX;
        private float nubeMaxZ;
        private float nubeMinX;
        private float nubeMinZ;
        private float anchoX;
        private float anchoZ;

        private int variacionVelCaida;
        private int variacionViento;

        public Nieve(int largoX, int largoZ, int cantidadCopos)
        {
            //Modifiers para variar los parametros de la tormenta
            GuiController.Instance.Modifiers.addInt("Viento", 0, 400, 150);
            GuiController.Instance.Modifiers.addInt("Velocidad caida Nieve", 0, 400, 145);

            //parametros que los maneja la clase clima para variar la tormenta
            variacionVelCaida = 0;
            variacionViento = 0;

            listaCopos = new List<CopoNieve>();

            Vector3 camaraPos = GuiController.Instance.CurrentCamera.getPosition();

            //seteo el tamaño de la nube
            Vector3 camara = GuiController.Instance.CurrentCamera.getPosition();
            anchoX = largoX;
            anchoZ = largoZ;

            nubeMaxX = camara.X + (anchoX / 2);
            nubeMinX = camara.X - (anchoX / 2);
            nubeMaxZ = camara.Z + (anchoZ / 2);
            nubeMinZ = camara.Z - (anchoZ / 2);

            //cargo los copos
            for (int i = 0; i < cantidadCopos; i++)
            {
                //genero una posicion random dentro de la nube
                Random posicionX = new Random(i);
                int posX = posicionX.Next((int)nubeMinX, (int)nubeMaxX);
                Random posicionZ = new Random(i);
                int posZ = posicionX.Next((int)nubeMinZ, (int)nubeMaxZ);

                listaCopos.Add(new CopoNieve(new Vector3(camaraPos.X + posX, 1000, camaraPos.Z + posZ), i % 4, i, this));
            }
        }

        public void cambiarParametrosClima(int velocidaCaida, int viento)
        {
            this.variacionVelCaida = velocidaCaida;
            this.variacionViento = viento;
        }

        //metodo que sirve para compensar el movimiento de los copos por el viento en la tormenta y no se vallan muy lejos del jugador
        public void moverNube(int corrimiento)
        {
            this.nubeMinX = nubeMinX + corrimiento;
            this.nubeMaxX = nubeMaxX + corrimiento;
        }

        public int obtenerVelCaidaMod()
        {
            return (int)GuiController.Instance.Modifiers["Velocidad caida Nieve"];
        }

        public int obtenerVelViento()
        {
            return (int)GuiController.Instance.Modifiers["Viento"];
        }

        public void renderNieve(float elapseTime)
        {
            if (!(Boolean)GuiController.Instance.Modifiers.getValue("nieve"))
            {
                return;
            }

            int i = 1;
            foreach (CopoNieve bola in listaCopos)
            {
                bola.cae(elapseTime, nubeMinX, nubeMaxX, nubeMinZ, nubeMaxZ, i, variacionViento, variacionVelCaida);
                bola.renderBola();
                i++;
            }
        }

        public void disposeNieve()
        {
            foreach (CopoNieve bola in listaCopos)
            {
                bola.disposeBola();
            }
        }

    }
}
