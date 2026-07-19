# Basketball reference package analysis

## Status

Read-only inspection completed on 2026-07-19 from `Basketball_TestScene_Inspection.unitypackage`.

The original package and prefabs remain protected reference material. Nothing in the package was modified.

## Package contents

Confirmed basketball-specific assets include:

- `Assets/BasketBall/BasketBall_FullCourt.unity`
- `Assets/BasketBall/Prefabs/BasketBallGrouping.prefab`
- `Assets/BasketBall/Prefabs/BasketBallGoal.prefab`
- `Assets/BasketBall/Prefabs/BasketBall_Ball.prefab`
- `Assets/BasketBall/Prefabs/FullCourt.prefab`
- six basketball UdonSharp scripts
- goal, finish and bounce audio
- goal particle effects and basketball visuals

## Main scripts and responsibilities

### `BasketBallGrouping.cs`

This is the basketball match, team, timer and score manager.

Confirmed responsibilities:

- manual Udon synchronization;
- player registration through Join and Leave actions;
- synchronized player IDs;
- synchronized positive and negative score values per player;
- automatic random assignment into Red and Blue teams when the match starts;
- individual player-name and player-score lists;
- total team-score calculation;
- configurable match length;
- default match length of 10 minutes;
- synchronized start time based on VRChat network time;
- countdown display using TextMeshPro;
- finish sound when time reaches zero;
- serialization for late joiners and ownership changes;
- overhead team tags above participating players.

Important limitation:

- it does not determine or display a winner when the timer ends;
- it does not play a winner particle effect at match end;
- it does not show a winning player or team name at match end.

### `BasketBallGoal.cs`

This handles a scored basket.

Confirmed responsibilities:

- validates that the basketball passes through the hoop in the correct direction;
- checks ownership and ball state;
- calculates one-, two- or three-point scores;
- adds or subtracts the score based on the shooter's assigned team and hoop side;
- plays goal particle effects;
- plays the goal sound;
- synchronizes the goal effect trigger.

### `BasketBallButtons.cs`

This is input only.

Confirmed button actions:

- respawn ball;
- join game;
- leave game;
- start game;
- increase match minutes;
- decrease match minutes;
- toggle no-time-limit mode.

### `BasketBallBall.cs`, `BasketBallPickup.cs`, `BasketBallBlock.cs`

These scripts manage the basketball interaction itself, including pickup, dribbling, shooting, blocking, respawning, ball synchronization and player movement adjustments. They are not the scoreboard source of truth and should not be changed for the generic scoreboard feature.

## Existing UI structure

The `BasketBallGrouping.prefab` already contains a substantial UI with:

- Red Team and Blue Team panels;
- team total scores;
- individual player names;
- individual player scores;
- unassigned-player list;
- minutes and seconds countdown;
- Join, Leave and Start controls;
- match-length controls;
- no-limit control;
- overhead team tags.

Stef copied only UI elements from this package into the Sports Gym. The original prefab and its scripts are not to be edited.

## Networking model

The basketball manager uses one manually synchronized UdonBehaviour as the shared source of truth.

Synchronized values include:

- player IDs and team assignment;
- positive scores;
- negative scores;
- match start timestamp;
- configured match duration.

A player takes ownership before changing shared match data and then requests serialization.

The countdown is reconstructed locally from the shared network timestamp rather than synchronizing every timer tick. This is efficient and useful as a reference pattern.

## What can be safely learned or reused

The following ideas are suitable reference patterns for the Sports Gym scoreboard:

- one manager as the shared source of truth;
- buttons as input only;
- TextMeshPro fields as visual output only;
- network-time-based countdown;
- manual synchronization only when state changes;
- late-join reconstruction from synchronized state;
- separate goal or sport adapters that report points to a central match manager;
- finish audio and particle references owned by the match manager.

Do not directly modify or subclass the protected basketball prefab without a separate explicit decision.

## Architectural implication for the Sports Gym

The basketball package proves that the desired ten-minute match concept is feasible and that much of the interaction design already exists.

However, its manager is basketball-specific. It combines:

- player registration;
- automatic team assignment;
- basketball scoring rules;
- individual score bookkeeping;
- UI references;
- overhead tags;
- timer logic.

Using that script unchanged for all sports would create tight coupling and unnecessary complexity.

The likely safe direction is a small independent generic match manager that uses the proven timer and networking patterns, while sport-specific scoring systems report points through small adapters. This remains a proposed direction, not yet an accepted architecture.

## Open design questions

Before architecture is approved, decide one question at a time:

1. Is the first public version team-based only, or must it display individual winners?
2. Do players explicitly Join a match, or does the scoreboard simply track Left and Right teams?
3. Are teams assigned automatically, selected manually, or not registered at all?
4. Does every sport use the same ten-minute duration?
5. Which sports can report scores automatically and which need manual score buttons?
6. What should happen on a draw?
7. Should switching sport mode cancel and reset the active match?

## Protected elements

Do not change yet:

- the original basketball scripts;
- the original basketball prefabs;
- the basketball goal triggers;
- basketball ball physics and ownership logic;
- original UdonBehaviour assets;
- original test scene.
