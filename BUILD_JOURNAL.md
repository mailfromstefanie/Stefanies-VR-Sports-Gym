# Build Journal

## 2026-07-19 — Repository initialized

### Built or prepared
- Created the project repository.
- Added the project identity and source-of-truth rules.
- Set the active feature to `FEATURES/SCOREBOARD`.
- Recorded the project in the inventory phase.
- Added initial scoreboard design questions.

### Not built
- No Unity objects were changed.
- No UdonSharp scripts were added.
- No networking architecture was selected.

### Learned
The existing world is reported to be nearly complete, but the two mode-switch scripts and scoreboard UI must be inspected before selecting a solution.

### Next step
Inspect the first existing sport-mode script without editing it.

## 2026-07-19 — Scoreboard implementation started

### Built
- Added `CURRENT_UNITY_STATE/SCRIPTS/SportsMatchManager.cs`.
- Added the five approved internal match-phase constants.
- Added the approved synchronized match snapshot fields.
- Added fixed Red and Blue player-ID arrays with `-1` empty-slot initialization.
- Added read-only getter methods for later buttons and scoreboard views.
- Added owner-only initial snapshot setup and one initial serialization request.

### Compile and component tests
- Stef imported `SportsMatchManager.cs`; Unity and UdonSharp compiled it without red errors.
- Stef created `SportsMatchManager` under `GlobalManagers` with the correct Transform and manual synchronization.
- The default match duration was 600 seconds.
- A local Play Mode initialization test produced no red errors.

### Join and Leave manager logic
- Added `RequestJoinRed()`, `RequestJoinBlue()` and `RequestLeaveGame()`.
- Added team lookup, empty-slot lookup, ownership validation and one serialization request per accepted change.
- Added owner-side removal of departed players through `OnPlayerLeft`.
- Stef imported the update and it compiled without red errors.

### First button helper
- Added `CURRENT_UNITY_STATE/SCRIPTS/SportsMatchButton.cs` for Join Red, Join Blue and Leave Game requests.
- The first version used a nested enum and failed because the installed UdonSharp version does not support nested type declarations.
- Replaced the nested enum with an Inspector integer action: `0 = Join Red`, `1 = Join Blue`, `2 = Leave Game`.
- Stef imported the corrected version and it compiled without red errors.

### Deliberately not built yet
- No real scoreboard button is connected.
- No scoreboard view or player-name rendering exists yet.
- No score, timer, goal, reset, audio, particles or SoccerBox football changes exist yet.

### Learned
The current UdonSharp version requires the action selector to avoid nested enum declarations. The corrected small helper compiles and is ready for an isolated Unity button test.

### Next step
Create one temporary test button for `Join Red`, connect it to `SportsMatchManager`, and verify the Inspector setup before entering Play Mode.