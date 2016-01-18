using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math_Implementation;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class SolarSystem : Game{
        Grid grid = null;
        float planetRotDir = 1.0f;
        float moonRotDir = -1.0f;
        float planet1RotSpeed = 15.0f;
        float planet2RotSpeed = 5.0f;
        float moon1RotSpeed = 3.0f;
        float moon2RotSpeed = 3.0f; 

        public SolarSystem() {
            grid = new Grid();
            GL.Enable(EnableCap.DepthTest);
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
        public static void Perspective(float fov, float aspectRatio, float zNear, float zFar) {
            float yMax = zNear * (float)Math.Tan(fov * (Math.PI / 360.0f));
            float xMax = yMax * aspectRatio;
            //GL.Frustum(-xMax, xMax, -yMax, yMax, zNear, zFar);
            Matrix4 frustum = Matrix4.Frustum(-xMax, xMax, -yMax, yMax, zNear, zFar);
            GL.MultMatrix(Matrix4.Transpose(frustum).Matrix);
        }
        public override void Update(float dTime) {
            base.Update(dTime);
        }
        public override void Render() {
            GL.Viewport(0, 0, MainGameWindow.Window.Width, MainGameWindow.Window.Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            float aspect = (float)MainGameWindow.Window.Width / (float)MainGameWindow.Window.Height;
            Matrix4 ortho = Matrix4.Ortho(-25.0f * aspect, 25.0f * aspect, -25.0f * aspect, 25.0f * aspect, -25.0f, 25.0f);
            GL.LoadMatrix(Matrix4.Transpose(ortho).OpenGL);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(10.0f, 5.0f, 15.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).OpenGL);
            MatrixStack stack = new MatrixStack();
            stack.Load(lookAt);
            GL.LoadMatrix(stack.OpenGL);

            grid.Render();
            DrawPlanets(-1.0f, 1.0f, 0.0f,stack);
        }
        
        protected void DrawPlanets(float worldX,float worldY, float worldZ,MatrixStack stack) {
            stack.Push();
                
            {
                Matrix4 scale = Matrix4.Scale(new Vector3(0.5f, 0.5f, 0.5f));
                Matrix4 translation = Matrix4.Translate(new Vector3(worldX, worldY, worldZ));
                Matrix4 model = translation * scale;
                stack.Mul(model);
                GL.LoadMatrix(stack.OpenGL);
                DrawSphere(3);
            }
        }
        //circle stuff
        static float X = 0.525731112119133606f;
        static float Z = 0.850650808352039932f;

        static float[][] vdata = new float[][] {
            new float[]{-X, 0.0f, Z}, new float[]{X, 0.0f, Z},
            new float[]{-X, 0.0f, -Z}, new float[]{X, 0.0f, -Z},
            new float[]{0.0f, Z, X}, new float[]{0.0f, Z, -X},
            new float[]{0.0f, -Z, X}, new float[]{0.0f, -Z, -X},
            new float[]{Z, X, 0.0f}, new float[]{-Z, X, 0.0f},
            new float[]{Z, -X, 0.0f}, new float[]{-Z, -X, 0.0f}
        };

        static int[][] tindices = new int[][] {
            new int[]{0,4,1}, new int[]{0,9,4}, new int[]{9,5,4},
            new int[]{4,5,8}, new int[]{4,8,1}, new int[]{8,10,1},
            new int[]{8,3,10}, new int[]{5,3,8}, new int[]{5,2,3},
            new int[]{2,7,3}, new int[]{7,10,3}, new int[]{7,6,10},
            new int[]{7,11,6}, new int[]{11,0,6}, new int[]{0,1,6},
            new int[]{6,1,10}, new int[]{9,0,11}, new int[]{9,11,2},
            new int[]{9,2,5}, new int[]{7,2,11}
        };

        protected static float[] Normalize(float[] float3) {
            float len = (float)Math.Sqrt(float3[0] * float3[0] + float3[1] * float3[1] + float3[2] * float3[2]);
            if (len != 0.0f) {
                float3[0] /= len;
                float3[1] /= len;
                float3[2] /= len;
            }
            return float3;
        }

        protected static void DrawTriangle(float[] a, float[] b, float[] c, int div, float r) {
            if (div <= 0) {
                GL.Normal3(b); GL.Vertex3(b[0] * r, b[1] * r, b[2] * r);
                GL.Normal3(a); GL.Vertex3(a[0] * r, a[1] * r, a[2] * r);
                GL.Normal3(c); GL.Vertex3(c[0] * r, c[1] * r, c[2] * r);
            }
            else {
                float[] ab = new float[] { 0.0f, 0.0f, 0.0f };
                float[] ac = new float[] { 0.0f, 0.0f, 0.0f };
                float[] bc = new float[] { 0.0f, 0.0f, 0.0f };
                for (int i = 0; i < 3; i++) {
                    ab[i] = (a[i] + b[i]) / 2;
                    ac[i] = (a[i] + c[i]) / 2;
                    bc[i] = (b[i] + c[i]) / 2;
                }
                ab = Normalize(ab);
                ac = Normalize(ac);
                bc = Normalize(bc);
                DrawTriangle(a, ab, ac, div - 1, r);
                DrawTriangle(b, bc, ab, div - 1, r);
                DrawTriangle(c, ac, bc, div - 1, r);
                DrawTriangle(ab, bc, ac, div - 1, r);
            }
        }

        public static void DrawSphere(int ndiv) {
            float radius = 1.0f;
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < 20; i++) {
                DrawTriangle(vdata[tindices[i][0]], vdata[tindices[i][1]], vdata[tindices[i][2]], ndiv, radius);
            }
            GL.End();
        }
    }
}
