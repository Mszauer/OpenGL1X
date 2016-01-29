using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class PointLight : LightingExample{
        Vector2 redAngle = null;
        Vector2 greenAngle = null;

        public override void Initialize() {
            grid = new Grid();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            Resize(MainGameWindow.Window.Width, MainGameWindow.Window.Height);
            redAngle = new Vector2();
            greenAngle = new Vector2();
            //enable lighting
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            //set light color to yellow
            float[] yellow = new float[] { 1.0f, 1.0f, 0.0f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, yellow);
            GL.Light(LightName.Light0, LightParameter.Specular, yellow);
            
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

            //set light position
            //w of 1 makes the light a point light
            float[] position = new float[] { 0.0f, 1.0f, 0.0f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Position, position);

            grid.Render();

            //set ray debug position
            Vector3 redPosition = new Vector3();
            redPosition.X = 1.0f * -(float)Math.Sin(redAngle.X * rads) * (float)Math.Cos(redAngle.Y * rads);
            redPosition.Y = 1.0f * -(float)Math.Sin(redAngle.Y * rads);
            redPosition.Z = -1.0f * (float)Math.Cos(redAngle.X * rads) * (float)Math.Cos(redAngle.Y * rads);

            Vector3 greenPosition = new Vector3();
            greenPosition.X = 1.0f * -(float)Math.Sin(greenAngle.X * rads) * (float)Math.Cos(greenAngle.Y * rads);
            greenPosition.Y = 1.0f * -(float)Math.Sin(greenAngle.Y * rads);
            greenPosition.Z = -1.0f * (float)Math.Cos(greenAngle.X * rads) * (float)Math.Cos(greenAngle.Y * rads);

            //add debug visualization
            GL.Disable(EnableCap.Lighting);
            GL.PushMatrix();
            GL.Translate(4f, 4f, 0f);
            GL.Begin(PrimitiveType.Lines);
            //draw red ray
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0f, 0f, 0f);
            redPosition.Normalize();
            redPosition *= -1.0f;
            GL.Vertex3(redPosition.X, redPosition.Y, redPosition.Z);
            //draw green ray
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            greenPosition.Normalize();
            greenPosition *= -2.0f;
            GL.Vertex3(greenPosition.X, greenPosition.Y, greenPosition.Z);
            GL.End();
            GL.PopMatrix();
            //re-enable lights
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
            base.Resize(width, height);
        }
    }
}
