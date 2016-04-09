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
        private class OBJLoaderHelper {
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
        public int LoadOBJ(string objPath) {
            InitCheck("Trying load obj without initializing OBJManager!");
            if (string.IsNullOrEmpty(objPath)) {
                Error("Load OBJ path was invalid");
                throw new ArgumentException(objPath);
            }
            for (int i = 0; i < managedOBJ.Count; i++) {
                if (managedOBJ[i].path == objPath) {
                    managedOBJ[i].refCount++;
                    return i;
                }
                if (managedOBJ[i].refCount <= 0) {
                    managedOBJ[i].refCount++;
                    managedOBJ[i].path = objPath;
                    managedOBJ[i] = new OBJLoader(objPath);
                    return i;
                }
            }
            managedOBJ.Add(new OBJLoader(objPath));
            return managedOBJ.Count - 1; 
        }
        public void Shutdown() {
            
        }
        //unload texture
        #endregion
    }
}
