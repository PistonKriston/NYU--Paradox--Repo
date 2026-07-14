# Features

Component checklist for **C4GD Final**. ✅ = implemented and in the codebase.

---

## Player

- ✅ **Player movement** (`PlayerController.cs`) — 2D platformer controls with horizontal movement, ground detection, jumping (Space), and wall-jumping off vertical surfaces. Uses `Rigidbody2D` velocity and overlap-circle checks for ground/wall layers.
- ✅ **Camera follow** (`FollowPlayer.cs`) — LateUpdate camera that tracks the player's X/Y position while preserving its own Z depth.
- ⚠️ **Legacy player movement** (`Player Movement.cs`) — Older movement script; may be superseded by `PlayerController.cs`. Verify which is attached in scenes before editing.

## Time Travel

- ✅ **Past / present switching** (`TimeTravel.cs`) — Press **E** to shift between timeline layers by translating vertically (`distanceBetweenLevels`). Enemies become static rigidbodies while in the past; the player moves between stacked level copies.

## Combat

- ✅ **Melee attacks** (`Combat.cs`) — Left-click spawns a short-lived sword hitbox attached to the player. **W + click** performs an upward slash variant.
- ✅ **Sword hitbox** (`Sword.cs`) — Trigger collider that applies damage to objects tagged `Enemy` via their `Health` component. Each sword instance damages a given enemy only once per swing, even if multiple colliders overlap.
- ✅ **Health system** (`Health.cs`) — Tracks `currentHP` / `maxHP`; `TakeDamage()` reduces HP and destroys the object at zero.

## Enemies

- ✅ **Ground / flying enemy AI** (`Enemy.cs`) — Patrols or chases the player depending on `flying` flag. Plays attack animation and applies damage over time while the player remains in a trigger zone.
- ✅ **Refactored enemy AI** (`EnemyFixed.cs`) — Cleaner enemy implementation with proper facing toward the player, separate damage tick logic, and ground/flying movement modes.
- ✅ **Patrol turn-around** (`EnemyTurnAround.cs`) — Flips a linked `Enemy` when a left-side ground/wall check fails, so patrol enemies reverse at ledges and walls.

## Game State

- ✅ **Game manager singleton** (`GameManager.cs`) — Persists across scenes (`DontDestroyOnLoad`). Stores player position, past/present state, and enemy positions for save/load.
- ✅ **Save points** (`Save.cs`) — Press **S** while overlapping a save trigger to call `GameManager.SaveGame()`.
- ⚠️ **Load / respawn** — `GameManager.Load()` reloads the active scene; player state restores on start. Enemy position restore is incomplete (known issue).

## UI & Menus

- ✅ **Main menu** (`MainMenu.cs`) — Start game (loads "Level 1"), toggle controls overlay, return to main menu panel.
- ✅ **Pause system** (`PauseResume.cs`) — **Escape** toggles pause (`Time.timeScale = 0`), shows/hides pause screen.
- ✅ **Pause menu** (`PauseMenu.cs`) — In-pause navigation: return to main menu scene, view controls overlay, resume game UI.
- ✅ **Death screen** (`DeathScreen.cs`) — Respawn reloads the scene via `GameManager.Load()`; main menu button loads scene index 2.
- ✅ **UI manager** (`UIManager.cs`) — Activates the death screen overlay when the player is destroyed.

---

## In Progress

_No items yet._

---

## Planned / Known Gaps

- Enemy position restore on load (save data written but not applied).
- Sword attack direction when facing left (`Combat.cs` uses the same offset for both directions).
- `GameManager.enemyPositions` array initialization before save.
