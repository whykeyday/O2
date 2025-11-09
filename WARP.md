# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

**WooL_DaiK_DIGF3007_AtG** is a Unity 2D interactive narrative game with branching story paths. Players navigate through four distinct story paths (Bicycle, Fire, Human, Sun) and must complete all four to reach the ending.

- **Unity Version**: 2022.3.62f3
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Target Platforms**: Standalone and WebGL
- **Key Package**: TextMeshPro 3.0.6

## Opening and Running the Project

### Opening in Unity
```powershell
# Open Unity Hub and add the project from:
"F:\OCADU Projects\DIGF_3007\WooL_DaiK_DIGF3007_AtG (unity files)\WooL_DaiK_DIGF3007_AtG"

# Or launch directly with Unity 2022.3.62f3:
& "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe" -projectPath "F:\OCADU Projects\DIGF_3007\WooL_DaiK_DIGF3007_AtG (unity files)\WooL_DaiK_DIGF3007_AtG"
```

### Running in Play Mode
1. Open the `Assets/Scenes/Title.unity` scene (entry point)
2. Press Play in Unity Editor (Ctrl+P)
3. Navigate through: Title → Main → Story Paths → Ending → Credits

### Building the Game

**Windows Standalone Build:**
```
File → Build Settings → PC, Mac & Linux Standalone
- Target Platform: Windows
- Architecture: x86_64
- Click "Build" or "Build and Run"
```

**WebGL Build:**
```
File → Build Settings → WebGL
- Click "Switch Platform" if needed
- Click "Build" or "Build and Run"
```

Note: WebGL builds automatically disable certain UI elements (Exit button) via `DisableOnWebGL.cs` component.

## Code Architecture

### Core Game Systems

**1. Progress Tracking System**
- **File**: `Assets/Scripts/StaticData.cs`
- **Purpose**: Global static flags tracking completion of all four story paths
- **State Variables**: `bicyclePath`, `firePath`, `humanPath`, `sunPath` (all static bools)
- **Important**: This is a singleton-like pattern without DontDestroyOnLoad - state persists only during runtime

**2. Scene Navigation System**
- **File**: `Assets/Scripts/ChangePageSCR.cs`
- **Purpose**: Handles all scene transitions and branching logic
- **Methods**:
  - `gotoPreviousPage()`: Back navigation
  - `gotoOption1()` through `gotoOption4()`: Story branch choices
- **Ending Logic**: `gotoOption1()` checks if all four paths are completed before allowing ending access
- **State Reset**: `gotoOption4()` resets all path flags when leaving the Ending scene

**3. Audio System**
- **BGMManager.cs**: Persistent singleton audio manager using DontDestroyOnLoad
  - Scene-specific music mapping via `SceneBGM` structs
  - Automatically switches tracks on scene load
  - Single instance enforced in `Awake()`
- **BGMPlayer.cs**: Simple per-scene audio player for standalone scenes
- **Path Completion Tracking**:
  - `ClearedPath.cs`: Marks specific story paths as completed
  - Checks scene names: "Bicycle3", "Fire3", "Human4", "Sun2"

**4. Platform-Specific Code**
- **DisableOnWebGL.cs**: Automatically disables GameObjects on WebGL builds (used for Exit button)
- **MainMenuController.cs**: Handles application quit with WebGL/Editor conditional compilation

### Scene Structure

```
Title.unity          → Entry point, game title screen
Main.unity           → Main hub with four path choices
├─ Bicycle Path/     → Bicycle story scenes (Bicycle1, Bicycle2, Bicycle3)
├─ Fire Path/        → Fire story scenes (Fire1, Fire2, Fire3)
├─ Human Path/       → Human story scenes (Human1-4)
└─ Sun Path/         → Sun story scenes (Sun1, Sun2)
Ending.unity         → Unlocked after completing all four paths
Credits.unity        → Game credits
Template.unity       → Scene template for new pages
```

### Key Patterns

**Scene-Based Architecture**: No complex state management or save system - relies on static variables for runtime progress tracking only. Progress is lost on application restart.

**Navigation Pattern**: All navigation is centralized through `ChangePageSCR` component attached to UI buttons. Each scene has serialized string fields pointing to connected scenes.

**Audio Pattern**: Single persistent `BGMManager` survives scene loads and automatically switches music based on scene name mappings configured in the Unity Inspector.

## Working with Scripts

### Adding New Story Scenes
1. Duplicate `Template.unity` scene
2. Add `ChangePageSCR` component to UI buttons
3. Configure scene name strings in Inspector
4. If this is a path completion scene, add `ClearedPath` component and call `pathCompleted()` on the appropriate button
5. Update `BGMManager` scene mappings if using different music

### Testing Specific Paths
To test the ending without playing through all paths, temporarily modify `StaticData.cs` to initialize all flags as `true`:
```csharp
public static bool bicyclePath = true;
public static bool firePath = true;
public static bool humanPath = true;
public static bool sunPath = true;
```

### Platform-Specific Features
When adding UI that should not appear in WebGL builds:
1. Add `DisableOnWebGL.cs` component to the GameObject
2. For conditional code, use: `#if UNITY_WEBGL && !UNITY_EDITOR`

## Project Settings

**Render Pipeline**: Universal Render Pipeline configured via:
- `Assets/Settings/UniversalRP.asset`
- `Assets/Settings/Renderer2D.asset`

**Default Resolution**:
- Standalone: 1920x1080
- WebGL: 960x600

## Git Workflow

Standard Unity `.gitignore` is configured. The following are tracked:
- `Assets/` (except built-in generated files)
- `ProjectSettings/`
- `Packages/manifest.json`

Ignored folders:
- `Library/`, `Temp/`, `Obj/`, `Build/`, `Builds/`, `Logs/`, `UserSettings/`
