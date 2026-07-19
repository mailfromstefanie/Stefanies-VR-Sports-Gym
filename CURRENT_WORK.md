# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Inventory and interaction design

Do not start Unity implementation yet.

## Current milestone

M1 — Understand the existing sport-mode system, scoreboard UI and protected basketball reference implementation before choosing the generic scoreboard architecture.

## Session goal

Use confirmed evidence from the current Sports Gym setup and the basketball test package to define the smallest useful public-release match experience.

## Confirmed current Sports Gym setup

- Repository: `mailfromstefanie/Stefanies-VR-Sports-Gym`.
- Platform: VRChat Worlds.
- Unity: 2022.3 LTS.
- Scripting: UdonSharp.
- Stef reports that the sports hall and sport games are nearly complete.
- Existing sport modes are Basketball, Soccer, Volleyball and Soccer Hockey.
- Current scripts are stored under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Current hierarchy screenshots are stored under `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The current score-board UI already exists in the Sports Gym but is not yet connected to a final generic match system.

## Confirmed sport-mode architecture

### `SportsModeManager.cs`

- Uses manual Udon synchronization.
- Holds the synchronized active sport-mode index.
- Supports four sport modes.
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

## Confirmed basketball reference behaviour

The package contains a full basketball test scene and the main scripts:

- `BasketBallGrouping.cs`
- `BasketBallGoal.cs`
- `BasketBallButtons.cs`
- `BasketBallBall.cs`
- `BasketBallPickup.cs`
- `BasketBallBlock.cs`

### Existing match features

`BasketBallGrouping` already implements:

- a default ten-minute match;
- configurable match duration;
- Join and Leave registration;
- automatic random Red and Blue team assignment on match start;
- synchronized player IDs and team assignments;
- individual player score bookkeeping;
- calculated team totals;
- TextMeshPro player names and scores;
- a synchronized start timestamp based on VRChat network time;
- a locally reconstructed countdown;
- finish audio when time expires;
- late-join serialization support;
- overhead team tags.

### Existing scoring features

`BasketBallGoal` already implements:

- validated hoop entry;
- ownership checks;
- one-, two- and three-point basketball scoring;
- score assignment to the shooter's team;
- goal particles;
- goal audio;
- synchronized goal effects.

### Missing from the basketball reference

The existing package does not yet:

- determine a winner when time expires;
- display a winning player or team at match end;
- play winner particles at match end;
- provide a generic score interface for Soccer, Volleyball or Soccer Hockey.

## Stef's desired simple experience

Provisional intent, not yet final architecture:

- a match lasts about ten minutes;
- score is kept during play;
- the winner is announced at the end;
- a particle effect plays;
- a cheering sound plays;
- the system should remain as simple as reasonably possible;
- the original basketball prefab must not be damaged.

Player-name presentation is still an open design choice because some sports may be played by teams rather than one individual winner.

## Current architectural observation

The basketball package provides useful proven patterns:

- one synchronized manager as truth;
- network-time-based countdown instead of synchronizing every second;
- buttons as input only;
- UI as display only;
- sport or goal logic reporting points to a manager;
- serialization only when state changes.

However, `BasketBallGrouping` is strongly basketball-specific and combines player registration, team assignment, score bookkeeping, timer, UI and overhead tags.

A small independent generic match manager may be safer than forcing the basketball manager to control every sport. This is only a proposed direction and is not yet accepted.

## Current design question

Decide this first, before discussing implementation:

**For the first public release, does the score system need registered players and individual names, or is a simpler Left Team versus Right Team match sufficient?**

This choice determines whether player registration and team assignment are necessary at all.

Discuss only this question next.

## Remaining open questions

After the current question is resolved, discuss one at a time:

1. How players join or choose a team, if registration is needed.
2. Whether all sports use a fixed ten-minute duration.
3. Which sports report scores automatically and which use manual controls.
4. What happens on a draw.
5. Whether changing sport mode cancels and resets the active match.
6. Who may start, reset or edit a match.
7. How late joiners see the active match and result.

## Risks

- Damaging or tightly coupling to the protected basketball prefab.
- Creating a second source of truth for the active sport mode.
- Copying basketball-specific complexity into every sport.
- Mixing UI state with match state.
- Networking that fails for late joiners or after ownership changes.
- Adding player registration when a simpler team scoreboard would be enough.
- Expanding scope and delaying a nearly finished world.

## Exact next step for Stef

Answer only the current design question:

For the first public release, choose between:

- **A — Simple team match:** Left Team versus Right Team, without registering individual players; or
- **B — Registered players:** players Join, receive or choose teams, and their names can be shown in the result.

No Unity changes are needed for this step.

## Do not do yet

- Do not create a generic MatchManager or ScoreManager.
- Do not edit the basketball scripts or prefabs.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.
- Do not decide automatic score adapters yet.
