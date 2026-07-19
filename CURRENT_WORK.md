# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Inventory and interaction design

Do not start Unity implementation yet.

## Current milestone

M1 — Define the smallest reliable match system for Soccer and Soccer Hockey using the existing sport-mode system and protected basketball reference patterns.

## Session goal

Finish the first-release interaction design before choosing the generic match architecture or writing Unity code.

## Confirmed current Sports Gym setup

- Repository: `mailfromstefanie/Stefanies-VR-Sports-Gym`.
- Platform: VRChat Worlds.
- Unity: 2022.3 LTS.
- Scripting: UdonSharp.
- The sports hall is nearly complete.
- The current public-release scoreboard scope is now limited to two playable modes:
  - Soccer;
  - Soccer Hockey.
- Both modes use the same football and the same two goals.
- Switching to Soccer Hockey changes the field setup and enables hockey sticks; players strike the football with the sticks.
- Current scripts are stored under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Current hierarchy screenshots are stored under `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The current scoreboard UI already exists but is not yet connected to a final match system.

## Confirmed sport-mode architecture

### `SportsModeManager.cs`

- Uses manual Udon synchronization.
- Holds the synchronized active sport-mode index.
- Supports four sport modes, although the current scoreboard release scope is Soccer and Soccer Hockey.
- Takes ownership before a player changes the active mode.
- Activates and deactivates sport-specific and shared object groups.
- Changes the sports-floor material.
- Refreshes sport-button visuals.
- Is the existing shared source of truth for the selected sport mode.

### `SportsModeButton.cs`

- Sends a selected mode index to `SportsModeManager`.
- Reads the current mode from the manager to refresh its sprite.
- Does not store an independent copy of the active sport mode.

This existing system follows:

- Manager = truth
- Button = input
- Visual = display

Do not replace it without a separate accepted decision.

## Protected basketball reference package

Stored at:

`REFERENCE_PACKAGES/Basketball/Basketball_TestScene_Inspection.unitypackage`

Detailed read-only findings are stored at:

`REFERENCE_PACKAGES/Basketball/ANALYSIS.md`

The original basketball test scene, prefabs and scripts are protected reference material. Stef copied only UI elements into the Sports Gym. Do not modify the original basketball package.

## Confirmed reusable basketball patterns

The basketball reference proves these patterns are feasible:

- registered players;
- synchronized player IDs and team assignments;
- player names in UI;
- one synchronized manager as truth;
- network-time-based countdown instead of synchronizing every second;
- late-join serialization support;
- buttons as input only;
- UI as display only;
- serialization when shared state changes;
- goal logic reporting points to a manager;
- one configurable button script using an Inspector enum.

The basketball implementation itself remains basketball-specific and protected.

## Accepted first-release match experience

### Play modes

Only Soccer and Soccer Hockey need the new match system for the current release.

Both use:

- one shared football;
- two goals;
- the same Red-versus-Blue match structure;
- the same score manager and goal detection concept.

### Team registration

Stef selected registered players with manual team choice.

The design must support:

- `Join Red`;
- `Join Blue`;
- leaving the match;
- a player belonging to at most one team at a time;
- registered player names being available for result presentation.

### Match flow

Provisional accepted direction:

1. Players choose Red or Blue.
2. A Start Game action begins one shared ten-minute match.
3. The countdown is reconstructed from one synchronized network start time.
4. Goals count only while the match is active.
5. When the ball enters Red's goal, Blue receives one point.
6. When the ball enters Blue's goal, Red receives one point.
7. The changed score is serialized when a goal is accepted.
8. Late joiners and rejoining players receive the current shared match state through synchronized variables and deserialization.
9. At time expiry, the winner is determined and presented with winner text, particles and cheering audio.

### Goal detection direction

A trigger or detection volume inside each goal is a suitable small design direction.

Each goal detector should only report a valid goal event to the central match manager. It should not keep its own independent score.

The exact anti-double-score rule, ball reset behaviour and trigger implementation are not yet final.

## Reusable configurable button pattern

`BasketBallButtons` demonstrates a useful Inspector-driven input pattern:

- each UI button can use the same button behaviour;
- Unity UI `On Click` invokes `UdonBehaviour.Interact()`;
- an Inspector enum/dropdown selects the intended action;
- references determine which manager receives the action.

This pattern is accepted as a design reference for the Sports Gym. A universal Art House or general-purpose prefab remains later scope.

## Current architectural observation

The likely smallest safe architecture is:

- `SportsModeManager` remains the truth for Soccer versus Soccer Hockey;
- one future generic match manager becomes the truth for registration, teams, timer, score and result;
- two goal detectors only report which goal was entered;
- configurable action buttons request Join Red, Join Blue, Leave and Start Game;
- scoreboard visuals only display manager state.

This is still a proposed architecture, not permission to build it yet.

## Current design question

Decide this next before implementation:

**May players change teams after joining, and if so, until what moment?**

Recommended simple rule:

- before the match: a player may switch by pressing the other team button;
- once the match has started: team changes are locked until the match ends or is reset.

Discuss only this question next.

## Remaining open questions

After the current question is resolved, discuss one at a time:

1. Whether a match may start with one team empty.
2. What happens on a draw.
3. Whether changing between Soccer and Soccer Hockey cancels and resets an active match.
4. Who may start or reset a match.
5. Exact goal anti-double-score behaviour.
6. Ball reset behaviour after a goal.
7. Exact winner text and how registered player names are shown.
8. Whether team membership persists after a completed match.

## Risks

- Damaging or tightly coupling to the protected basketball prefab.
- Creating a second source of truth for the active sport mode.
- Letting goal detectors keep independent scores.
- Double-scoring while the ball remains in or bounces through a goal trigger.
- Team switching during play making the winner list unreliable.
- Networking that fails for late joiners, rejoining players or after ownership changes.
- Turning the reusable button idea into an oversized framework before release.
- Expanding scope beyond the two games that currently need completion.

## Exact next step for Stef

Answer only this design question:

- may players switch between Red and Blue before the match starts;
- and should team switching lock as soon as Start Game is pressed?

No Unity changes are needed for this step.

## Do not do yet

- Do not create the generic match manager yet.
- Do not create goal trigger objects yet.
- Do not edit the basketball scripts or prefabs.
- Do not create the universal button prefab yet.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.
