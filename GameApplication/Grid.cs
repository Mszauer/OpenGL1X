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
        public void Render() {
            // Draw grid
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
    }
}
