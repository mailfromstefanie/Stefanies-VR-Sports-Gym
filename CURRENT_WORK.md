# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Implementation — microstep 1 compile test.

The first-release interaction design and technical architecture are approved.

## Current milestone

M3 — Introduce the smallest compiling `SportsMatchManager` foundation without connecting gameplay yet.

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

## Built in the current microstep

Created `CURRENT_UNITY_STATE/SCRIPTS/SportsMatchManager.cs` with:

- the approved phase, team and empty-slot constants;
- the approved synchronized snapshot fields;
- Red and Blue player-ID arrays;
- owner-only initial state setup;
- one initial `RequestSerialization()` call;
- read-only getter methods for later helpers.

## Not implemented yet

- no Join Red, Join Blue or Leave Game actions;
- no buttons or scoreboard view;
- no score or timer behaviour;
- no goals or goal triggers;
- no reset confirmations;
- no audio or particles;
- no SoccerBox football changes;
- no Unity hierarchy changes.

## Current test

1. Bring the new `SportsMatchManager.cs` file into the Unity project.
2. Wait until Unity and UdonSharp finish compiling.
3. Do not add it to a GameObject yet.
4. Check the Console for red errors.

## Pass condition

Unity finishes compiling with no red error caused by `SportsMatchManager.cs`.

## Failure evidence

If an error appears, capture the complete first red Console error, including file name, line number and message.

## Next step after a pass

Create one empty manager GameObject, add `SportsMatchManager`, inspect its fields, and perform a local initialization test before connecting any button.