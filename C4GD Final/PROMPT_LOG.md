# Prompt Log

A running record of prompts and outcomes for the **C4GD Final** Unity project.

---

## Log Entries

### 1 — 2026-07-14

**Prompt:** Shared the project Scripts path (`Assets/Scripts`) for exploration.

**Outcome:** Catalogued all 17 C# scripts in the project. Mapped core systems (player movement, time travel, combat, enemies, save/load, UI menus) and identified several issues: uninitialized `enemyPositions` in `GameManager`, incomplete enemy restore on load, sword direction not flipping in `Combat.cs`, duplicate player movement scripts, and death screen triggering on any player destroy.

---

### 2 — 2026-07-14

**Prompt:** Create a Cursor rule that maintains `PROMPT_LOG.md`, `FEATURES.md`, and `README.md` — updating them after every prompt, on completed components, and as the game is built out.

**Outcome:** Added `.cursor/rules/project-documentation.mdc` with always-on instructions for documentation upkeep. Created initial `PROMPT_LOG.md`, `FEATURES.md`, and `README.md` based on the current codebase state.

---

### 3 — 2026-07-14

**Prompt:** Why does the enemy not take damage when the sword is instantiated?

**Outcome:** Diagnosed three causes: enemy prefabs were tagged `Untagged` instead of `Enemy` (so `CompareTag` always failed), child turn-around colliders could be hit instead of the root object, and the sword's dynamic rigidbody with gravity could fall away before overlapping. Fixed prefab tags, updated `Sword.cs` to use `GetComponentInParent<Health>()`, and set the sword rigidbody to kinematic with zero gravity.

---

### 4 — 2026-07-14

**Prompt:** Enemy should only take damage once when the sword collides.

**Outcome:** Updated `Sword.cs` to track damaged enemies per sword instance with a `HashSet<Health>`, preventing multiple hits from the enemy's overlapping child colliders during a single swing.
