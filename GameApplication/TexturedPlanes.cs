using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;
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
            GL.BindTexture(TextureTarget.Texture2D, crazyTexture);
            //GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public override void Shutdown() {
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

            GL.Color3(1f, 1f, 1f);//white
            GL.Begin(PrimitiveType.Triangles);
                GL.Vertex3(1, 4, 2);//top right
                GL.Vertex3(1, 4, -2);//top left
                GL.Vertex3(1, 0, -2);//bottom left

                GL.Vertex3(1, 4, 2);//top right
                GL.Vertex3(1, 0, -2);//bottom left
                GL.Vertex3(1, 0, 2);//bottom Right
            GL.End();
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
