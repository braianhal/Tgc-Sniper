using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Sound;

namespace AlumnoEjemplos.SRC.Renderman
{
    public class SoundManager
    {
        public TgcStaticSound sonidoMunicion;
        //public TgcStaticSound sonidoCaminandoIzq, sonidoCaminandoDer;
        public TgcStaticSound sonidoPasoEnemigo;
        public TgcStaticSound sonidoEnemMuerto;
        public TgcStaticSound sonidoDisparo;
        public TgcStaticSound sonidoRecarga;
        public TgcStaticSound sonidoEnemigoAlcanzaPersonaje;
        public TgcStaticSound sonidoAviso;
        public TgcStaticSound sonidoFin;
        private Boolean esPasoIzquierdo;
        private TgcStaticSound sinMunicion;
        private TgcStaticSound explosion;
        private TgcStaticSound headshot;
        private TgcStaticSound vientoLigero;
        private TgcStaticSound vientoFuerte;

        private TgcStaticSound pasoIzq;
        private TgcStaticSound pasoDer;

        private DateTime tiempoCordinacionCaminar;
        private TgcStaticSound sonidoBackground;

        public static SoundManager Instance;

        public static SoundManager getInstance()
        {
            if (Instance == null)
            {
                Instance = new SoundManager();
            }
            return Instance;
        }

        public SoundManager()
        {


            pasoIzq = new TgcStaticSound();
            pasoDer = new TgcStaticSound();
            tiempoCordinacionCaminar = DateTime.Now;

            sonidoDisparo = new TgcStaticSound();
            sonidoRecarga = new TgcStaticSound();
            sonidoPasoEnemigo = new TgcStaticSound();
            sonidoEnemMuerto = new TgcStaticSound();
            sonidoEnemigoAlcanzaPersonaje = new TgcStaticSound();
            sonidoBackground = new TgcStaticSound();
            sonidoMunicion = new TgcStaticSound();
            sonidoAviso = new TgcStaticSound();
            sonidoFin = new TgcStaticSound();
            sinMunicion = new TgcStaticSound();
            explosion = new TgcStaticSound();
            headshot = new TgcStaticSound();
            vientoLigero = new TgcStaticSound();
            vientoFuerte = new TgcStaticSound();


            sonidoDisparo.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Sounds\\sniper1.wav");
            sonidoBackground.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Sounds\\ambiente.wav");
            pasoIzq.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Sounds\\pasoIzq.wav");
            pasoDer.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Sounds\\pasoDer.wav");
            explosion.loadSound(GuiController.Instance.AlumnoEjemplosMediaDir + "Sounds\\explosionBarril.wav");

            
            esPasoIzquierdo = true;
        }


        public void sonidoCaminando()
        {
            DateTime tiempoSonido = DateTime.Now;

            // se pone esto con el objetivo de que no se pisen los sonidos, sino que antes de ejecutarse espere a que el otro sonido(el del otro paso) termine
            if ((tiempoSonido.Millisecond - tiempoCordinacionCaminar.Millisecond) > 500 || (tiempoSonido.Second != tiempoCordinacionCaminar.Second))
            {
                if (esPasoIzquierdo)
                {
                    pasoIzq.play();
                    esPasoIzquierdo = false;

                }
                else
                {
                    pasoDer.play();
                    esPasoIzquierdo = true;
                }

                tiempoCordinacionCaminar = DateTime.Now;
            }
        }

        public void playVentizcaFuerte()
        {
            this.vientoLigero.stop();
            this.vientoFuerte.play();
        }

        public void playVentizcaLigera()
        {
            this.vientoFuerte.stop();
            this.vientoLigero.play();
        }

        public void playSonidoSinMunicion()
        {
            sinMunicion.play();
        }

        public void playSonidoAmbiente()
        {
            sonidoBackground.play();
        }

        public void sonidoCaminandoEnemigo()
        {
            sonidoPasoEnemigo.play();
        }

        public void sonidoEnemigoMuerto()
        {
            sonidoEnemMuerto.play();
        }

        public void playSonidoRecarga()
        {
            sonidoRecarga.play();
        }

        public void playSonidoDisparo()
        {
            sonidoDisparo.play();
        }

        public void playSonidoJugadorAlcanzado()
        {
            sonidoEnemigoAlcanzaPersonaje.play();
        }

        public void playSonidoMunicion()
        {
            sonidoMunicion.play();
        }

        public void playSonidoAviso()
        {
            sonidoAviso.play();
        }

        public void playSonidoFin()
        {
            //sonidoFin.play();
        }

        public void playSonidoExplosion()
        {
            explosion.play();
        }

        public void playHeadShot()
        {
            headshot.play();
        }

        public void stopSonidoFin()
        {
            sonidoFin.dispose();
        }
        public void dispose()
        {
            sonidoPasoEnemigo.dispose();
            sonidoEnemMuerto.dispose();
            sonidoDisparo.dispose();
            sonidoRecarga.dispose();
            sonidoEnemigoAlcanzaPersonaje.dispose();
            sonidoMunicion.dispose();
            sonidoAviso.dispose();
            sinMunicion.dispose();
            //sonidoFin.dispose();
            vientoLigero.dispose();
            vientoFuerte.dispose();
            sonidoBackground.dispose();

            pasoIzq.dispose();
            pasoDer.dispose();

        }

    }
}


