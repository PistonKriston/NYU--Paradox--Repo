# C4GD Final

A 2D time-travel platformer built in Unity for NYU's game development coursework (Paradox team). Shift between past and present timelines to solve platforming puzzles, fight enemies, and progress through layered level geometry.

---

## Project Summary

**C4GD Final** is a side-scrolling action platformer where the core mechanic is **timeline switching**. The present and past exist as vertically offset copies of the same level space. Press **E** to jump between them — changing which platforms, hazards, and enemies are active. Enemies freeze in place when you travel to the past, letting you reposition or bypass threats before returning to the present.

Combat uses a short-range sword spawned on mouse click. Enemies patrol or fly toward the player and deal damage-over-time while in contact. Save points let you checkpoint progress; death reloads the scene from the last saved state.

---

## Getting Started

### Prerequisites

- **Unity 2022.3.62f3** (LTS) — see `ProjectSettings/ProjectVersion.txt`
- Windows or macOS with Unity Hub installed
- Git (project is hosted under the NYU Paradox repo)

### Opening the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/<org>/NYU--Paradox--Repo.git
   ```
2. Open **Unity Hub** → **Add** → select the `C4GD Final` folder inside the cloned repo.
3. Ensure Unity **2022.3.62f3** is selected as the editor version.
4. Open the project and wait for initial import to finish.

### Running the Game

1. Open a scene from `Assets/Scenes/` (e.g. **Level 1** or **Level_1_Map**).
2. Press **Play** in the Unity Editor.
3. From the built main menu scene, use **Start** to load Level 1.

### Controls

| Input | Action |
|-------|--------|
| **A / D** or Arrow keys | Move left / right |
| **Space** | Jump (ground); wall-jump when against a wall |
| **Left Mouse Button** | Sword attack |
| **W + Left Mouse Button** | Upward sword slash |
| **E** | Switch between past and present |
| **S** (at save point) | Save game |
| **Escape** | Pause / resume |

---

## Features

### Player

- 2D platformer movement with acceleration, max speed, and friction (`PlayerController.cs`)
- Ground and wall detection via overlap circles and layer masks
- Wall-jumping off vertical surfaces
- Camera that follows the player (`FollowPlayer.cs`)

### Time Travel

- Toggle between past and present timelines with **E** (`TimeTravel.cs`)
- Vertically stacked level copies separated by a configurable distance
- Enemies become static in the past; player translates between layers

### Combat

- Click-to-attack sword hitbox with timed auto-destroy (`Combat.cs`, `Sword.cs`)
- Optional upward attack variant (**W + click**)
- Health and damage system shared by player and enemies (`Health.cs`)

### Enemies

- Ground patrol enemies with ledge/wall turn-around (`Enemy.cs`, `EnemyTurnAround.cs`)
- Flying enemies that chase the player in 2D (`Enemy.cs` / `EnemyFixed.cs`)
- Attack animations and damage-over-time on player contact
- Refactored enemy controller with cleaner facing and movement (`EnemyFixed.cs`)

### Game State & Persistence

- Singleton game manager persisting across scene loads (`GameManager.cs`)
- Save points — press **S** at triggers to store position and timeline state (`Save.cs`)
- Scene reload for respawn after death (`DeathScreen.cs` → `GameManager.Load()`)

### UI & Menus

- Main menu with start and controls screens (`MainMenu.cs`)
- Pause menu with escape toggle (`PauseResume.cs`, `PauseMenu.cs`)
- Death screen with respawn and main menu options (`DeathScreen.cs`, `UIManager.cs`)

---

## Project Structure

```
C4GD Final/
├── Assets/
│   ├── Scenes/          # Level and menu scenes
│   ├── Scripts/         # All C# gameplay scripts
│   └── Prefabs/         # Sword, enemies, etc.
├── ProjectSettings/     # Unity project configuration
├── PROMPT_LOG.md        # Development prompt history
├── FEATURES.md          # Component checklist
└── README.md            # This file
```

### Key Scripts

| Script | Purpose |
|--------|---------|
| `PlayerController.cs` | Player movement, jumping, wall-jump |
| `TimeTravel.cs` | Past/present timeline switching |
| `Combat.cs` / `Sword.cs` | Melee attack spawning and damage |
| `Health.cs` | HP tracking and death |
| `Enemy.cs` / `EnemyFixed.cs` | Enemy AI and combat |
| `GameManager.cs` | Save/load state singleton |
| `Save.cs` | Save-point trigger interaction |
| `MainMenu.cs` / `PauseMenu.cs` / `DeathScreen.cs` | UI flow |

---

## Known Issues

- `GameManager.enemyPositions` is not initialized before use — saving can throw a null reference error.
- Enemy positions are saved but not restored when reloading a scene.
- Sword local position does not flip when the player faces left (`Combat.cs`).
- `PlayerController.OnDestroy` shows the death screen on any destroy, including scene changes.

See `FEATURES.md` for the full component status and planned work.

---

## Development

This project uses a Cursor rule (`.cursor/rules/project-documentation.mdc`) to keep `PROMPT_LOG.md`, `FEATURES.md`, and this README updated as development continues.

---

## License & Credits

Developed as part of NYU game development coursework by the **Paradox** team. See repository contributors for authorship details.
