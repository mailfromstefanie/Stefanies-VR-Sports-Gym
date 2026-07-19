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

### Reusable configurable button pattern

`BasketBallButtons` demonstrates a useful Inspector-driven input pattern:

- each UI button has the same button behaviour;
- Unity UI `On Click` invokes `UdonBehaviour.Interact()`;
- an Inspector enum/dropdown selects the intended action, such as Join Game, Leave Game, Start Game or changing the match duration;
- the button behaviour routes that selected action to the referenced manager or ball settings;
- a new button can therefore be configured mainly through Inspector references instead of wiring a different public method for every button.

This pattern is a strong candidate for a future reusable Sports Gym button prefab and, after separate design and testing, for Stefanie's Art House Cinema. It is not yet approved as a finished universal framework.

## Missing from the basketball reference

The existing package does not yet:

- determine a winner when time expires;
- display a winning player or team at match end;
- play winner particles at match end;
- provide a generic score interface for Soccer, Volleyball or Soccer Hockey.

## Accepted first-release experience direction

Stef selected **B — Registered players**.

The first public-release design must therefore support:

- players joining and leaving the match;
- players being associated with a team;
- player names being available for score and result presentation;
- a match lasting about ten minutes;
- score being kept during play;
- winner presentation at the end;
- a particle effect and cheering sound;
- protection of the original basketball prefab.

The exact winner text is not decided yet. For a team game it may need to show the winning team and one or more registered player names rather than claiming one individual always won.

## Current architectural observation

The basketball package provides useful proven patterns:

- one synchronized manager as truth;
- network-time-based countdown instead of synchronizing every second;
- buttons as input only;
- UI as display only;
- sport or goal logic reporting points to a manager;
- serialization only when state changes;
- one configurable button script using an Inspector action enum.

However, `BasketBallGrouping` is strongly basketball-specific and combines player registration, team assignment, score bookkeeping, timer, UI and overhead tags.

A small independent generic match manager may be safer than forcing the basketball manager to control every sport. This is only a proposed direction and is not yet accepted.

## Current design question

Decide this next, before discussing implementation:

**How should registered players enter teams for the first public release?**

- **A — Automatic balanced assignment:** players press Join; the system divides them between Red and Blue when the match starts.
- **B — Manual team choice:** players explicitly press Join Red or Join Blue.

The basketball reference currently uses automatic assignment. Manual choice gives players control but needs extra rules for uneven or empty teams.

Discuss only this question next.

## Remaining open questions

After the current question is resolved, discuss one at a time:

1. Whether all sports use a fixed ten-minute duration.
2. Which sports report scores automatically and which use manual controls.
3. What happens on a draw.
4. Whether changing sport mode cancels and resets the active match.
5. Who may start, reset or edit a match.
6. How late joiners see the active match and result.
7. How the winning team and registered player names are presented.
8. Whether the configurable action-button pattern becomes a separate generic prefab now or later.

## Risks

- Damaging or tightly coupling to the protected basketball prefab.
- Creating a second source of truth for the active sport mode.
- Copying basketball-specific complexity into every sport.
- Mixing UI state with match state.
- Networking that fails for late joiners or after ownership changes.
- Manual team choice allowing heavily uneven teams without clear rules.
- Turning the generic button idea into an oversized framework before the Sports Gym release.
- Expanding scope and delaying a nearly finished world.

## Exact next step for Stef

Answer only the current design question:

For registered players, choose between:

- **A — Automatic balanced assignment** when the match starts; or
- **B — Manual Join Red / Join Blue buttons**.

No Unity changes are needed for this step.

## Do not do yet

- Do not create a generic MatchManager or ScoreManager.
- Do not edit the basketball scripts or prefabs.
- Do not create the universal button prefab yet.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.
- Do not decide automatic score adapters yet.
