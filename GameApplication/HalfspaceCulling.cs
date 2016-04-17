using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Math_Implementation;

namespace GameApplication {
    class Plane {
        public Vector3 n = new Vector3(0f,0f,0f);//plane normal. points x on the plane satisfy dot(n,x)=d
        public float d = 0; //distance from origin, d=dot(n,p)
        public static Plane ComputePlane(Vector3 a, Vector3 b, Vector3 c) {
            Plane p = new Plane();
            p.n = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            p.d = Vector3.Dot(p.n, a);
            return p;
        }
        public static float HalfSpace(Plane p,Vector3 v) {
            return (p.n.X*v.X) + (p.n.Y*v.Y) + (p.n.Z*v.Z) + p.d;
            /*
            Vector4 N = new Vector4(p.n.X, p.n.Y, p.n.Z, 0f);
            Vector4 PointOnPlane = new Vector4(p.n.X * p.d, p.n.Y * p.d, p.n.Z * p.d, 1f);
            Vector4 V = PointOnPlane - new Vector4(v.X, v.Y, v.X, 1f);
            return (int)Vector4.Dot(N,V);
            */

        }
        public static Plane FromNumbers(Vector4 numbers) {
            Plane p = new Plane();
            p.n = new Vector3();
            p.n.X = numbers.X;
            p.n.Y = numbers.Y;
            p.n.Z = numbers.Z;
            p.d = numbers.W;
            return p;
        }

    }
}
