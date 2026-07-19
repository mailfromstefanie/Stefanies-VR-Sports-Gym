# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Testing — microstep 6 late-joiner team-list reconstruction.

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
- Two-client synchronization passed:
  - client A joined Red and client B joined Blue;
  - both clients displayed identical Red and Blue lists;
  - client A switched from Red to Blue without duplicating or removing client B;
  - client A left the game and disappeared on both clients while client B remained registered;
  - ownership transfer did not produce duplicate registrations.

## Current microstep

Run only the first late-joiner reconstruction test for team registration and player-list rendering.

Test that:

1. client A joins Red;
2. client B joins Blue;
3. a third client joins after both registrations already exist;
4. client C sees the same Red and Blue names without anyone pressing a button again;
5. all three clients continue to show the same lists after one existing player switches team.

Do not add score, timer, goals or reset behaviour during this test.

## Pass condition

- The late joiner reconstructs the current Red and Blue registrations from synchronized manager state.
- No existing player must repeat a Join action to make the names appear.
- All three clients display identical lists after a subsequent team switch.
- No red Console or Udon errors occur.

## Not implemented yet

- no completed late-joiner team-list test;
- no score or timer behaviour;
- no goals or goal triggers;
- no reset confirmations;
- no audio or particles;
- no SoccerBox football changes.
