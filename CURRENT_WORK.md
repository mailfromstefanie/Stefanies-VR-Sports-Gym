# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Implementation — microstep 2 manager component setup.

The first-release interaction design and technical architecture are approved.

## Current milestone

M3 — Introduce and verify the smallest working `SportsMatchManager` foundation without connecting gameplay yet.

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

## Completed microstep

`SportsMatchManager.cs` was imported into Unity and compiled successfully with no red Console errors.

## Current microstep

1. Create one empty GameObject named `SportsMatchManager`.
2. Keep its Transform at position `0,0,0`, rotation `0,0,0`, scale `1,1,1`.
3. Add the `SportsMatchManager` UdonSharp component.
4. Do not connect buttons, texts, football, goals or other objects.
5. Inspect the component and capture a screenshot before entering Play Mode.

## Pass condition

- The component can be added without a missing-script warning.
- The Inspector shows the default match duration as `600` seconds.
- No new red Console errors appear.

## Not implemented yet

- no Join Red, Join Blue or Leave Game actions;
- no buttons or scoreboard view;
- no score or timer behaviour;
- no goals or goal triggers;
- no reset confirmations;
- no audio or particles;
- no SoccerBox football changes.