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
            for ()
        }
    }
}
