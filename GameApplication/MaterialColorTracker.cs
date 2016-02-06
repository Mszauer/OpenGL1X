using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class MaterialColorTracker : LightingExample{
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

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new float[] { 1, 1, 1, 1 });
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 20.0f);
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);

        }
        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(0.0f, 5.0f, -7.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            GL.Disable(EnableCap.Lighting);
            grid.Render();
            GL.Enable(EnableCap.Lighting);

            GL.Color3(0.0f, 1.0f, 0.0f);//green middle
            GL.PushMatrix();
            {
                GL.Translate(0.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new float[] { 0.0f, 1.0f, 0.0f, 1.0f });
                Primitives.DrawSphere(3);
            }
            GL.PopMatrix();

            GL.Color3(0.0f, 0.0f, 1.0f);//blue left
            GL.PushMatrix();
            {
                GL.Translate(3.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
                Primitives.DrawSphere(3);
            }
            GL.PopMatrix();

            GL.Color3(1.0f, 0.0f, 0.0f);//red right
            GL.PushMatrix();
            {
                GL.Translate(-3.0f, 1.0f, 0.5f);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new float[] { 1.0f, 1.0f, 0.0f, 1.0f });
                Primitives.DrawSphere(3);
            }
            GL.PopMatrix();
        }
        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;
            Matrix4 perspective = Matrix4.Perspective(60.0f, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}
