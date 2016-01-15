using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class BoxDemon : Game {
        Grid grid = null;
        public override void Initialize() {
            base.Initialize();
            grid = new Grid();
        }
        protected static void LookAt(float eyeX, float eyeY, float eyeZ, float targetX, float targetY, float targetZ, float upX, float upY, float upZ) {
            float len = (float)Math.Sqrt(upX * upX + upY * upY + upZ * upZ);
            upX /= len; upY /= len; upZ /= len;

            float[] f = { targetX - eyeX, targetY - eyeY, targetZ - eyeZ };
            len = (float)Math.Sqrt(f[0] * f[0] + f[1] * f[1] + f[2] * f[2]);
            f[0] /= len; f[1] /= len; f[2] /= len;

            float[] s = { 0f, 0f, 0f };
            s[0] = f[1] * upZ - f[2] * upY;
            s[1] = f[2] * upX - f[0] * upZ;
            s[2] = f[0] * upY - f[1] * upX;
            len = (float)Math.Sqrt(s[0] * s[0] + s[1] * s[1] + s[2] * s[2]);
            s[0] /= len; s[1] /= len; s[2] /= len;

            float[] u = { 0f, 0f, 0f };
            u[0] = s[1] * f[2] - s[2] * f[1];
            u[1] = s[2] * f[0] - s[0] * f[2];
            u[2] = s[0] * f[1] - s[1] * f[0];
            len = (float)Math.Sqrt(s[0] * u[0] + u[1] * u[1] + u[2] * u[2]);
            u[0] /= len; u[1] /= len; u[2] /= len;

            float[] result = {s[0], u[0], -f[0], 0.0f,
                              s[1], u[1], -f[1], 0.0f,
                              s[2], u[2], -f[2], 0.0f,
                              0.0f, 0.0f,  0.0f, 1.0f};

            GL.MultMatrix(result);
            GL.Translate(-eyeX, -eyeY, -eyeZ);
        }
        public override void Update(float dTime) {

        }
        public override void Render() {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1, 1, -1, 1, -1, 1);
            LookAt(
                    0.5f, 0.5f, 0.5f,
                    0.0f, 0.0f, 0.0f,
                    0.0f, 1.0f, 0.0f
                );
            GL.MatrixMode(MatrixMode.Modelview);
            grid.Render();
            GL.LoadIdentity();
            GL.Translate(0.0f, 0.0f, -0.25f);
            GL.Translate(0.25f, 0.0f, 0.0f);
            GL.Rotate(45.0f, 1.0f, 0.0f, 0.0f);
            GL.Rotate(73.0f, 0.0f, 1.0f, 0.0f);
            GL.Scale(0.05f, 0.05f, 0.05f);

            GL.Color3(1.0f, 0.0f, 0.0f);
            DrawCube();
        }
        public override void Shutdown() {

        }
        public static void DrawCube() {
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0);
            GL.End();
        }
    }
}
