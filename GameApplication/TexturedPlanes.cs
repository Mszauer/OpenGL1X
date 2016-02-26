using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using System.Drawing;
using System.Drawing.Imaging;

namespace GameApplication {
    class TexturedPlanes : Game {
        protected Grid grid = null;
        int crazyTexture = 0;
        public override void Initialize() {
            base.Initialize();
            base.Initialize();
            grid = new Grid(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);

            crazyTexture = GL.GenTexture();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            //load bmp texture
            Bitmap bmp = new Bitmap("Assets/crazy_taxi.png");
            //get the data about bmp
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //upload data to gpu
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            //mark cpu memory for disposal
            bmp.UnlockBits(bmp_data);
            bmp.Dispose();

        }
        public override void Shutdown() {
            GL.DeleteTexture(crazyTexture);
            crazyTexture = -1;
            base.Shutdown();
        }
        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(-7.0f, 5.0f, -7.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            grid.Render();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, crazyTexture);

            GL.Color3(1f, 1f, 1f);//white
            GL.Begin(PrimitiveType.Triangles);
                GL.TexCoord2(1, 0);
                GL.Vertex3(1, 4, 2);//top right
                GL.TexCoord2(0,0);
                GL.Vertex3(1, 4, -2);//top left
                GL.TexCoord2(0, 1);
                GL.Vertex3(1, 0, -2);//bottom left

                GL.TexCoord2(1, 0);
                GL.Vertex3(1, 4, 2);//top right
                GL.TexCoord2(0, 1);
                GL.Vertex3(1, 0, -2);//bottom left
                GL.TexCoord2(1, 1);
                GL.Vertex3(1, 0, 2);//bottom Right
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;
            Matrix4 perspective = Matrix4.Perspective(60.0f, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }
    }
}
