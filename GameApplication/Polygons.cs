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
            */
            float scale = 0.125f;

            // Draw points
            float xOffset = -0.9f;
            float yOffset = 0.9f;
            GL.PointSize(4.0f);
            GL.Begin(PrimitiveType.Points);
            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    // We use 0.9 instead of 1 to go close to the edge, not all the way
                    // Why are we multiplying 4 by 0.125 (scale)?
                    // Because X and Y both go from 0 to 3, a total of 4 units
                    GL.Vertex3(xOffset + x * scale, yOffset - y * scale, 0);
                }
            }
            GL.End();

            // Draw Triangle back facing
            xOffset = -0.25f;
            yOffset = 0.9f;
            GL.Begin(PrimitiveType.Triangles);
            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    GL.Vertex3(xOffset + x * scale, yOffset - y * scale, 0);
                    GL.Vertex3(xOffset + (x + 1) * scale, yOffset - y * scale, 0);
                    GL.Vertex3(xOffset + x * scale, yOffset - (y + 1.0f) * scale, 0);
                }
            }
            GL.End();

            // Set polygons to render as lines
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            // Draw quads
            xOffset = 0.9f;
            yOffset = 0.9f;
            GL.Begin(PrimitiveType.Quads);
            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    GL.Vertex3(xOffset - x * scale, yOffset - y * scale, 0);
                    GL.Vertex3(xOffset - (x + 1) * scale, yOffset - y * scale, 0);
                    GL.Vertex3(xOffset - (x + 1) * scale, yOffset - (y + 1) * scale, 0);
                    GL.Vertex3(xOffset - x * scale, yOffset - (y + 1) * scale, 0);
                }
            }
            GL.End();

            // Draw triangle strip
            xOffset = 0.15f;
            yOffset = -0.5f;
            for (int x = 0; x < 4; x++) {
                GL.Begin(PrimitiveType.TriangleStrip);
                for (int y = 0; y < 4; y++) {
                    GL.Vertex3(xOffset + x * scale, yOffset + y * scale, 0);
                    GL.Vertex3(xOffset + (x + 1) * scale, yOffset + y * scale, 0);
                    GL.Vertex3(xOffset + x * scale, yOffset + (y + 1) * scale, 0);
                    GL.Vertex3(xOffset + (x + 1) * scale, yOffset + (y + 1) * scale, 0);
                }
                GL.End();
            }

            // Draw triangle fan backward facing
            xOffset = -0.15f;
            yOffset = -0.5f;
            GL.Begin(PrimitiveType.TriangleFan);
            // Center of thefan
            GL.Vertex3(xOffset - 0.0f, yOffset + 0.0f, 0.0f);
            // Bottom side
            for (int x = 5; x > 0; x--) {
                GL.Vertex3(xOffset - (x - 1) * scale, yOffset + 4 * scale, 0);
            }
            // Right side
            for (int y = 5; y > 0; y--) {
                GL.Vertex3(xOffset - 4 * scale, yOffset + (y - 1) * scale, 0);
            }
            GL.End();

            // Reset polygon modes for next render
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }
        public void Shutdown() {

        }
    }
}
