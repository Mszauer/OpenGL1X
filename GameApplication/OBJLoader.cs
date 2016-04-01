using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class OBJLoader {
        protected int vertexBuffer = -1;

        protected bool hasNormals = false;
        protected bool hasUvs = false;

        protected int numVerts = 0;
        protected int numNormals = 0;
        protected int numUvs = 0;

        protected string materialPath = null;

        public OBJLoader(string path) {
            List<float> vertices = new List<float>();
            List<float> normals = new List<float>();
            List<float> texCoord = new List<float>();

            List<uint> vertIndex = new List<uint>();
            List<uint> normIndex = new List<uint>();
            List<uint> uvIndex = new List<uint>();

            List<float> vertexData = new List<float>();
            List<float> normalData = new List<float>();
            List<float> uvData = new List<float>();

            using (TextReader reader = File.OpenText(path)) {
                string line = reader.ReadLine();
                while (line != null ) {
                    if (string.IsNullOrEmpty(line)||line.Length < 2) {
                        continue;
                    }
                    line.ToLower();
                    string[] content = line.Split(' ');
                    if (content[0] == "usemtl") {
                        materialPath = content[1];
                    }
                    else if (content[0] == "v") {
                        //add vertex
                        foreach (string vertex in content) {
                            
                        }
                    }
                    else if (content[0] == "vt") {
                        // vertex texture
                        foreach (string vertex in content) {

                        }
                    }
                    else if (content[0] == "vn") {
                        //vertex normal
                        foreach (string vertex in content) {

                        }
                    }
                    else if (content[0] == "f") {
                        //face
                        List<float> faces = new List<float>();
                        for (int i = 1; i < content.Length - 1; i++) { //loop through values
                            content[i].Split('/'); // split based on /
                            foreach (char value in content[i]) { //loop through all chars after splitting
                                faces.Add(System.Convert.ToInt32(value));//add to create linear array?
                            }
                        }
                    }
                    else if (content[0] == "s") {
                        //specular
                    }
                    line = reader.ReadLine();
                }
                
            }
            for (int i = 0; i < vertIndex.Count; i++) {
                vertexData.Add(vertices[(int)vertIndex[i] * 3 + 0]);
                vertexData.Add(vertices[(int)vertIndex[i] * 3 + 1]);
                vertexData.Add(vertices[(int)vertIndex[i] * 3 + 2]);
            }
            for (int i = 0; i < normIndex.Count; i++) {
                normalData.Add(normals[(int)normIndex[i] * 3 + 0]);
                normalData.Add(normals[(int)normIndex[i] * 3 + 1]);
                normalData.Add(normals[(int)normIndex[i] * 3 + 2]);
            }
            for (int i = 0; i < uvIndex.Count; i++) {
                uvData.Add(texCoord[(int)uvIndex[i] * 2 + 0]);
                uvData.Add(texCoord[(int)uvIndex[i] * 2 + 1]);
            }

            hasNormals = normalData.Count > 0;
            hasUvs = uvData.Count > 0;

            numVerts = vertexData.Count;
            numUvs = uvData.Count;
            numNormals = normalData.Count;

            List<float> data = new List<float>();//create linear array of data
            data.AddRange(vertexData); //add everything from vertData
            data.AddRange(normalData);//add everything from normData
            data.AddRange(uvData);//add everything from uvData

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);//bind array
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(data.Count * sizeof(float)), data.ToArray(), BufferUsageHint.StaticDraw);//transfer array to gpu
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);//release array
        }
        public void Destroy() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DeleteBuffer(vertexBuffer);

            vertexBuffer = -1;
        }
        public void Render(bool useNormals = true, bool useTextures = true) {
            if (vertexBuffer == -1) {
                return;
            }
            if (!hasNormals) {
                useNormals = false;
            }
            if (!hasUvs) {
                useTextures = false;
            }
            //enable client states , check arguments
            //bind array buffer
            //set pointers
            //vertex ptr offset = 0;
            //normal ptr offset = numberts * sizeof(float)
            //uv ptr offset = (numverts+numnorms) * sizeof(float)

            //call GL.DrawArrays, always triangles
        }
    }
}
