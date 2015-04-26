using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Input;

namespace AlumnoEjemplos.MiGrupo
{
    static class CamaraSniper
    {
        public static void nuevaCamara(TgcFpsCamera camara, float altura, float velocidadMovimiento)
        {
            camara = GuiController.Instance.FpsCamera;
            camara.Enable = true;
            camara.MovementSpeed = velocidadMovimiento;
            camara.JumpSpeed = 0;
            camara.setCamera(new Vector3(0, altura, 0), new Vector3(1, 0, 0));
        }
    }
}
