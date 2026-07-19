# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Implementation — microstep 3 team registration methods.

The first-release interaction design and technical architecture are approved.

## Current milestone

M3 — Introduce and verify the smallest working `SportsMatchManager` foundation before connecting UI.

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
- `SportsMatchButton` will use an Inspector-selected action and keep no match state.
- `SportsScoreboardView` will render all texts and visuals from manager state.

## Completed microsteps

- `SportsMatchManager.cs` imported and compiled with no red Console errors.
- Empty `SportsMatchManager` GameObject created under `GlobalManagers`.
- Transform and Inspector defaults verified.
- Local Play Mode initialization test completed with no red Console errors.

## Current microstep

Add only manager-side methods for:

1. Join Red;
2. Join Blue;
3. Leave Game;
4. player lookup and empty-slot handling;
5. accepted-action ownership and one serialization request.

Do not create buttons or scoreboard visuals yet. Compile this manager-only change first.

## Pass condition

- Unity and UdonSharp compile the updated manager without red errors.
- Existing scene behaviour remains unchanged because no UI is connected.
- No football, goal, timer, score or sport-mode object is modified.

## Not implemented yet

- no buttons or scoreboard view;
- no score or timer behaviour;
- no goals or goal triggers;
- no reset confirmations;
- no audio or particles;
- no SoccerBox football changes.
