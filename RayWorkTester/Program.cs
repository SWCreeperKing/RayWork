using System.Numerics;
using Raylib_cs;
using RayWork;
using RayWorkTester;

const string title = "Test";
const int fps = 60;
const int windowWidth = 1280;
const int windowHeight = 720;

var app = new RayApplication(new MainScene(), new Vector2(windowWidth, windowHeight),
    title, fps, ConfigFlags.FLAG_WINDOW_RESIZABLE);