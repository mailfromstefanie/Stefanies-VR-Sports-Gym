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

### Deliberately not built yet
- No Unity GameObject or component was added.
- No scoreboard button was connected.
- No Join, Leave, score, timer, goal or reset action was implemented.
- The SoccerBox football was not changed.

### Safety boundary
This is only the compile-test foundation. The next step must first verify that Unity and UdonSharp import and compile this single script without errors.

### Next step
Copy or pull `SportsMatchManager.cs` into the Unity project, let Unity compile, and inspect the Console before adding the manager to the hierarchy.