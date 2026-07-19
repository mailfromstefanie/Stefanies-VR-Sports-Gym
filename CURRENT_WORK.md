# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Testing — microstep 5 two-client team-list synchronization.

The first-release interaction design and technical architecture are approved.

## Current milestone

M3 — Verify synchronized team registration and the smallest scoreboard view.

## Confirmed project setup

- Platform: VRChat Worlds, Unity 2022.3 LTS, UdonSharp.
- Soccer and Soccer Hockey share one football and two goals.
- Switching between those modes does not reset the match.
- Existing scripts are under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Existing screenshots are under `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The protected basketball reference remains read-only.
- `SportsModeManager.cs` remains the synchronized sport-mode truth.
- The current football is a SoccerBox ball with its own `SBBall` synchronization and child `BallTrigger` interaction.
- Manager GameObjects remain active continuously; sport-specific roots and scoreboard visuals may be inactive until their mode is selected.

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
- `SportsScoreboardView` renders texts and visuals from manager state.
- Scoreboard refresh is event-driven: startup, accepted manager changes, player cleanup and deserialization call one refresh. There is no per-frame `Update()` refresh.

## Completed microsteps

- `SportsMatchManager.cs` imported and compiled with no red Console errors.
- Empty `SportsMatchManager` GameObject created under `GlobalManagers`.
- Transform and Inspector defaults verified.
- Local Play Mode initialization test completed with no red Console errors.
- Manager-side Join Red, Join Blue and Leave Game logic added and compiled.
- `SportsMatchButton.cs` added and corrected for the installed UdonSharp version.
- Temporary Inspector debug fields added.
- Local ClientSim manager test passed:
  - Join Red: local team `1`, Red `1`, Blue `0`;
  - Join Blue: local team `2`, Red `0`, Blue `1`;
  - Leave Game: `0, 0, 0`.
- `SportsScoreboardView.cs` added and compiled without red errors.
- The view is connected to `SportsMatchManager` and the Red and Blue `PlayerNames` TMP texts.
- The manager holds a reference to the view and refreshes it only after meaningful state changes and deserialization.
- Local ClientSim player-list rendering passed:
  - Join Red displays the local name only under Red;
  - Join Blue moves the name from Red to Blue;
  - Leave Game clears both lists.
- Inactive-root reconstruction passed:
  - the player joined Red while the Soccer scoreboard root was inactive in Basketball mode;
  - after switching to Soccer, the already registered name appeared immediately.

## Current microstep

Run only the first two-client synchronization test for team registration and player-list rendering.

Test that:

1. client A joins Red;
2. client B sees A under Red;
3. client B joins Blue;
4. both clients see the same Red and Blue lists;
5. one client switches team and both clients update;
6. one client leaves the game and both clients clear that registration.

Do not add score, timer, goals or reset behaviour during this test.

## Pass condition

- Both clients display identical Red and Blue player lists after every accepted Join, Switch and Leave action.
- Ownership transfer does not create duplicate registrations or remove the other player's registration.
- No red Console or Udon errors occur.

## Not implemented yet

- no completed two-client synchronization test;
- no score or timer behaviour;
- no goals or goal triggers;
- no reset confirmations;
- no audio or particles;
- no SoccerBox football changes.
