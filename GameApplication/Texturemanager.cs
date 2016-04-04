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
        private void Error(string error) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = old;
        }
        private void Warning(string error) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(error);
            Console.ForegroundColor = old;
        }
        private void InitCheck(string errorMessage) {
            if (!isInitialized) {
                Error(errorMessage);
            }
        }
        private void IndexCheck(int index,string function) {
            if (managedTextures == null) {
                Error(function + " is trying to access element " + index + " when managedTextures is null");
                return;
            }
            if (index < 0) {
                Error(function + " is trying to access a negative element " + index);
                return;
            }
        }
        private bool IsPowerOfTwo(int x) {
            if (x > 1) {
                while (x % 2 == 0) {
                    x >>= 1;
                }
            }
            return x == 1;
        }
        private int LoadGLTexture(string filename, out int width,out int height, bool nearest) {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            if (nearest) {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }
            else {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if (!IsPowerOfTwo(bmp.Width)) {
                Warning("Texture width non power of two: " + filename);
            }
            if (!IsPowerOfTwo(bmp.Height)) {
                Warning("Texture height non power of two: " + filename);
            }
            if (bmp.Width > 2048) {
                Warning("Texture width > 2048: " + filename);
            }
            if (bmp.Height > 2048) {
                Warning("Texture heigh > 2048: " + filename);
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.Short, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            width = bmp.Width;
            height = bmp.Height;
            return id;
        }
        #endregion

        #region PublicAPI
        public void Initialize() {
            if (isInitialized) {
                Error("Trying to double initialize texture maanger");
            }
            managedTextures = new List<TextureInstance>();
            managedTextures.Capacity = 100;
            isInitialized = true;
        }
        public void Shutdown() {
            InitCheck("Trying to shut down a non initialized texture");

            for (int i = 0; i < managedTextures.Count; i++) {
                if (managedTextures[i].refCount > 0) {
                    Warning("Texture refernce is > 0: " + managedTextures[i].path);

                    GL.DeleteTexture(managedTextures[i].glHandle);
                    managedTextures[i] = null;
                }
                else if (managedTextures[i].refCount < 0) {
                    Error("Texture reference count is < 0, should never happen! " + managedTextures[i].path);

                }
            }

            managedTextures.Clear();
            managedTextures = null;
            isInitialized = false;
        }
        public int LoadTexture(string texturePath, bool UseNearestFiltering = false) {
            InitCheck("Trying to laod texture without initializing TextureManager!");

            if (string.IsNullOrEmpty(texturePath)) {
                Error("Load texture file path was null");
                throw new ArgumentException(texturePath);
            }

            //check if texture is already loaded, increase ref count if it is
            for (int i = 0; i < managedTextures.Count; i++) {
                if (managedTextures[i].path == texturePath) {
                    managedTextures[i].refCount++;
                    return i;
                }
            }

            //try to recycle an old texture handle
            for (int i = 0; i < managedTextures.Count; i++) {
                if (managedTextures[i].refCount <= 0) {
                    managedTextures[i].glHandle = LoadGLTexture(texturePath, out managedTextures[i].width, out managedTextures[i].height, UseNearestFiltering);
                    managedTextures[i].refCount = 1;
                    managedTextures[i].path = texturePath;
                    return i;
                }
            }

            //texture no loaded, no recyclable slots in array
            //load texture into new slot
            TextureInstance newTexture = new TextureInstance();
            newTexture.refCount = 1;
            newTexture.glHandle = LoadGLTexture(texturePath, out newTexture.width, out newTexture.height, UseNearestFiltering);
            newTexture.path = texturePath;
            managedTextures.Add(newTexture);

            return managedTextures.Count - 1;
        }
        public void UnloadTexture(int textureId) {
            InitCheck("Trying to unload texture without initializing TextureManager!");
            IndexCheck(textureId, "UnloadTexture");

            managedTextures[textureId].refCount--;
            if (managedTextures[textureId].refCount == 0) {
                GL.DeleteTexture(managedTextures[textureId].glHandle);
            }
            else if (managedTextures[textureId].refCount < 0) {
                Error("Ref count of texture is less than 0: " + managedTextures[textureId].path);
            }
        }
        public int GetTextureWidth(int textureId) {
            InitCheck("Trying to access texture width without initializing TextureManager!");
            IndexCheck(textureId, "GetTextureWidth");

            return managedTextures[textureId].width;
        }
        public int GetTextureHeight(int textureId) {
            InitCheck("Trying to access texture height without initializing TextureManager!");
            IndexCheck(textureId, "GetTextureHeight");

            return managedTextures[textureId].height;
        }
        public Size GetTextureSize(int textureId) {
            InitCheck("Trying to access texture size without intiializing TextureManager!");
            IndexCheck(textureId, "GetTextureSize");

            return new Size(managedTextures[textureId].width, managedTextures[textureId].height);
        }
        public int GetGLHandle(int textureId) {
            InitCheck("Trying to access texture handle without initializing TextureManager!");
            IndexCheck(textureId, "GetGLHandle");

            return managedTextures[textureId].glHandle;
        }
        #endregion

    }
}
