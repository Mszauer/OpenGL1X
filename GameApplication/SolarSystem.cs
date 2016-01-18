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
        
        public override void Update(float dTime) {
            base.Update(dTime);
        }
        public override void Render() {
            GL.Viewport(0, 0, MainGameWindow.Window.Width, MainGameWindow.Window.Height);

            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)MainGameWindow.Window.Width / (float)MainGameWindow.Window.Height;
            Matrix4 perspective = Matrix4.Perspective(60, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(10.0f, 5.0f, 15.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            MatrixStack stack = new MatrixStack();
            stack.Load(lookAt);
            GL.LoadMatrix(stack.OpenGL);

            grid.Render();
            DrawPlanets(-1.0f, 1.0f, 0.0f,stack);
        }
        
        protected void DrawPlanets(float worldX,float worldY, float worldZ,MatrixStack stack) {
            stack.Push();
            {
                GL.Color3(1.0f, 1.0f, 0.0f);
                //sun
                Matrix4 scale = Matrix4.Scale(new Vector3(0.5f, 0.5f, 0.5f));
                Matrix4 translation = Matrix4.Translate(new Vector3(worldX, worldY, worldZ));
                Matrix4 model = translation * scale;
                stack.Mul(model);
                GL.LoadMatrix(stack.OpenGL);
                DrawSphere(3);
                stack.Push();
                GL.LoadMatrix(stack[stack.Count-1]);
                {
                    GL.Color3(0.0f, 1.0f, 0.0f);
                    //first planet
                    Matrix4 p1scale = Matrix4.Scale(new Vector3(0.3f, 0.3f, 0.3f));
                    Matrix4 p1rotation = Matrix4.AngleAxis(50.0f, 1.0f, 0.0f, 0.0f);
                    Matrix4 p1translation = Matrix4.Translate(new Vector3(-0.55f, 0.5f, 0.0f));
                    Matrix4 planet = p1translation * p1rotation * p1scale;
                    stack.Mul(planet);
                    GL.LoadMatrix(stack.OpenGL);
                    DrawSphere(1);
                    stack.Push();
                    {
                        //draw planet1 moon
                        GL.Color3(1.0f, 0.0f, 0.0f);
                        Matrix4 m1Scale = Matrix4.Scale(new Vector3(-0.2f, 0.05f, 0.0f));
                        Matrix4 m1Rotation = Matrix4.AngleAxis(45.0f, 0.0f, 1.0f, 0.0f);
                        Matrix4 m1Translation = Matrix4.Translate(new Vector3(-0.3f, 0.0f, 0.0f));
                        Matrix4 moon = m1Translation * m1Rotation * m1Scale;
                        stack.Mul(moon);
                        GL.LoadMatrix(moon.OpenGL);
                        DrawSphere(1);
                    }
                    stack.Pop();
                }//end first planet
                {
                    //do second planet stuff here
                }
                stack.Pop();
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
