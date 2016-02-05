using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class MaterialSpecular : LightingExample{
        float[] white = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        public override void Initialize() {
            base.Initialize();
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
        }
        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(0.0f, 5.0f, -7.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f,1.0f,0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            GL.Disable(EnableCap.Lighting);
            grid.Render();
            GL.Enable(EnableCap.Lighting);

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.PushMatrix();
            {
                GL.Translate(0.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, white);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 0f);
                Primitives.DrawSphere(3);
            }
            GL.PopMatrix();

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.PushMatrix();
            {
                GL.Translate(3.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, white);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 16f);
                Primitives.DrawSphere(3);
            }
            GL.PopMatrix();

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.PushMatrix();
            {
                GL.Translate(-3.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, white);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 128f);
                Primitives.DrawSphere(3);
            }
            GL.PopMatrix();
        }
        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;

            Matrix4 perspective = Matrix4.Perspective(60, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}
