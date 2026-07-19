# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Implementation — microstep 4 player-list rendering.

The first-release interaction design and technical architecture are approved.

## Current milestone

M3 — Verify local team registration and begin the smallest synchronized scoreboard view.

## Confirmed project setup

- Platform: VRChat Worlds, Unity 2022.3 LTS, UdonSharp.
- Soccer and Soccer Hockey share one football and two goals.
- Switching between those modes does not reset the match.
- Existing scripts are under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Existing screenshots are under `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The protected basketball reference remains read-only.
- `SportsModeManager.cs` remains the synchronized sport-mode truth.
- The current football is a SoccerBox ball with its own `SBBall` synchronization and child `BallTrigger` interaction.

## Approved architecture

- One manually synchronized `SportsMatchManager` owns all match truth.
- Internal phases are `READY`, `PLAYING`, `GOAL_PAUSE`, `SUDDEN_DEATH` and `FINISHED`.
- Red and Blue registrations use fixed synchronized player-ID arrays with `-1` for empty slots.
- The countdown uses one synchronized network end timestamp.
- Goal detectors report only events; the manager validates and blocks duplicates.
- Football reset must preserve SoccerBox, `SBBall`, `BallTrigger` and the existing Rigidbody physics.
- Persistent announcements and winner particles reconstruct for late joiners.
- One-shot audio is played only for newly received events.
- `SportsMatchButton` uses an Inspector-selected integer action and keeps no match state.
- `SportsScoreboardView` will render all texts and visuals from manager state.

## Completed microsteps

- `SportsMatchManager.cs` imported and compiled with no red Console errors.
- Empty `SportsMatchManager` GameObject created under `GlobalManagers`.
- Transform and Inspector defaults verified.
- Local Play Mode initialization test completed with no red Console errors.
- Manager-side Join Red, Join Blue and Leave Game logic added and compiled.
- `SportsMatchButton.cs` added and corrected for the installed UdonSharp version.
- Temporary Inspector debug fields added.
- Local ClientSim test passed:
  - Join Red: local team `1`, Red `1`, Blue `0`;
  - Join Blue: local team `2`, Red `0`, Blue `1`;
  - Leave Game: `0, 0, 0`.

## Current microstep

Create only the first minimal `SportsScoreboardView` that:

1. references `SportsMatchManager`;
2. references one Red-team text and one Blue-team text;
3. converts synchronized player IDs into current display names;
4. refreshes after startup and manager state changes;
5. contains no score, timer, winner, lock or announcement rendering yet.

## Pass condition

- Unity and UdonSharp compile the view without red errors.
- In ClientSim, joining Red shows the local display name only in the Red text.
- Switching to Blue removes the name from Red and shows it only in Blue.
- Leaving clears both texts.

## Not implemented yet

- no two-client synchronization test;
- no score or timer behaviour;
- no goals or goal triggers;
- no reset confirmations;
- no audio or particles;
- no SoccerBox football changes.