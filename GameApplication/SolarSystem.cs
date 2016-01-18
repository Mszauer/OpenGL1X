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
                Circle.DrawSphere(3);
                stack.Push();
                {
                    GL.Color3(0.0f, 1.0f, 0.0f);
                    //first planet
                    Matrix4 p1scale = Matrix4.Scale(new Vector3(0.8f, 0.8f, 0.8f));
                    Matrix4 p1rotation = Matrix4.AngleAxis(50.0f, 1.0f, 0.0f, 0.0f);
                    Matrix4 p1translation = Matrix4.Translate(new Vector3(-2.5f, 0.5f, 0.0f));
                    Matrix4 planet = p1translation * p1rotation * p1scale;
                    stack.Mul(planet);
                    GL.LoadMatrix(stack.OpenGL);
                    Circle.DrawSphere(1);
                    stack.Push();
                    {
                        //draw planet1 moon
                        GL.Color3(1.0f, 0.0f, 0.0f);
                        Matrix4 m1Scale = Matrix4.Scale(new Vector3(0.5f, 0.5f, 0.5f));
                        Matrix4 m1Rotation = Matrix4.AngleAxis(45.0f, 0.0f, 1.0f, 0.0f);
                        Matrix4 m1Translation = Matrix4.Translate(new Vector3(-2.0f, 0.0f, 0.0f));
                        Matrix4 moon = m1Translation /*m1Rotation*/ * m1Scale;
                        stack.Mul(moon);
                        GL.LoadMatrix(stack.OpenGL);
                        Circle.DrawSphere(1);
                    }
                    stack.Pop();
                }//end first planet
                {
                    //second planet
                    GL.Color3(0.0f, 0.0f, 1.0f);
                    Matrix4 pscale = Matrix4.Scale(new Vector3(0.8f, 0.8f, 0.8f));
                    Matrix4 protation = Matrix4.AngleAxis(50.0f, 0.0f, 1.0f, 0.0f);
                    Matrix4 ptranslation = Matrix4.Translate(new Vector3(12.0f, 0.5f, 0.0f));
                    Matrix4 planet = ptranslation * protation * pscale;
                    stack.Mul(planet);
                    GL.LoadMatrix(stack.OpenGL);
                    Circle.DrawSphere(1);
                    stack.Push();
                    {
                        //draw planet1 moon
                        GL.Color3(0.5f, 1.0f, 1.0f);
                        Matrix4 mScale = Matrix4.Scale(new Vector3(0.5f, 0.5f, 0.5f));
                        Matrix4 mRotation = Matrix4.AngleAxis(45.0f, 1.0f, 0.0f, 0.0f);
                        Matrix4 mTranslation = Matrix4.Translate(new Vector3(2.0f, 0.0f, 0.0f));
                        Matrix4 moon = mTranslation * mRotation * mScale;
                        stack.Mul(moon);
                        GL.LoadMatrix(stack.OpenGL);
                        Circle.DrawSphere(1);
                    }
                    stack.Pop();
                }
                stack.Pop();
            }
        }
        //circle stuff
        
    }
}
