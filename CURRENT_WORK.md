# Current Work

## Active feature

`FEATURES/SCOREBOARD`

Feature entry point: `FEATURES/SCOREBOARD/00_START_HERE.md`

## Current phase

Implementation — microstep 8 minimal synchronized score rendering.

The first-release interaction design and technical architecture are approved.

Implementation still means: explain one small step, give exactly one action, stop, and wait for Stef's observed result before continuing.

## Current milestone

M4 — Begin the smallest synchronized score display without adding match flow yet.

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
- Three-client late-joiner reconstruction passed:
  - client C joined after A and B were already registered;
  - C immediately reconstructed both existing team lists without a repeated Join action;
  - after an existing player switched team, A, B and C all displayed the same updated lists.
- Registered-player departure cleanup passed:
  - the registered manager owner left the instance;
  - the departed player was removed from the remaining clients;
  - the other registration remained intact;
  - ownership/master transfer did not freeze the manager;
  - remaining clients could still Join, Switch and Leave afterward.

## Current microstep

Extend only `SportsScoreboardView` so it can display the manager's existing synchronized Red and Blue score values.

This step may:

1. add one Red score TMP reference;
2. add one Blue score TMP reference;
3. render `GetRedScore()` and `GetBlueScore()` during the existing event-driven refresh.

This step must not yet:

- add plus or minus actions;
- add score permissions;
- add goals or goal triggers;
- add timer or match-start behaviour;
- alter the SoccerBox football.

## Pass condition

- Unity and UdonSharp compile without red errors.
- Both existing score texts show `0` when the view becomes active.
- Team-name rendering continues to work.
- No `Update()` loop is introduced.

## Not implemented yet

- no score-changing button behaviour;
- no timer behaviour;
- no goals or goal triggers;
- no reset confirmations;
- no audio or particles;
- no SoccerBox football changes.

## New-chat collaboration gate

Before acting, read `COLLABORATION_RULES.md`.

For this implementation phase:

1. explain the purpose of the next action in plain Dutch;
2. give exactly one small action;
3. wait for Stef's compile, screenshot, Console, ClientSim or VRChat result;
4. do not continue automatically;
5. do not mark the microstep complete until the full pass condition has been observed.
