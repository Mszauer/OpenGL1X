using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class MaterialColors : LightingExample {
        public override void Initialize() {
            base.Initialize();
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            float[] lightPosition = new float[] { 0f, 1f, 1f };
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
            float[] red = new float[] { 1f, 0f, 0f, 1f };
            float[] blue = new float[] { 0f, 0f, 1f, 1f };
            float[] white = new float[] { 1f, 1f, 1f, 1f };
            GL.Light(LightName.Light0, LightParameter.Ambient, red);
            GL.Light(LightName.Light0, LightParameter.Diffuse, blue);
            GL.Light(LightName.Light0, LightParameter.Specular, white);

        }
        public override void Render() {

            Matrix4 lookAt = Matrix4.LookAt(new Vector3(0.0f, 5.0f, -7.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            GL.Disable(EnableCap.Lighting);
            grid.Render();
            GL.Enable(EnableCap.Lighting);

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.PushMatrix();
            {
                GL.Translate(0.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, new float[] { 1f, 1f, 1f, 1f });
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new float[] { 0f, 0f, 0f, 1f });
                Primitives.DrawSphere(3);
            }
            GL.PopMatrix();

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.PushMatrix();
            {
                GL.Translate(3.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, new float[] { 0f, 0f, 0f, 1f });
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new float[] { 1f, 1f, 1f, 1f });
                Primitives.DrawSphere(3);
            }
            GL.PopMatrix();

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.PushMatrix();
            {
                GL.Translate(-3.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, new float[] { 0.2f, 0.2f, 0.2f, 1f });
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new float[] { 0.8f, 0.8f, 0.8f, 1f });
                Primitives.DrawSphere(3);
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
