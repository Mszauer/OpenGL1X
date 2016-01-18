using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Implementation {
    class MatrixStack {
        protected List<Matrix4> stack = null;
        public MatrixStack() {
            stack = new List<Matrix4>();
            stack.Add(new Matrix4());
        }
        public void Push() {
            //new matrix
            Matrix4 top = new Matrix4();
            //copy values of top matrix
            for (int i = 0; i < stack.Count; i++) {
                top[i] = stack[stack.Count-1][i];
            }
            //add new copy to top
            stack.Add(top);
        }
        public void Load(Matrix4 matrix) {
            stack[stack.Count - 1] = matrix;
        }
        public void Mul(Matrix4 matrix) {
            stack[stack.Count - 1] = stack[stack.Count - 1] * matrix;
        }
        public float[] OpenGL {
            get {
                return stack[stack.Count - 1].OpenGL;
            }
        }
    }
}
