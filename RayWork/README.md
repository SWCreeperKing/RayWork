# Getting Started

---

To get started with using RayWorks, extend the `Scene` class,
for these examples I will be using `Top Level Statements`.
Inorder to create and run the application, make a new `RayApplication`.

`RayApplication` takes the following parameters with * marking the required parameters.

- *Scene mainScene
- *Vector2 windowHeight
    - or int windowWidth, int windowHeight
- string title
- int fps
- ConfigFlags configFlags
    - this is for Raylib's window ConfigFlags, 0 is default

```csharp
new RayApplication(new Program(), new Vector2(1280, 720), "Test Window")

public partial class Program : Scene
{
}
```

Scenes contain 3 `virtual` `void` methods

- `Initialize()`
    - for when the scene initializes for the first time
- `ReInitialize()`
    - for when the scene manager switches back to an already initialized scene
- `UpdateLoop()`
    - this is called every frame before drawing has started
- `RenderLoop()`
    - this is called during the drawing and where you can put Raylib's draw methods
- `DisposeLoop()`
    - this gets called when the program shuts down

# SceneManager

---

`SceneManager` is a static class that allows you to add, and switch scenes.
When switching scenes, the first time a scene is switched to `Initialize()` will be called
otherwise `ReInitialize()` method will be called

# Logging

---

All of the Logging is handled by the `Logger` static class.
When there is a crash the logger will automatically generate a crash log file
that contains the entire program log. 

# Input

---

RayWork also has an `Input` static class. It has 2 public methods:

- `IsKeyDown`
  - returns true if a key is down, false if a key up
- `IsKeyUp`
  - returns the opposite of `IsKeyDown`

With has 3 events:

- `OnKeyPressed`
  - when a key is pressed
- `OnKeyReleased`
  - when a key is released
- `OnKeyRepeat`
  - when a key is repeating

The 2 public properties of the `Input` class are:

- `KeyboardDelaySeconds`
  - how long it takes when holding a key to repeat the key input
- `KeyboardRepeatsPerSecond`
  - how many times the key should repeat per second

So what is this about key repeating? When you are in a text field,
and you hold down a key, that key repeats, this is what the repeating refers to