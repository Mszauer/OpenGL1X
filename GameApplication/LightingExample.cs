using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class LightingExample : Game{
        protected Grid grid = null;
        protected Vector3 cameraAngle = new Vector3(0.0f, -25.0f, 10.0f);
        protected float rads = (float)(Math.PI / 180.0f);

        public override void Initialize() {
            grid = new Grid(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            Resize(MainGameWindow.Window.Width, MainGameWindow.Window.Height);
        }
        public override void Update(float dTime) {
            cameraAngle.X += 30.0f * dTime;
        }
        public override void Render() {
            Vector3 eyePos = new Vector3();
            eyePos.X = cameraAngle.Z * -(float)Math.Sin(cameraAngle.X * rads * (float)Math.Cos(cameraAngle.Y * rads));
            eyePos.Y = cameraAngle.Z * -(float)Math.Sin(cameraAngle.Y * rads);
            eyePos.Z = -cameraAngle.Z * (float)Math.Cos(cameraAngle.X * rads * (float)Math.Cos(cameraAngle.Y * rads));

            Matrix4 lookAt = Matrix4.LookAt(eyePos, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            grid.Render();

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.PushMatrix();
            {
                GL.Translate(0.0f, 2.5f, -2.0f);
                Primitives.Torus();
            }
            GL.PopMatrix();

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.PushMatrix();
            {
                GL.Translate(2.5f, 1.0f, -0.5f);
                Primitives.DrawSphere();
            }
            GL.PopMatrix();

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.PushMatrix();
            {
                GL.Translate(-1.0f, 0.5f, 0.5f);
                Primitives.Cube();
            }
            GL.PopMatrix();
        }
        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            //set projection matrix
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;

            Matrix4 perspective = Matrix4.Perspective(60, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            //switch to view matrix
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}
