using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class Polygons {
        public void Initialize() {

        }
        public void Update(float dTime) {

        }
        public void Render() {
            /*
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            //square 1
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex4(-0.25, -0.25, 0, 1);
            GL.Vertex4(-0.25, 0.25, 0, 1);
            GL.Vertex4(0.25, 0.25, 0, 1);
            GL.Vertex4(0.25, -0.25, 0, 1);
            GL.End();
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Point);
            //square 2
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex4(-0.25, -1, 0, 1);
            GL.Vertex4(-0.25, -0.5, 0, 1);
            GL.Vertex4(0.25, -0.5, 0, 1);
            GL.Vertex4(0.25, -1, 0, 1);
            GL.End();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //square 3
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex4(-0.8, -1, 0, 1);
            GL.Vertex4(-0.8, -0.5, 0, 1);
            GL.Vertex4(-0.3, -0.5, 0, 1);
            GL.Vertex4(-0.3, -1, 0, 1);
            GL.End();
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
            //square 4
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex4(-0.8, -0.25, 0, 1);
            GL.Vertex4(-0.8, 0.25, 0, 1);
            GL.Vertex4(-0.3, 0.25, 0, 1);
            GL.Vertex4(-0.3, -0.25, 0, 1);
            GL.End();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            //square 5
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex4(0.8, -0.25, 0, 1);
            GL.Vertex4(0.8, 0.25, 0, 1);
            GL.Vertex4(0.3, 0.25, 0, 1);
            GL.Vertex4(0.3, -0.25, 0, 1);
            GL.End();
            */
            if (GL.IsEnabled(EnableCap.PolygonSmooth)) {
                GL.Disable(EnableCap.PolygonSmooth);
            }
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex2(0.10, 0);
            GL.Vertex2(0.86, 1);
            GL.Vertex2(0.10, 1);
            GL.End();
            if (!GL.IsEnabled(EnableCap.PolygonSmooth)) {
                GL.Enable(EnableCap.PolygonSmooth);
            }
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex2(-0.10, 0);
            GL.Vertex2(-0.10, 1);
            GL.Vertex2(-0.86, 1);
            GL.End();
        }
        public void Shutdown() {

        }
    }
}
