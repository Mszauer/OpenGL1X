using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class Shaded_triangle : Game{
        Grid grid = null;

        public Shaded_triangle() {
            grid = new Grid();
        }
        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 30.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(lookAt.OpenGL);
            grid.Render();

            GL.ShadeModel(ShadingModel.Smooth);

            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(-10.0f, -10.0f, -5.0f);//red
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(20.0f, -10.0f, -5.0f);//green
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(-10.0f, 20.0f, -5.0f);//blue
            GL.End();
        }
        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            //set projection matrix
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)MainGameWindow.Window.Width / (float)MainGameWindow.Window.Height;
            Matrix4 perspective = Matrix4.Perspective(60, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            //switch to view matrix
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}
