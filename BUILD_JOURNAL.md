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

### Temporary debug visibility
- Added read-only Inspector debug fields for local team, Red player count and Blue player count.
- The temporary fields refresh after initialization, deserialization and accepted team changes.

### Local team registration test result
- `Join Red` produced local team `1`, Red count `1`, Blue count `0`.
- `Join Blue` moved the same player cleanly to local team `2`, Red count `0`, Blue count `1`.
- `Leave Game` returned all three values to `0, 0, 0`.
- No duplicate registration was observed.

### Deliberately not built yet
- No real scoreboard player-name rendering exists yet.
- No two-client synchronization test has been completed yet.
- No score, timer, goal, reset, audio, particles or SoccerBox football changes exist yet.

### Learned
The manager-side team registration flow and `SportsMatchButton` actions work locally in ClientSim. The next smallest step is rendering synchronized Red and Blue player names through one scoreboard view.

### Next step
Add the first minimal `SportsScoreboardView` that reads the manager and refreshes only the Red and Blue player-name texts.