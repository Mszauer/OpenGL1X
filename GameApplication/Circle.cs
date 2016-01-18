using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class Circle {
        static float X = 0.525731112119133606f;
        static float Z = 0.850650808352039932f;

        static float[][] vdata = new float[][] {
            new float[]{-X, 0.0f, Z}, new float[]{X, 0.0f, Z},
            new float[]{-X, 0.0f, -Z}, new float[]{X, 0.0f, -Z},
            new float[]{0.0f, Z, X}, new float[]{0.0f, Z, -X},
            new float[]{0.0f, -Z, X}, new float[]{0.0f, -Z, -X},
            new float[]{Z, X, 0.0f}, new float[]{-Z, X, 0.0f},
            new float[]{Z, -X, 0.0f}, new float[]{-Z, -X, 0.0f}
        };

        static int[][] tindices = new int[][] {
            new int[]{0,4,1}, new int[]{0,9,4}, new int[]{9,5,4},
            new int[]{4,5,8}, new int[]{4,8,1}, new int[]{8,10,1},
            new int[]{8,3,10}, new int[]{5,3,8}, new int[]{5,2,3},
            new int[]{2,7,3}, new int[]{7,10,3}, new int[]{7,6,10},
            new int[]{7,11,6}, new int[]{11,0,6}, new int[]{0,1,6},
            new int[]{6,1,10}, new int[]{9,0,11}, new int[]{9,11,2},
            new int[]{9,2,5}, new int[]{7,2,11}
        };

        protected static float[] Normalize(float[] float3) {
            float len = (float)Math.Sqrt(float3[0] * float3[0] + float3[1] * float3[1] + float3[2] * float3[2]);
            if (len != 0.0f) {
                float3[0] /= len;
                float3[1] /= len;
                float3[2] /= len;
            }
            return float3;
        }

        protected static void DrawTriangle(float[] a, float[] b, float[] c, int div, float r) {
            if (div <= 0) {
                GL.Normal3(b); GL.Vertex3(b[0] * r, b[1] * r, b[2] * r);
                GL.Normal3(a); GL.Vertex3(a[0] * r, a[1] * r, a[2] * r);
                GL.Normal3(c); GL.Vertex3(c[0] * r, c[1] * r, c[2] * r);
            }
            else {
                float[] ab = new float[] { 0.0f, 0.0f, 0.0f };
                float[] ac = new float[] { 0.0f, 0.0f, 0.0f };
                float[] bc = new float[] { 0.0f, 0.0f, 0.0f };
                for (int i = 0; i < 3; i++) {
                    ab[i] = (a[i] + b[i]) / 2;
                    ac[i] = (a[i] + c[i]) / 2;
                    bc[i] = (b[i] + c[i]) / 2;
                }
                ab = Normalize(ab);
                ac = Normalize(ac);
                bc = Normalize(bc);
                DrawTriangle(a, ab, ac, div - 1, r);
                DrawTriangle(b, bc, ab, div - 1, r);
                DrawTriangle(c, ac, bc, div - 1, r);
                DrawTriangle(ab, bc, ac, div - 1, r);
            }
        }

        public static void DrawSphere(int ndiv) {
            float radius = 1.0f;
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < 20; i++) {
                DrawTriangle(vdata[tindices[i][0]], vdata[tindices[i][1]], vdata[tindices[i][2]], ndiv, radius);
            }
            GL.End();
        }
    }
}
