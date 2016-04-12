using System;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using OpenTK.Input;

namespace GameApplication {
    class CameraExample : Game {
        Grid grid = null;
        OBJLoader model = null;
        Plane cameraPlane = null;
        //todo:set based on camera input
        protected Matrix4 viewMatrix = new Matrix4();

        protected float Yaw = 0f; //y rotation of camera
        protected float Pitch = 0f; //x rotation of camera
        //orientation = roll*pitch*yaw
        protected Vector3 CameraPosition = new Vector3(0, 0, 10);
        protected Vector2 LastMousePosition = new Vector2();

        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;
            Matrix4 perspective = Matrix4.Perspective(60.0f, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
        public override void Initialize() {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            Resize(MainGameWindow.Window.Width, MainGameWindow.Window.Height);

            grid = new Grid(true);
            model = new OBJLoader("Assets/test_object.obj");

            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.0f, 0.5f, 0.5f, 0.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1f, 1f, 1f, 1f });
        }
        public override void Shutdown() {
            model.Destroy();
        }
        public override void Update(float dTime) {
            viewMatrix = Move3DCamera(dTime);
        }
        public override void Render() {
            GL.LoadMatrix(Matrix4.Transpose(viewMatrix).Matrix);

            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.DepthTest);
            grid.Render();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            if (Plane.HalfSpace(cameraPlane,new Vector3(0f,0f,0f)) >= 0) {
                GL.Color3(1f, 1f, 1f);
                model.Render(true, false);
            }
            else {
                Console.WriteLine("Green susane culled");
            }
        }
        Matrix4 Move3DCamera(float timeStep, float moveSpeed = 10.0f) {
            //helper variables for mouse position
            const float mouseSensitivity = .01f;
            MouseState mouse = OpenTK.Input.Mouse.GetState();
            KeyboardState keyboard = OpenTK.Input.Keyboard.GetState();

            //figure out delta mouse movement
            Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
            var mouseMove = mousePosition - LastMousePosition;
            LastMousePosition = mousePosition;

            //if left is pressed, update yaw and pitch
            if (mouse.LeftButton == ButtonState.Pressed) {
                Yaw -= mouseSensitivity * mouseMove.X;
                Pitch -= mouseSensitivity * mouseMove.Y;
                if (Pitch < -90f) {
                    Pitch = -90f;
                }
                else if (Pitch > 90f) {
                    Pitch = 90f;
                }
            }

            //now that we have yaw and pitch, create an orientation
            Matrix4 pitch = Matrix4.XRotation(Pitch);
            Matrix4 yaw = Matrix4.YRotation(Yaw);
            Matrix4 orientation = /*rol * */pitch * yaw;

            //get the orientations right and forwards vectors
            Vector3 right = Matrix4.MultiplyVector(orientation, new Vector3(1f, 0f, 0f));
            Vector3 forward = Matrix4.MultiplyVector(orientation, new Vector3(0f, 0f, 1f));

            //update movement based on WASD
            if (keyboard[OpenTK.Input.Key.W]) {
                CameraPosition += forward * -1f * moveSpeed * timeStep;
            }
            if (keyboard[OpenTK.Input.Key.S]) {
                CameraPosition += forward * moveSpeed * timeStep;
            }
            if (keyboard[OpenTK.Input.Key.A]) {
                CameraPosition += right * -1f * moveSpeed * timeStep;
            }
            if (keyboard[OpenTK.Input.Key.D]) {
                CameraPosition += right * moveSpeed * timeStep;
            }

            //now we have position vector, make position matrix
            Matrix4 position = Matrix4.Translate(CameraPosition);
            //using position and orientation, get camera into world
            Matrix4 cameraWorldPosition = position * orientation;
            //the view matrix is inverse of world
            Matrix4 cameraViewMatrix = Matrix4.Inverse(cameraWorldPosition);

            right = new Vector3(cameraWorldPosition[0, 0], cameraWorldPosition[1, 0], cameraWorldPosition[2, 0]);
            Vector3 left = new Vector3(-right.X, -right.Y, -right.Z);
            Vector3 up = new Vector3(cameraWorldPosition[0, 1], cameraWorldPosition[1, 1], cameraWorldPosition[2, 1]);

            right = Matrix4.MultiplyPoint(cameraWorldPosition, right);
            left = Matrix4.MultiplyPoint(cameraWorldPosition, left);
            up = Matrix4.MultiplyPoint(cameraWorldPosition, up);

            cameraPlane = Plane.ComputePlane(left, right, up);

            return cameraViewMatrix;
        }
    }
}
