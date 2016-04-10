using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class OBJManager {
        #region Singleton
        private static OBJManager instance = null;
        public static OBJManager Instance {
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
            public string path = string.Empty;
            public int refCount = 0;
            public OBJLoader loader = null;
        }
        #endregion

        #region HelperVariables
        private List<OBJLoaderHelper> managedOBJ = null;
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
            managedOBJ = new List<OBJLoaderHelper>();
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
            }
            for (int i = 0; i < managedOBJ.Count; i++) {
                if (managedOBJ[i].refCount <= 0) {
                    managedOBJ[i].refCount = 1;
                    managedOBJ[i].path = objPath;
                    managedOBJ[i] = new OBJLoaderHelper();
                    managedOBJ[i].loader = new OBJLoader(objPath);
                    return i;
                }
            }
            managedOBJ.Add(new OBJLoaderHelper());
            managedOBJ[managedOBJ.Count - 1].refCount = 1;
            managedOBJ[managedOBJ.Count - 1].path = objPath;
            managedOBJ[managedOBJ.Count - 1].loader = new OBJLoader(objPath);
            return managedOBJ.Count - 1; 
        }
        public void Shutdown() {
            InitCheck("Trying to shutdown UnInitialized OBJManager!");
            for(int i = 0; i < managedOBJ.Count; i++) {
                if (managedOBJ[i].refCount > 0) {
                    Error("OBJ reference cound is > 0: " + managedOBJ[i].refCount);
                    managedOBJ[i].loader.Destroy();
                    managedOBJ[i].loader = null;
                    managedOBJ[i] = null;
                }
                else if (managedOBJ[i].refCount < 0) {
                    Error("OBJ reference count is < 0, this should never happen!: " + managedOBJ[i].path);
                }
            }
        }
        public void Render(bool useNormals = true, bool useTextures = true) {
            InitCheck("Trying to render without initializing OBJManager!");
            for (int i = 0; i < managedOBJ.Count; i++) {
                managedOBJ[i].loader.Render(useNormals, useTextures);
            }
        }
        //unload obj
        public void UnloadObj(int objID) {
            InitCheck("Trying to unload obj without initializing OBJManager!");
            IndexCheck(objID, "UnloadObj");
            managedOBJ[objID].refCount--;
            if (managedOBJ[objID].refCount == 0) {
                managedOBJ[objID].loader.Destroy();
                managedOBJ[objID].loader = null;
            }
            else if (managedOBJ[objID].refCount < 0) {
                Error("Ref count of obj is less than 0 : " + managedOBJ[objID].path);
            }
        }
        public OBJLoader GetModel(int objID) {
            InitCheck("Trying to retrieve model without initializing OBJManager!");
            IndexCheck(objID, "GetModel");
            if (managedOBJ[objID].loader == null) {
                Error("Trying retrieve null model: " + objID);
            }
            return managedOBJ[objID].loader;
        }
        #endregion
    }
}
