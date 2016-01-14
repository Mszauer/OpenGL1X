using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GameApplication {
    class MainGameWindow : OpenTK.GameWindow {
        //global reference
        public static OpenTK.GameWindow Window = null;
        public static bool IsFullScreen = false;
        public static Game TheGame = null;
        public static Grid Axiis = null;

        public static void Initialize(object sender, EventArgs e) {
            TheGame.Initialize();
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Version));
        }
        public static void Update(object sender, FrameEventArgs e) {
            float dTime = (float)e.Time;
            TheGame.Update(dTime);
        }
        public static void Render(object sender, FrameEventArgs e) {
            GL.ClearColor(Color.CadetBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //Axiis.Render();
            TheGame.Render();

            Window.SwapBuffers();
        }
        public static void Shutdown(object sender, EventArgs e) {
            TheGame.Shutdown();
        }
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            Rectangle drawingArea = ClientRectangle;
            //do resize logic here
        }
        public static void ToggleFullscreen() {
            if (IsFullScreen) {
                Window.WindowBorder = WindowBorder.Resizable;
                Window.WindowState = WindowState.Normal;
                Window.ClientSize = new Size(800, 600);
            }
            else {
                Window.WindowBorder = WindowBorder.Hidden;
                Window.WindowState = WindowState.Fullscreen;
            }
            IsFullScreen = !IsFullScreen;
        }
        [STAThread]
        public static void Main(string[] args) {
            //create new window
            Window = new MainGameWindow();
            Axiis = new Grid();
            TheGame = new LookAtSample();
            Window.Load += new EventHandler<EventArgs>(Initialize);
            Window.UpdateFrame += new EventHandler<FrameEventArgs>(Update);
            Window.RenderFrame += new EventHandler<FrameEventArgs>(Render);
            Window.Unload += new EventHandler<EventArgs>(Shutdown);

            Window.Title = "Game Name";
            Window.ClientSize = new System.Drawing.Size(800, 600);
            Window.VSync = VSyncMode.On;
            //run 60fps
            Window.Run(60.0f);

            //Dispose at end
            Window.Dispose();
        }
    }
}
