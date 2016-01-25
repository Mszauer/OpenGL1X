using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class Directional1 : LightingExample{
        Vector2 redAngle = null;
        Vector2 greenAngle = null;
        public override void Initialize() {
            base.Initialize();

            redAngle = greenAngle = new Vector2();

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Light1);
            GL.Enable(EnableCap.Light2);

            float[] white = new float[] { 0f, 0f, 0f, 1f };
            float[] blue = new float[] { 0f, 0f, 1f, 1f };
            float[] red = new float[] { 1.0f, 0.0f, 0.0f, 1.0f };
            float[] green = new float[] { 0.0f, 1.0f, 0.0f, 1.0f };
            //Config light0
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.0f, 0.5f, 0.5f, 0.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, blue);
            GL.Light(LightName.Light0, LightParameter.Diffuse, blue);
            GL.Light(LightName.Light0, LightParameter.Specular, white);

            //config light1
            GL.Light(LightName.Light1, LightParameter.Position, new float[] { 0.0f, -0.5f, 0.5f, 0.0f });
            GL.Light(LightName.Light1, LightParameter.Ambient, red);
            GL.Light(LightName.Light1, LightParameter.Diffuse, red);
            GL.Light(LightName.Light1, LightParameter.Specular, white);

            //config light2
            GL.Light(LightName.Light2, LightParameter.Position, new float[] { 1.0f, 0.0f, 0.0f, 0.0f });
            GL.Light(LightName.Light2, LightParameter.Ambient, green);
            GL.Light(LightName.Light2, LightParameter.Diffuse, green);
            GL.Light(LightName.Light2, LightParameter.Specular, white);

            Vector3 redPosition = new Vector3();
            redPosition.X = 1.0f * -(float)Math.Sin(redAngle.X * rads) * (float)Math.Cos(redAngle.Y * rads);
            redPosition.Y = 1.0f * -(float)Math.Sin(redAngle.Y * rads);
            redPosition.Z = -1.0f * (float)Math.Cos(redAngle.X * rads) * (float)Math.Cos(redAngle.Y * rads);

            Vector3 greenPosition = new Vector3();
            greenPosition.X = 1.0f * -(float)Math.Sin(greenAngle.X * rads) * (float)Math.Cos(greenAngle.Y * rads);
            greenPosition.Y = 1.0f * -(float)Math.Sin(greenAngle.Y * rads);
            greenPosition.Z = -1.0f * (float)Math.Cos(greenAngle.X * rads) * (float)Math.Cos(greenAngle.Y * rads);

            GL.Light(LightName.Light1, LightParameter.Position, new float[] { redPosition.X, redPosition.Y, redPosition.Z, 0.0f });
            GL.Light(LightName.Light2, LightParameter.Position, new float[] { greenPosition.X, greenPosition.Y, greenPosition.Z, 0.0f });

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
            greenPosition *= -1.0f;
            GL.Vertex3(greenPosition.X, greenPosition.Y, greenPosition.Z);
            GL.End();
            GL.PopMatrix();
            //re-enable lights
            GL.Enable(EnableCap.Lighting);
        }
        public override void Render() {
            base.Render();
        }
        public override void Resize(int width, int height) {
            base.Resize(width, height);
        }
        public override void Update(float dTime) {
            //base.Update(dTime);
            redAngle.Y += 15.0f * dTime;
            greenAngle.X += 30.0f * dTime;
        }
    }
}
