using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using System.Drawing;
using System.Drawing.Imaging;

namespace GameApplication {
    class SnowParticleSystem : ParticleSystem{
        public static Vector3 SNOWFLAKE_VELOCITY = new Vector3(0f, -2f, 0f);
        public static Vector3 VELOCITY_VARIATION = new Vector3(0.2f, 0.5f, 0.2f);
        public static float SNOWFLAKE_SIZE = 0.25f;
        public static float SIZE_DELTA = 0.25f;
        public static float SNOWFLAKE_PER_SEC = 500;

        protected float height;
        protected float width;
        protected float depth;
        protected int texture;

        public SnowParticleSystem(int maxParticles,Vector3 origin, Vector3 size) :base (maxParticles, origin) {
            height = size.Y;
            width = size.X;
            depth = size.Z;

            texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            Bitmap bmp = new Bitmap("Assets/snow.png");
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            bmp.UnlockBits(bmp_data);
            bmp.Dispose();
        }
        public override void Shutdown() {
            GL.DeleteTexture(texture);
            texture = -1;
        }
        public override void Update(float dTime) {
            for (int i = 0; i < numParticles;/*forever loop*/) {
                particleList[i].position = particleList[i].position + particleList[i].velocity * dTime;
                //if out of bounds / hit border
                if (particleList[i].position.Y <= systemOrigin.Y) {
                    //copy the last particle into this one and reduce list by 1
                    particleList[i] = particleList[--numParticles];
                    //create new particle
                    particleList[numParticles] = new Particle();
                }
                else {
                    i++;//go to next particle
                }
            }
            //store elapsed time
            accumulatedTime += dTime;
            //determine how many should be alive
            int newParticles = (int)(SNOWFLAKE_PER_SEC * accumulatedTime);
            //reduce stored time by newly spawned particle
            accumulatedTime -= 1.0f / SNOWFLAKE_PER_SEC * newParticles;
            //request more particles
            Emit(newParticles);
        }
        public override void Render() {
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < numParticles; i++) {
                Vector3 startPos = particleList[i].position;
                float size = particleList[i].size;

                GL.TexCoord2(0f, 1f);
                GL.Vertex3(startPos.X, startPos.Y, startPos.Z);

                GL.TexCoord2(1f, 1f);
                GL.Vertex3(startPos.X + size, startPos.Y, startPos.Z);

                GL.TexCoord2(1f, 0f);
                GL.Vertex3(startPos.X + size, startPos.Y - size, startPos.Z);

                GL.TexCoord2(0f, 0f);
                GL.Vertex3(startPos.X, startPos.Y - size, startPos.Z);
            }
            GL.End();
        }
        public override void InitParticle(int index) {
            particleList[index].position.X = systemOrigin.X + (float)random.NextDouble() * width;
            particleList[index].position.Y = height;
            particleList[index].position.Z = systemOrigin.Z + (float)random.NextDouble() * depth;

            particleList[index].size = SNOWFLAKE_SIZE + (float)random.NextDouble() * SIZE_DELTA;

            particleList[index].velocity.X = SNOWFLAKE_VELOCITY.X + (float)random.NextDouble() * VELOCITY_VARIATION.X;
            particleList[index].velocity.Y = SNOWFLAKE_VELOCITY.Y + (float)random.NextDouble() * VELOCITY_VARIATION.Y;
            particleList[index].velocity.Z = SNOWFLAKE_VELOCITY.Z + (float)random.NextDouble() * VELOCITY_VARIATION.Z;

        }
    }
}
