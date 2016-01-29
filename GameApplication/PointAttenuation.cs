using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class PointAttenuation : LightingExample {
        Vector2 redAngle = null;
        Vector2 greenAngle = null;
        float lightAngle = 0.0f;

        public override void Initialize() {
            base.Initialize();
            redAngle = new Vector2();
            greenAngle = new Vector2();
            //enable lighting
            GL.Enable(EnableCap.Lighting);
            //enable each light
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Light1);
            //Light0 initialize
            float[] yellow = new float[] { 1.0f, 1.0f, 0.0f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, yellow);
            GL.Light(LightName.Light0, LightParameter.Specular, yellow);
            GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 0.25f);
            GL.Light(LightName.Light0, LightParameter.LinearAttenuation, 0.25f);
            GL.Light(LightName.Light0, LightParameter.QuadraticAttenuation, 0.0f);
            //light1 initialize
            float[] red = new float[] { 1.0f, 0.0f, 0.0f, 1.0f };
            GL.Light(LightName.Light1, LightParameter.Ambient, red);
            GL.Light(LightName.Light1, LightParameter.Diffuse, red);
            GL.Light(LightName.Light1, LightParameter.Specular, red);
            GL.Light(LightName.Light1, LightParameter.ConstantAttenuation, 1.0f);
            GL.Light(LightName.Light1, LightParameter.LinearAttenuation, 0.0f);
            GL.Light(LightName.Light1, LightParameter.QuadraticAttenuation, 0.0f);

        }
        public override void Update(float dTime) {
            base.Update(dTime);
            cameraAngle.X += 30.0f * dTime;
            lightAngle += 90.0f * dTime;
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
            float[] position = new float[] { 0.5f, 1.0f, 0.5f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Position, position);

            grid.Render(3);
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

            GL.Enable(EnableCap.Light1);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.PushMatrix();
            {
                GL.Translate(2.5f, 1.0f, -0.5f);
                //move light into place
                GL.PushMatrix();
                {
                    GL.Rotate(lightAngle, 0.0f, 1.0f, 0.0f);
                    GL.Translate(-2.0f, 0.0f, -2.0f);
                    //render light where model-view matrix is
                    position = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
                    GL.Light(LightName.Light1, LightParameter.Position, position);
                    //disable lighting
                    GL.Disable(EnableCap.Light1);
                    //make light smaller
                    GL.Scale(0.25f, 0.25f, 0.25f);
                    //and blue
                    GL.Color3(0.0f, 0.0f, 1.0f);
                    //draw light visualization
                    Primitives.DrawSphere();
                    //re-enable lighting
                    GL.Enable(EnableCap.Light1);
                }
                //get rid of light matrix
                GL.PopMatrix();
                Primitives.DrawSphere(5);
            }
            GL.PopMatrix();
            GL.Disable(EnableCap.Light1);

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
