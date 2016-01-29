using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;

namespace GameApplication {
    class Grid {
        public bool RenderSolid = false;
        public Grid() {
            RenderSolid = false;
        }
        public Grid(bool solid) {
            RenderSolid = true;
        }
        public void Render(int subdiv = 0) {
            // Draw grid
            if (RenderSolid) {
                GL.Color3(0.4f, 0.4f, 0.4f);
                GL.Begin(PrimitiveType.Triangles);
                {
                    GL.Normal3(0.0f, 1.0f, 0.0f);
                    SubdivideQuad(-10, 10, -10, 10, -0.01f, 0, subdiv);

                }
                GL.End();
            }
            // Set render color to gray
            GL.Color3(0.5f, 0.5f, 0.5f);
            // Draw a 40x40 grid at 0.5 intervals
            // The grid will go from -10 to +10
            GL.Begin(PrimitiveType.Lines);
            // Draw horizontal lines
            for (int x = 0; x < 40; ++x) {
                float xPos = (float)x * 0.5f - 10.0f;
                GL.Vertex3(xPos, 0, -10);
                GL.Vertex3(xPos, 0, 10);
            }
            // Draw vertical lines
            for (int z = 0; z < 40; ++z) {
                float zPos = (float)z * 0.5f - 10.0f;
                GL.Vertex3(-10, 0, zPos);
                GL.Vertex3(10, 0, zPos);
            }
            GL.End();

            // Draw basis vectors
            GL.Begin(PrimitiveType.Lines);
            // Set the color to R & draw X axis
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            // Set the color to G & draw the Y axis
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
            // Set the color to B & draw the Z axis
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.End();
        }
        private static void SubdivideQuad(float l, float r, float t, float b, float y, int subdivLevel, int target) {
            if (subdivLevel >= target) {
                GL.Vertex3(l, y, t);
                GL.Vertex3(l, y, b);
                GL.Vertex3(r, y, b);

                GL.Vertex3(l, y, t);
                GL.Vertex3(r, y, b);
                GL.Vertex3(r, y, t);
            }
            else {
                float half_width = Math.Abs(r - l) * 0.5f;
                float half_height = Math.Abs(b - t) * 0.5f;

                SubdivideQuad(l, l + half_width, t, t + half_height, y, subdivLevel + 1, target);
                SubdivideQuad(l + half_width, r, t, t + half_height, y, subdivLevel + 1, target);
                SubdivideQuad(l, l + half_width, t + half_height, b, y, subdivLevel + 1, target);
                SubdivideQuad(l + half_width, r, t + half_height, b, y, subdivLevel + 1, target);
            }
        }
    }
}
