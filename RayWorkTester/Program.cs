using Raylib_CsLo;
using RayWork;

new RayApplication(new RayWorkTester.Program(), 1280, 720);

namespace RayWorkTester
{
    public class Program : Scene
    {
        public override void Initialize()
        {
            AddChild(new TestObject());
        }

        public override void UpdateLoop(float deltaTime)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F3)) Debugger.IsDebugging = !Debugger.IsDebugging;
        }
    }
}