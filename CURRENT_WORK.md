# Current Work

## Active feature
`FEATURES/SCOREBOARD`

## Current phase
Inventory

## Current milestone
M1 — Inspect the existing sport-mode scripts and scoreboard UI.

## Session goal
Capture the real current setup before choosing or writing a scoreboard architecture.

## Confirmed
- The repository has been created.
- The world is intended for VRChat using Unity 2022.3 LTS and UdonSharp.
- Stef reports that the sport hall itself is nearly complete.
- Stef reports that two existing scripts switch between sport modes.
- Stef reports that a scoreboard UI already exists.

## Not yet confirmed
- Script names and exact responsibilities.
- How the two sport-mode scripts communicate.
- Current Unity hierarchy and Inspector references.
- Which Text or TextMeshPro components the scoreboard uses.
- Whether scores must be global, who may edit them and what happens on mode switch.
- Existing networking and ownership behaviour.

## Current problem
The real scripts and Unity setup have not yet been inspected. Designing from memory would risk duplicating or breaking the existing mode system.

## Open questions
1. What are the two existing sport-mode scripts, exactly?
2. Which GameObjects hold them and which Inspector references are assigned?
3. What controls and text fields already exist in the scoreboard UI?
4. What should happen to scores when switching sport modes?
5. Who may operate the scoreboard?

Discuss or inspect one question at a time.

## Risks
- Accidentally creating a second source of truth for the active sport mode.
- Mixing UI state with game state.
- Networking that fails for late joiners or after ownership changes.
- Expanding scope and delaying a nearly finished world.

## Exact next step for Stef
Provide the first of the two existing sport-mode scripts as text or a `.cs` file. Do not change it yet.

## Do not do yet
- Do not create a ScoreManager.
- Do not connect buttons.
- Do not add synced variables.
- Do not replace either existing sport-mode script.
- Do not alter the Unity hierarchy.
