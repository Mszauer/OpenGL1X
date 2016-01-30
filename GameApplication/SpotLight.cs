using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
        class SpotLight : LightingExample {

        public override void Initialize() {
            base.Initialize();
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            float[] black = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
            float[] yellow = new float[] { 1.0f, 1.0f, 0.0f, 1.0f };
            float[] direction = new float[] { 0.0f, -1.0f, 0.0f };
            GL.Light(LightName.Light0, LightParameter.Ambient, yellow);
            GL.Light(LightName.Light0, LightParameter.Diffuse, yellow);
            GL.Light(LightName.Light0, LightParameter.Specular, black);
            GL.Light(LightName.Light0, LightParameter.SpotCutoff, 15f);
            GL.Light(LightName.Light0, LightParameter.SpotExponent, 5.0f);
            GL.Light(LightName.Light0, LightParameter.SpotDirection, direction);
        }
        public override void Update(float dTime) {
            base.Update(dTime);
        }
        public override void Render() {
            Vector3 eyePos = new Vector3();
            eyePos.X = cameraAngle.Z * -(float)Math.Sin(cameraAngle.X * rads * (float)Math.Cos(cameraAngle.Y * rads));
            eyePos.Y = cameraAngle.Z * -(float)Math.Sin(cameraAngle.Y * rads);
            eyePos.Z = -cameraAngle.Z * (float)Math.Cos(cameraAngle.X * rads * (float)Math.Cos(cameraAngle.Y * rads));

            Matrix4 lookAt = Matrix4.LookAt(eyePos, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            float[] pos = new float[] { 0.0f, 3.0f, 3.0f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Position, pos);
           
            grid.Render(6);

            GL.Disable(EnableCap.Lighting);
            GL.PushMatrix();
            {
                GL.Translate(pos[0],pos[1],pos[2]);
                GL.Scale(0.25f, 0.25f, 0.25f);
                GL.Color3(1.0f, 1.0f, 0.0f);
                Primitives.DrawSphere();
                GL.Begin(PrimitiveType.Lines);

                GL.Vertex3(0.0f, 0.0f, 0.0f);
                Vector3 lightDirection = new Vector3(-2.0f, -1.0f, -3.0f);
                lightDirection.Normalize();
                GL.Vertex3(lightDirection[0] * 5.0f, lightDirection[1] * 5.0f, lightDirection[2] * 5.0f);
                GL.End();
            }
            GL.PopMatrix();
            GL.Enable(EnableCap.Lighting);

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
            base.Resize(width,height);
        }
    }
}
