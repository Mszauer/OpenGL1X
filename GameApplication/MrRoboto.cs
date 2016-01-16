using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class MrRoboto : Game{
        Grid grid = null;

        float leftArmRot = -15.0f;
        float rightArmRot = 15.0f;
        float leftLegRot = 15.0f;
        float rightLegRot = -15.0f;

        float leftArmDir = 1.0f;
        float rightArmDir = -1.0f;
        float leftLegDir = -1.0f;
        float rightLegDir = 1.0f;

        public override void Initialize() {
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
            GL.Frustum(-xMax, xMax, -yMax, yMax, zNear, zFar);
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
        public override void Update(float dTime) {
            //update rotations
            leftArmRot += 50.0f * dTime * leftArmDir;
            rightArmRot += 50.0f * dTime * rightArmDir;
            leftLegRot += 50.0f * dTime * leftLegDir;
            rightLegRot += 50.0f * dTime * rightLegDir;

            //clamp and change dir
            if (leftArmRot > 20.0f || leftArmRot < -20.0f) {
                leftArmRot = (leftArmRot < 0.0f) ? -20.0f : 20.0f;
                leftArmDir *= -1;
            }
            if (rightArmRot > 20.0f || rightArmRot < -20.0f) {
                rightArmRot = (rightArmRot < 0.0f) ? -20.0f : 20.0f;
                rightArmDir *= -1;
            }
            if (leftLegRot > 20.0f || leftLegRot < -20.0f) {
                leftLegRot = (leftLegRot < 0.0f) ? -20.0f : 20.0f;
                leftLegDir *= -1;
            }
            if (rightLegRot > 20.0f || rightLegRot < -20.0f) {
                rightLegRot = (rightLegRot < 0.0f) ? -20.0f : 20.0f;
                rightLegDir *= -1;
            }
        }
        public override void Render() {
            GL.Viewport(0, 0, MainGameWindow.Window.Width, MainGameWindow.Window.Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Perspective(60.0f, (float)MainGameWindow.Window.Width / (float)MainGameWindow.Window.Height, 0.01f, 1000.0f);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            LookAt(10.0f, 5.0f, 15.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            grid.Render();
            DrawRobot(-1.0f, 1.0f, 0.0f);
        }
        protected void DrawRobot(float worldX,float worldY, float worldZ) {
            //save matrix state
            GL.PushMatrix();

                //translate robot to desired coordinates
                GL.Translate(worldX, worldY, worldZ);

                //draw head
                GL.Color3(1.0f, 0.0f, 0.0f);//red
                GL.PushMatrix();
                    GL.Translate(1.0f, 4.0f, 0.0f);
                    GL.Scale(0.5f, 0.5f, 0.5f);
                    DrawCube();
                GL.PopMatrix();//finish head
                //draw body
                GL.Color3(0.0f, 1.0f, 0.0f);//green
                GL.PushMatrix();
                    GL.Translate(1.0f, 2.5f, 0.0f);
                    GL.Scale(0.75f, 1.0f, 0.5f);
                    DrawCube();
                GL.PopMatrix();
                //draw left arm
                GL.Color3(1.0f, 0.0f, 1.0f);//magenta
                GL.PushMatrix();
                    GL.Translate(0.0f, 2.25f, 0.0f);
                    GL.Translate(0.0f, 1.0f, 0.0f);//unPivot
                    GL.Rotate(leftArmRot, 1.0f, 0.0f, 0.0f);
                    GL.Translate(0.0f, -1.0f, 0.0f);
                    GL.Scale(0.25f, 1.0f, 0.25f);
                    DrawCube();
                GL.PopMatrix();
                //draw right arm
                GL.Color3(0.0f, 0.0f, 1.0f);//blue
                GL.PushMatrix();
                    GL.Translate(2.0f, 2.25f, 0.0f);
                    GL.Translate(0.0f, 1.0f, 0.0f);
                    GL.Rotate(rightArmRot, 1.0f, 0.0f, 0.0f);
                    GL.Translate(0.0f, -1.0f, 0.0f);
                    GL.Scale(0.25f, 1.0f, 0.25f);
                    DrawCube();
                GL.PopMatrix();
                //draw left leg
                GL.Color3(1.0f, 1.0f, 0.0f);//yellow
                GL.PushMatrix();
                    GL.Translate(0.5f, 0.5f, 0.0f);
                    GL.Translate(0.0f, 1.0f, 0.0f);
                    GL.Rotate(leftLegRot, 1.0f, 0.0f, 0.0f);
                    GL.Translate(0.0f, -1.0f, 0.0f);
                    GL.Scale(0.25f, 1.0f, 0.25f);
                    DrawCube();
                //draw left foot
                    GL.PushMatrix();
                        GL.Translate(0.0f, -1.0f, 1.0f);
                        GL.Scale(1.0f, 0.25f, 2.0f);
                        DrawCube();
                    GL.PopMatrix();
                GL.PopMatrix();
                //Draw Right leg
                GL.Color3(0.0f, 1.0f, 1.0f);//baby blue
                GL.PushMatrix();
                    GL.Translate(1.5f, 0.5f, 0.0f);
                    GL.Translate(0.0f, 1.0f, 0.0f);
                    GL.Rotate(rightLegRot, 1.0f, 0.0f, 0.0f);
                    GL.Translate(0.0f, -1.0f, 0.0f);
                    GL.Scale(0.25f, 1.0f, 0.25f);
                    DrawCube();
                    //draw right food
                    GL.PushMatrix();
                        GL.Translate(0.0f, -1.0f, 1.0f);
                        GL.Scale(1.0f, 0.25f, 2.0f);
                        DrawCube();
                    GL.PopMatrix();//foot pop
                GL.PopMatrix();//leg pop
            GL.PopMatrix();//pop first matrix push
        }
    }
}
