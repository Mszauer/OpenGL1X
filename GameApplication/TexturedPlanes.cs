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
        int houseTexture = 0;
        int uiTexture = 0;
        float house1_uv_top = 0f;
        float house1_uv_bottom = 0f;
        float house1_uv_left = 0f;
        float house1_uv_right = 0f;
        float house2_uv_top = 0f;
        float house2_uv_bottom = 0f;
        float house2_uv_left = 0f;
        float house2_uv_right = 0f;
        public override void Initialize() {
            base.Initialize();
            base.Initialize();
            grid = new Grid(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            crazyTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, crazyTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            {
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

            houseTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, houseTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            {
                Bitmap bmp = new Bitmap("Assets/houses.png");
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                house1_uv_bottom = 326.0f/(float)bmp.Height;
                house1_uv_right = 186.0f / (float)bmp.Width;

                house2_uv_left = 332.0f / (float)bmp.Width;
                house2_uv_right = (332.0f + 180.0f) / (float)bmp.Width;
                house2_uv_bottom = 336.0f / (float)bmp.Height;

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
                bmp.UnlockBits(bmp_data);
                bmp.Dispose();
            }
            uiTexture = GL.GenTexture();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //set parameters like above
            //create uv ints
        }
        public override void Shutdown() {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.DeleteTexture(crazyTexture);
            GL.DeleteTexture(houseTexture);
            GL.DeleteTexture(uiTexture);
            crazyTexture = -1;
            houseTexture = -1;
            uiTexture = -1;
            base.Shutdown();
        }
        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(-7.0f, 5.0f, -7.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            RenderWorld();

            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();//backup 3d projection
            GL.LoadIdentity();//clear

            GL.MatrixMode(MatrixMode.Modelview);//switch to model
            GL.PushMatrix();//back up modelview
            GL.LoadIdentity();//clear

            //set gl.ortho
            //RenderUI();

            GL.MatrixMode(MatrixMode.Projection);//switch back to world
            GL.PopMatrix();

            GL.MatrixMode(MatrixMode.Modelview);//restore 3d modelview
            GL.PopMatrix();
            // We make sure the matrix mode is modelview for the next render iteration
        }
        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;
            Matrix4 perspective = Matrix4.Perspective(60.0f, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        public void DrawTexture(int texID, Rectangle screenRect, Rectangle source, Size sourceImage) {
            GL.BindTexture(TextureTarget.Texture2D, texID);
            GL.Color3(1f, 1f, 1f);

            float top = screenRect.Y;
            float left = screenRect.X;
            float bottom = screenRect.Y + screenRect.Height;
            float right = screenRect.X + screenRect.Width;

            float uv_top = (float)source.Y / (float)sourceImage.Height;
            float uv_left = (float)source.X / (float)sourceImage.Width;
            float uv_bottom = (float)(source.Y + source.Height) / (float)sourceImage.Height;
            float uv_right = (float)(source.X + source.Width) / (float)sourceImage.Width;


            GL.Begin(PrimitiveType.Triangles);

            GL.TexCoord2(uv_right, uv_top);//top right
            GL.Vertex3(right, top, 0);

            GL.TexCoord2(uv_left, uv_top);//top left
            GL.Vertex3(left, top, 0);

            GL.TexCoord2(uv_left, uv_bottom);//bottom left
            GL.Vertex3(left, bottom, 0);

            GL.TexCoord2(uv_right, uv_top);//topRight
            GL.Vertex3(right, top, 0);

            GL.TexCoord2(uv_left, uv_bottom);//bottomLeft
            GL.Vertex3(left, bottom, 0);

            GL.TexCoord2(uv_right, uv_bottom);//bottomRight
            GL.Vertex3(right, bottom, 0);

            GL.End();
        }
        public void RenderWorld() {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            grid.Render();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            GL.Color3(1f, 1f, 1f);//white
            GL.BindTexture(TextureTarget.Texture2D, crazyTexture);
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(1, 0);
            GL.Vertex3(1, 4, 2);//top right
            GL.TexCoord2(0, 0);
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

            GL.BindTexture(TextureTarget.Texture2D, houseTexture);
            //house 1
            GL.Color3(1f, 1f, 1f);
            GL.PushMatrix();
            GL.Translate(-1f, 0.5f, -1f);
            GL.Rotate(-130f, 0f, 1f, 0f);
            GL.Scale(0.57f, 1f, 1f);
            GL.Scale(3f, 3f, 3f);
            GL.Begin(PrimitiveType.Triangles);

            GL.TexCoord2(house1_uv_right, house1_uv_top);
            GL.Vertex3(0.5f, 0.5f, 0f);//top right
            GL.TexCoord2(house1_uv_left, house1_uv_top);
            GL.Vertex3(-0.5f, 0.5f, 0f);//top left
            GL.TexCoord2(house1_uv_left, house1_uv_bottom);
            GL.Vertex3(-0.5f, -0.5f, 0f);//bottom left
            GL.TexCoord2(house1_uv_right, house1_uv_top);
            GL.Vertex3(0.5, 0.5, 0);//top right
            GL.TexCoord2(house1_uv_left, house1_uv_bottom);
            GL.Vertex3(-0.5, -0.5, 0);//bottom left
            GL.TexCoord2(house1_uv_right, house1_uv_bottom);
            GL.Vertex3(0.5, -0.5, 0);//bottom right
            GL.End();
            GL.PopMatrix();
            // house 2
            GL.Color3(1f, 1f, 1f);
            GL.PushMatrix();
            GL.Translate(-2f, 0.5f, -3f);
            GL.Rotate(-130f, 0f, 1f, 0f);
            GL.Scale(0.53f, 1f, 1f);
            GL.Scale(3f, 3f, 3f);
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(house2_uv_right, house2_uv_top);
            GL.Vertex3(0.5, 0.5, 0);//top right
            GL.TexCoord2(house2_uv_left, house2_uv_top);
            GL.Vertex3(-0.5, 0.5, 0);//top left
            GL.TexCoord2(house2_uv_left, house2_uv_bottom);
            GL.Vertex3(-0.5, -0.5, 0);//bottom left
            GL.TexCoord2(house2_uv_right, house2_uv_top);
            GL.Vertex3(0.5, 0.5, 0);//top right
            GL.TexCoord2(house2_uv_left, house2_uv_bottom);
            GL.Vertex3(-0.5, -0.5, 0);//bottom left
            GL.TexCoord2(house2_uv_right, house2_uv_bottom);
            GL.Vertex3(0.5, -0.5, 0);//bottom Right
            GL.End();
            GL.PopMatrix();

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public void RenderUI() {

        }
    }
}
