# Current Work

## Active feature
`FEATURES/SCOREBOARD`

## Current phase
Inventory

## Current milestone
M1 — Inspect the existing sport-mode system, scoreboard UI and original basketball reference scene before choosing a scoreboard architecture.

## Session goal
Capture the real current setup and identify reusable basketball game logic without modifying the protected basketball prefab or prematurely building a second score system.

## Confirmed
- The repository exists at `mailfromstefanie/Stefanies-VR-Sports-Gym`.
- The world targets VRChat using Unity 2022.3 LTS and UdonSharp.
- Stef reports that the sport hall itself is nearly complete and mainly needs a simple scoreboard/game-round system before public release.
- The current Unity-state evidence is stored under `CURRENT_UNITY_STATE/`:
  - scripts in `CURRENT_UNITY_STATE/SCRIPTS/`;
  - hierarchy screenshots in `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The two existing sport-mode scripts are:
  - `SportsModeManager.cs`;
  - `SportsModeButton.cs`.
- `SportsModeManager` is the shared source of truth for the active sport mode.
- The active mode is manually synchronized through one `[UdonSynced]` integer.
- A player who changes mode takes ownership of the manager object before serialization.
- The manager supports Basketball, Soccer, Volleyball and Soccer Hockey.
- The manager activates and deactivates sport-specific and shared object groups, changes the floor material and refreshes sport-button visuals.
- `SportsModeButton` only sends mode-selection input to the manager and displays active/inactive button visuals.
- This existing split follows the intended architecture: Manager = truth, Button = input, Visual = display.
- A scoreboard UI already exists in the current scene and hierarchy screenshots have been archived, but its exact Text/TextMeshPro components and Inspector bindings are not yet confirmed.
- The basketball package contains its own prefab and scripts. It is protected external/reference content and must not be edited or replaced casually.
- Stef copied only the basketball scoreboard UI into the sport-hall scene.
- The original basketball test scene and its dependencies are archived as read-only reference material at:
  `REFERENCE_PACKAGES/Basketball/Basketball_TestScene_Inspection.unitypackage`
- Stef's current desired experience is deliberately small in scope:
  - a match of about 10 minutes;
  - score tracking during the round;
  - a clear winner at the end;
  - a particle celebration and cheering sound.
- Showing an individual winning player's name has been proposed, but is not yet approved because team membership and player registration may make this significantly more complex.

## Not yet confirmed
- The exact hierarchy and Inspector references for `SportsModeManager` and `SportsModeButton` in the live sport-hall scene.
- Which Text or TextMeshPro components the copied scoreboard UI uses.
- Which controls already exist on the copied scoreboard UI.
- The internal architecture of the original basketball test scene.
- Which basketball scripts own score, timer, player/team registration, winner selection, networking and presentation.
- Whether the basketball package exposes safe public methods or values that the sport-hall scoreboard can read or call.
- Whether any basketball logic can be reused across the other sports without coupling the entire world to the basketball prefab.
- Whether the first public release uses teams/sides or named individual players.
- What happens on a draw.
- What happens to an active match when the sport mode changes.
- Who may start, reset or operate a match.
- Exact late-joiner and ownership behaviour for the future match state.

## Current problem
The sport-mode architecture is now partly understood, but the basketball reference package has not yet been unpacked and inspected. Designing a new score or timer system before that inspection could duplicate working logic, create two sources of truth or require unsafe changes to a protected prefab.

## Open questions
1. Which scenes, prefabs and scripts are actually contained in `Basketball_TestScene_Inspection.unitypackage`?
2. Which component is the source of truth for basketball score and match time?
3. How does the basketball package identify players, teams and a winner?
4. Which parts can be reused or observed without modifying the basketball prefab?
5. What controls and text fields already exist in Stef's copied scoreboard UI?
6. Should the first release show Team/Side A versus Team/Side B, or individual player names?
7. What should happen when the active sport mode changes during or after a match?
8. Who may operate start, reset and score controls?

Discuss or inspect one question at a time.

## Risks
- Modifying or breaking the original basketball prefab and package logic.
- Creating a second source of truth for basketball score or match time.
- Coupling every sport to basketball-specific scripts.
- Mixing UI state with match state.
- Networking that fails for late joiners, ownership transfer or a departing master.
- Adding player-name and team-registration complexity before the basic round works.
- Expanding scope and delaying a nearly finished world.

## Exact next step for Nova
Inspect `REFERENCE_PACKAGES/Basketball/Basketball_TestScene_Inspection.unitypackage` as read-only reference material. Produce an inventory of contained scenes, prefabs and scripts, then identify the likely score, timer, player/team, networking and winner-presentation responsibilities. Do not propose Unity implementation until this inspection is complete.

## Exact next step for Stef
Do not change the basketball prefab, scoreboard hierarchy or sport-mode scripts while the reference package is being inspected. No additional upload is needed unless the package proves incomplete.

## Do not do yet
- Do not create a new ScoreManager or MatchManager.
- Do not connect scoreboard buttons.
- Do not add new synced score or timer variables.
- Do not replace or edit the basketball prefab scripts.
- Do not alter the Unity hierarchy.
- Do not decide that individual player names are required for the first release.
- Do not begin winner particles or sound implementation yet.
