using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;


namespace GameApplication {
    class LinesSample :Game {
        public override void Initialize() {

        }
        public override void Update(float dTime) {

        }
        public override void Render() {
            float lineWidth = 0.5f;
            for (float lineY = 1.0f; lineY > -1.0f; lineY -= 0.25f) {
                GL.LineWidth(lineWidth);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(-0.9f, lineY, 0.0f);
                GL.Vertex3(-0.1f, lineY, 0.0f);
                GL.End();
                lineWidth += 1.0f;
            }
        }
        public override void Shutdown() {

        }
    }
}
