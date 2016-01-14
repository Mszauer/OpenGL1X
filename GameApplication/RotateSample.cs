using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace GameApplication {
    class RotateSample : Game{
        Grid grid = new Grid();
        public override void Render() {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Rotate(90f, 1.0f, 0.0f, 0.0f);
            GL.Rotate(180.0f,0.0f,0.0f,1.0f);
            grid.Render();
        }
    }
}
