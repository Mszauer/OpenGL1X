using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class TextureManager {
        #region Singleton
        private static TextureManager instance = null;
        public static TextureManager Instance {
            get {
                if (instance == null) {
                    instance = new TextureManager();
                }
                return instance;
            }
        }
        private TextureManager() {

        }
        #endregion
        #region HelperClass
        private class TextureInstance {
            public int glHandle = -1;
            public string path = string.Empty;
            public int refCount = 0;
            public int width = 0;
            public int height = 0;
        }
        #endregion

        #region MemberVariables
        private List<TextureInstance> managedTextures = null;
        private bool isInitialized = false;
        #endregion

        #region HelperFunctions
        private void Error(string error) { }
        private void Warning(string error) { }
        private void initCheck(string errorMessage) { }
        private void IndexCheck(int index,string function) { }
        private bool IsPowerOfTwo(int x) { }
        private int LoadGLTexture(string filename, out int width,out int height, bool nearest) { }
        #endregion

        #region PublicAPI
        public void Initialize() { }
        public void Shutdown() { }
        public int LoadTexture(string texturePath, bool UseNearestFiltering = false) { }
        public void UnloadTexture(int textureId) { }
        public int GetTextureWidth(int textureId) { }
        public int GetTextureHeight(int textureId) { }
        public Size GetTextureSize(int textureId) { }
        public int GetGLHandle(int textureId) { }
        #endregion

    }
}
