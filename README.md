## Info

---

This is a WIP framework for C# bindings of <a href="https://github.com/raysan5/raylib">
Raylib</a>, <a href="https://github.com/NotNotTech/Raylib-CsLo">Raylib-CsLo</a>

## Project Structure

---

### the projects have their own documentation

- <a href="https://github.com/SWCreeperKing/RayWork/tree/master/RayWork">RayWork</a>
    - Main project
- <a href="https://github.com/SWCreeperKing/RayWork/tree/master/RayWork.ECS">RayWork.ECS</a>
    - The foundation of the Entity Component System
- <a href="https://github.com/SWCreeperKing/RayWork/tree/master/RayWork.RLImgui">RayWork.RLImgui</a>
    - Bindings for <a href="https://github.com/ocornut/imgui">Imgui</a> using a modified version
      of <a href="https://github.com/raylib-extras/rlImGui-cs">RLImgui</a>
- <a href="https://github.com/SWCreeperKing/RayWork/tree/master/RayWork.SaveSystem">RayWork.SaveSystem</a>
    - A simple standalone save system
- <a href="https://github.com/SWCreeperKing/RayWork/tree/master/RayWork.SelfUpdater">RayWork.SelfUpdater</a>
  - A standalone self updater
- <a href="https://github.com/SWCreeperKing/RayWork/tree/master/RayWorkTester">RayWorkTester</a>
    - A testing project for RayWork
- <a href="https://github.com/SWCreeperKing/RayWork/tree/master/UpdateTester">UpdateTester</a>
    - A testing project for the self updater

## About this project

---

this is a better and more updated rework of my wrapper turned
framework <a href="https://github.com/SWCreeperKing/RayWrapper">RayWrapper</a>

### Styling

---

- ALL Class Scoped Variables
  - UpperCamelCase
  - only exception: variables hidden via setter/getters
- Class Scoped Variables hidden via setter/getters
  - _UpperCamelCase
- Statement 1 liners
  - brackets can be removed ONLY IF it is a return or throw
  - otherwise brackets should remain
- Method Body Expressions
  - no restriction but,
  - if spans to multiple lines than the => must be the the start of the line under the expression