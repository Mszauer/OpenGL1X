using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace GameApplication {
    class DrawElements : Game{
        Grid grid = null;
        float[] cubeVertices = null;
        float[] cubeColors = null;
        uint[] cubeIndicies = null;

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

            cubeVertices = new float[] {
                -1.0f,-1.0f,1.0f,//vertex 1
                1.0f,-1.0f,1.0f,//vertex2
                1.0f,1.0f,1.0f,//vertex3
                -1.0f,1.0f,1.0f,//vertex4
                -1.0f,-1.0f,-1.0f,//vertex5
                1.0f,-1.0f,-1.0f,//vertex6
                1.0f,1.0f,-1.0f,//vertex7
                -1.0f,1.0f,-1.0f//vertex8
            };

            cubeColors = new float[] {
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
                //left
                4,5,1,//left triangle1
                1,0,4,//left triangle2
                //right
                3,2,6,//right triangle1
                6,7,3//right triangle2
            };
        }
        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(-7.0f, 5.0f, -7.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            GL.Disable(EnableCap.DepthTest);
            grid.Render();
            GL.Enable(EnableCap.DepthTest);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, cubeVertices);
            GL.ColorPointer(3, ColorPointerType.Float, 0, cubeColors);

            GL.DrawElements(PrimitiveType.Triangles, cubeIndicies.Length, DrawElementsType.UnsignedInt, cubeIndicies);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
        }
    }
}
