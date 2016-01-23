using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class Primitives {
        // Cached gemetry to speed up rendering!
        protected class tshape {
            public float rr;
            public float tr;
            public int nc;
            public int nt;
        }
        protected static Dictionary<int, List<float>> sphereData = new Dictionary<int, List<float>>();
        protected static List<KeyValuePair<tshape, List<float>>> torusData = new List<KeyValuePair<tshape, List<float>>>();

        public static void DrawSphere(int ndiv = 2) {
            float radius = 1.0f;
            if (!sphereData.ContainsKey(ndiv)) {
                sphereData[ndiv] = new List<float>();
                for (int i = 0; i < 20; i++) {
                    DrawTriangle(vdata[tindices[i][0]], vdata[tindices[i][1]], vdata[tindices[i][2]], ndiv, radius, ndiv);
                }
            }

            GL.Begin(PrimitiveType.Triangles);
            int x = 0;
            List<float> data = sphereData[ndiv];
            while (x < data.Count) {
                GL.Normal3(data[x++], data[x++], data[x++]);
                GL.Vertex3(data[x++], data[x++], data[x++]);

                GL.Normal3(data[x++], data[x++], data[x++]);
                GL.Vertex3(data[x++], data[x++], data[x++]);

                GL.Normal3(data[x++], data[x++], data[x++]);
                GL.Vertex3(data[x++], data[x++], data[x++]);

            }
            GL.End();
        }

        public static void Torus(float ringRadius = 0.2f, float tubeRadius = 0.8f, int numc = 6, int numt = 12) {
            List<float> data = null;
            foreach (KeyValuePair<tshape, List<float>> kvp in torusData) {
                if (kvp.Key.rr == ringRadius && kvp.Key.tr == tubeRadius && kvp.Key.nc == numc && kvp.Key.nt == numt) {
                    data = kvp.Value;
                }
            }
            if (data == null) {
                tshape key = new tshape();
                key.rr = ringRadius;
                key.tr = tubeRadius;
                key.nc = numc;
                key.nt = numt;

                KeyValuePair<tshape, List<float>> newItem = new KeyValuePair<tshape, List<float>>(key, new List<float>());
                torusData.Add(newItem);
                data = torusData[torusData.Count - 1].Value;

                int i, j, k;
                double s, t, x, y, z, twopi;

                twopi = 2.0 * (double)Math.PI;
                for (i = 0; i < numc; i++) {
                    for (j = 0; j <= numt; j++) {
                        for (k = 1; k >= 0; k--) {
                            s = (i + k) % numc + 0.5;
                            t = j % numt;

                            x = (tubeRadius + ringRadius * Math.Cos(s * twopi / numc)) * Math.Cos(t * twopi / numt);
                            y = (tubeRadius + ringRadius * Math.Cos(s * twopi / numc)) * Math.Sin(t * twopi / numt);
                            z = ringRadius * Math.Sin(s * twopi / numc);

                            float[] n = { (float)x, (float)y, (float)z };
                            n = Normalize(n);

                            //GL.Normal3(n);
                            data.Add(n[0]);
                            data.Add(n[1]);
                            data.Add(n[2]);
                            //GL.Vertex3(x, y, z);
                            data.Add((float)x);
                            data.Add((float)y);
                            data.Add((float)z);
                        }
                    }
                }
            }

            GL.Begin(PrimitiveType.QuadStrip);
            int a = 0;
            while (a < data.Count) {
                GL.Normal3(data[a++], data[a++], data[a++]);
                GL.Vertex3(data[a++], data[a++], data[a++]);
            }
            GL.End();
        }

        public static void Cube() {
            GL.Begin(PrimitiveType.Triangles);

            // bottom face
            GL.Normal3(0.0f, -1.0f, 0.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);

            //top face
            GL.Normal3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);

            //right face
            GL.Normal3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            //front face
            GL.Normal3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            //left face
            GL.Normal3(-1.0f, 0.0f, 0.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);

            //back face
            GL.Normal3(0.0f, 0.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            GL.End();
        }

        private static float X = 0.525731112119133606f;
        private static float Z = 0.850650808352039932f;

        private static float[][] vdata = new float[][] {
            new float[]{-X, 0.0f, Z}, new float[]{X, 0.0f, Z},
            new float[]{-X, 0.0f, -Z}, new float[]{X, 0.0f, -Z},
            new float[]{0.0f, Z, X}, new float[]{0.0f, Z, -X},
            new float[]{0.0f, -Z, X}, new float[]{0.0f, -Z, -X},
            new float[]{Z, X, 0.0f}, new float[]{-Z, X, 0.0f},
            new float[]{Z, -X, 0.0f}, new float[]{-Z, -X, 0.0f}
        };

        private static int[][] tindices = new int[][] {
            new int[]{0,4,1}, new int[]{0,9,4}, new int[]{9,5,4},
            new int[]{4,5,8}, new int[]{4,8,1}, new int[]{8,10,1},
            new int[]{8,3,10}, new int[]{5,3,8}, new int[]{5,2,3},
            new int[]{2,7,3}, new int[]{7,10,3}, new int[]{7,6,10},
            new int[]{7,11,6}, new int[]{11,0,6}, new int[]{0,1,6},
            new int[]{6,1,10}, new int[]{9,0,11}, new int[]{9,11,2},
            new int[]{9,2,5}, new int[]{7,2,11}
        };

        private static float[] Normalize(float[] float3) {
            float len = (float)Math.Sqrt(float3[0] * float3[0] + float3[1] * float3[1] + float3[2] * float3[2]);
            if (len != 0.0f) {
                float3[0] /= len;
                float3[1] /= len;
                float3[2] /= len;
            }
            return float3;
        }

        private static void DrawTriangle(float[] a, float[] b, float[] c, int div, float r, int ndiv) {
            if (div <= 0) {
                //GL.Normal3(b); GL.Vertex3(b[0] * r, b[1] * r, b[2] * r);
                sphereData[ndiv].Add(b[0]); sphereData[ndiv].Add(b[1]); sphereData[ndiv].Add(b[2]);
                sphereData[ndiv].Add(b[0] * r); sphereData[ndiv].Add(b[1] * r); sphereData[ndiv].Add(b[2] * r);
                //GL.Normal3(a); GL.Vertex3(a[0] * r, a[1] * r, a[2] * r);
                sphereData[ndiv].Add(a[0]); sphereData[ndiv].Add(a[1]); sphereData[ndiv].Add(a[2]);
                sphereData[ndiv].Add(a[0] * r); sphereData[ndiv].Add(a[1] * r); sphereData[ndiv].Add(a[2] * r);
                //GL.Normal3(c); GL.Vertex3(c[0] * r, c[1] * r, c[2] * r);
                sphereData[ndiv].Add(c[0]); sphereData[ndiv].Add(c[1]); sphereData[ndiv].Add(c[2]);
                sphereData[ndiv].Add(c[0] * r); sphereData[ndiv].Add(c[1] * r); sphereData[ndiv].Add(c[2] * r);
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
                DrawTriangle(a, ab, ac, div - 1, r, ndiv);
                DrawTriangle(b, bc, ab, div - 1, r, ndiv);
                DrawTriangle(c, ac, bc, div - 1, r, ndiv);
                DrawTriangle(ab, bc, ac, div - 1, r, ndiv);
            }
        }
    }
}