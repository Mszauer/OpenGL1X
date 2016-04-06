using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class OBJManager {
        #region Singleton
        private static OBJManager instance = null;
        private static OBJManager Instance {
            get {
                if (instance == null) {
                    instance = new OBJManager();
                }
                return instance;
            }
        }
        private OBJManager() {

        }
        #endregion

        #region HelperClass
        private class OBJLoader {
            public int GLHandle = -1;
            public string path = string.Empty;
            public int refCount = 0;
            public int width = 0;
            public int height = 0;
        }
        #endregion

        #region HelperVariables
        private List<OBJLoader> managedOBJ = null;
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
            if (managedOBJ == null) {
                Error(function + " is trying to access element " + index + " when managedOBJ is null");
                return;
            }
            if (index <= 0) {
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

        //TODO LOAD TEXTURE FUTURE
        #endregion

        #region PublicAPI
        public void Initialize() {
            if (isInitialized) {
                Error("Trying to double initialize OBJManager");
            }
            managedOBJ = new List<OBJLoader>();
            managedOBJ.Capacity = 100;
            isInitialized = true;
        }
        public void Shutdown() {
            
        }
        //load texture
        //unload texture
        public int GetOBJWidth(int objID) {
            InitCheck("Trying to access obj width without initializing OBJManager!");
            IndexCheck(objID, "GetOBJWidth");

            return managedOBJ[objID].width;
        }
        public int GetOBJHeight(int objID) {
            InitCheck("Trying to access obj height without initializing OBJManager!");
            IndexCheck(objID, "GetOBJHeight");

            return managedOBJ[objID].height;
        }
        public Size GetOBJSize(int objID) {
            InitCheck("Trying to access obj size without initializing OBJManager!");
            IndexCheck(objID, "GetOBJSize");

            return new Size(managedOBJ[objID].width,managedOBJ[objID].height);
        }
        public int GetGLHandle(int objID) {
            InitCheck("Trying to access OBJ handle without initializing OBJManager!");
            IndexCheck(objID, "GetGLHandle");

            return managedOBJ[objID].GLHandle;
        }
        #endregion
    }
}
