using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Math_Implementation;

namespace GameApplication {
    class Plane  : Game{
        public Vector3 n = new Vector3(0f,0f,0f);//plane normal. points x on the plane satisfy dot(n,x)=d
        public float d = 0; //distance from origin, d=dot(n,p)
        public static Plane ComputePlane(Vector3 a, Vector3 b, Vector3 c) {
            Plane p = new Plane();
            p.n = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            p.d = Vector3.Dot(p.n, a);
            return p;
        }
        public static int HalfSpace(Plane p,Vector3 v) {
            /*
            int result = (p.n.X*v.X) + (p.n.Y*v.Y) + (p.n.Z*v.Z) + p.d;
            return result;
            */
            Vector4 v1 = new Vector4(p.n.X, p.n.Y, p.n.Z, p.d);
            Vector4 v2 = new Vector4(v.X, v.Y, v.X, 1);
            return (int)Vector4.Dot(v1, v2);
        }

    }
}
