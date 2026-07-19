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

### Compile test result
- Stef imported `SportsMatchManager.cs` into the Unity project.
- Unity and UdonSharp completed compilation successfully.
- No red Console errors were reported.

### Learned
The foundation script is syntactically valid in the current Unity 2022.3 LTS and UdonSharp project.

### Next step
Create one empty GameObject named `SportsMatchManager`, add the compiled component, inspect the default fields and perform a local initialization test before connecting any button.