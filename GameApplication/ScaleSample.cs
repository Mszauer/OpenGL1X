using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class ScaleSample : Game {
        Grid grid = new Grid();
        public override void Render() {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Rotate(90.0f, 1.0f, 0.0f, 0.0f);
            GL.Rotate(180.0f, 0.0f, 0.0f, 1.0f); //to see the scaling
            //GL.Scale(2.0, 2.0, 2.0);
            GL.Scale(0.25, 0.25, 0.25);
            grid.Render();
        }
    }
}
