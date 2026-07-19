# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Inventory and interaction design

Do not start Unity implementation yet.

## Current milestone

M1 — Define the smallest reliable shared match system for Soccer and Soccer Hockey.

## Session goal

Finish the first-release interaction design before choosing the generic match architecture or writing Unity code.

## Confirmed current Sports Gym setup

- Repository: `mailfromstefanie/Stefanies-VR-Sports-Gym`.
- Platform: VRChat Worlds.
- Unity: 2022.3 LTS.
- Scripting: UdonSharp.
- The sports hall is nearly complete.
- The current public-release scoreboard scope is limited to Soccer and Soccer Hockey.
- Both modes use the same football and the same two goals.
- The shared football and both goals are assigned under `Soccer Hockey Shared Objects` in `SportsModeManager`.
- Switching to Soccer Hockey changes the field setup and enables hockey sticks, but does not replace the football or goals.
- Current scripts are stored under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Current hierarchy screenshots are stored under `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The scoreboard UI already exists but is not yet connected to a final match system.

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
- two shared goals;
- the same Red-versus-Blue match structure;
- the same score manager and goal detection concept.

The match system is independent from the visual sport-mode switch. Changing between Soccer and Soccer Hockey does not automatically cancel or reset a match because the ball, goals and scoring rules remain the same.

Players reset the match deliberately through a separate Reset Game action.

### Team registration

Stef selected registered players with manual team choice.

The design must support:

- `Join Red`;
- `Join Blue`;
- leaving the match;
- a player belonging to at most one team at a time;
- registered player names being available for result presentation.

### Locked-team rule

Accepted behaviour:

- before the match starts, players may choose Red or Blue;
- when `Start Game` is pressed, both teams lock;
- while teams are locked, new players may not join an active match;
- while teams are locked, existing players may not switch teams;
- a shared `Allow Team Switching` control can temporarily unlock team joining and switching during the active match;
- only while that control is enabled may a new player join Red or Blue or an existing player change teams;
- switching the control off locks both teams again;
- the lock state is shared and must be visible to all players, including late joiners.

The exact permission rule for who may operate `Allow Team Switching` is still open.

### Match flow

Accepted direction:

1. Players choose Red or Blue.
2. A Start Game action begins one shared ten-minute match and locks both teams.
3. The countdown is reconstructed from one synchronized network start time.
4. Goals count only while the match is active.
5. When the ball enters Red's goal, Blue receives one point.
6. When the ball enters Blue's goal, Red receives one point.
7. The changed score is serialized when a goal is accepted.
8. Late joiners and rejoining players receive the current shared match state through synchronized variables and deserialization.
9. A late joiner observes the current match but cannot join a locked team unless `Allow Team Switching` is enabled.
10. Switching between Soccer and Soccer Hockey leaves the active match intact.
11. A separate Reset Game action clears the match state when players choose to reset it.
12. At time expiry, the winner is determined and presented with winner text, particles and cheering audio.

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
- one future generic match manager becomes the independent truth for registration, teams, team-lock state, timer, score and result;
- the match manager does not reset merely because the sport mode changes;
- two goal detectors only report which goal was entered;
- configurable action buttons request Join Red, Join Blue, Leave, Start Game, Reset Game and Allow Team Switching;
- scoreboard visuals only display manager state.

This is still a proposed architecture, not permission to build it yet.

## Current design question

Decide this next before implementation:

**Who is allowed to toggle `Allow Team Switching` during an active match?**

Small options to compare:

- any registered player in either team;
- only the player who started the match;
- only instance master or a configured admin.

Discuss only this question next.

## Remaining open questions

After the current question is resolved, discuss one at a time:

1. Whether a match may start with one team empty.
2. What happens on a draw.
3. Who may start or reset a match.
4. Exact goal anti-double-score behaviour.
5. Ball reset behaviour after a goal.
6. Exact winner text and how registered player names are shown.
7. Whether team membership persists after a completed match.
8. Whether Reset Game needs confirmation or restricted access.
9. How the UI shows that teams are locked or temporarily open.

## Risks

- Damaging or tightly coupling to the protected basketball prefab.
- Creating a second source of truth for the active sport mode.
- Letting goal detectors keep independent scores.
- Double-scoring while the ball remains in or bounces through a goal trigger.
- New players entering an active match without the current players deliberately opening the teams.
- Unauthorized players repeatedly unlocking teams or disrupting the match.
- Networking that fails for late joiners, rejoining players or after ownership changes.
- Accidental resets by any player during an active match.
- Turning the reusable button idea into an oversized framework before release.
- Expanding scope beyond the two games that currently need completion.

## Exact next step for Stef

Answer only this design question:

Who may toggle `Allow Team Switching` during an active match?

No Unity changes are needed for this step.

## Do not do yet

- Do not create the generic match manager yet.
- Do not create goal trigger objects yet.
- Do not edit the basketball scripts or prefabs.
- Do not create the universal button prefab yet.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.