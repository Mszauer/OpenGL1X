using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class VBO : Game {
        Grid grid = null;
        float[] cubeVertices = null;
        float[] cubeColors = null;
        uint[] cubeIndicies = null;
        protected int vertexBuffer;
        protected int indexBuffer;
        protected int numIndices;

        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;
            Matrix4 perspective = Matrix4.Perspective(60.0f, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        public override void Initialize() {
            grid = new Grid(true);

            cubeVertices = new float[] { //includes colors
                -1.0f,-1.0f,1.0f,//vertex 1
                1.0f,-1.0f,1.0f,//vertex2
                1.0f,1.0f,1.0f,//vertex3
                -1.0f,1.0f,1.0f,//vertex4
                -1.0f,-1.0f,-1.0f,//vertex5
                1.0f,-1.0f,-1.0f,//vertex6
                1.0f,1.0f,-1.0f,//vertex7
                -1.0f,1.0f,-1.0f,//vertex8
                //colors
                1.0f,0.0f,0.0f,//vertex1
                0.0f,1.0f,0.0f,//vertex2
                0.0f,0.0f,1.0f,//vertex3
                1.0f,1.0f,1.0f,//vertex4
                1.0f,0.0f,0.0f,//vertex5
                0.0f,1.0f,0.0f,//vertex6
                0.0f,0.0f,1.0f,//vertex7
                1.0f,1.0f,1.0f//vertex8
            };
            cubeIndicies = new uint[] {
                //front
                0,1,2,//front triangle1
                2,3,0,//front triangle2
                //top
                1,5,6,//top triangle1
                6,2,1,//top triangle2
                //back
                7,6,5,//back triangle1
                5,4,7,//back triangle2
                //bottom
                4,0,3,//bottom triangle1
                3,7,4,//bottom triangle2
                //left
                4,5,1,//left triangle1
                1,0,4,//left triangle2
                //right
                3,2,6,//right triangle1
                6,7,3//right triangle2
            };

            numIndices = cubeIndicies.Length;

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, new System.IntPtr(cubeVertices.Length * sizeof(float)), cubeVertices, BufferUsageHint.DynamicDraw);

            indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new System.IntPtr(cubeIndicies.Length * sizeof(uint)), cubeIndicies, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(-7.0f, 5.0f, -7.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            GL.Disable(EnableCap.DepthTest);
            grid.Render();
            GL.Enable(EnableCap.DepthTest);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

            // 0 bytes from the begenning of the buffer
            GL.VertexPointer(3, VertexPointerType.Float, 0, new System.IntPtr(0));
            // The buffer first contains positions, which are 8 vertices, made up of 3 floats each.
            // after that comes the color information, therefore the colors are:
            // 8 * 3 * sizeof(float) bytes away from the begenning of the buffer
            GL.ColorPointer(3, ColorPointerType.Float, 0, new System.IntPtr(sizeof(float)*8*3));

            // The index buffer only contains indices we want to draw, so they are 0 bytes
            // from the begenning of the array. You can use the constant i use here instead
            // of making a new IntPtr, if the offset you are looking for is 0
            GL.DrawElements(PrimitiveType.Triangles, numIndices, DrawElementsType.UnsignedInt, System.IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
        }
        public override void Shutdown() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DeleteBuffer(vertexBuffer);
            GL.DeleteBuffer(indexBuffer);
        }
    }
}
