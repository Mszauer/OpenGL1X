using System;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class LookAtSample : Game {
        Grid grid = null;

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

            float[] result = {
                s[0], u[0], -f[0], -eyeX,
                s[1], u[1], -f[1], -eyeY,
                s[2], u[2], -f[2],  -eyeZ,
                0.0f, 0.0f, 0.0f, 1.0f
            };
            GL.MultMatrix(result);
        }

        public override void Initialize() {
            grid = new Grid();
        }

        public override void Update(float dTime) {

        }
        public override void Render() {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity(); // Reset modelview matrix
            LookAt(0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);

            grid.Render();
        }
        public override void Shutdown() {

        }
    }
}