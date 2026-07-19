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

## Confirmed project setup

- Repository: `mailfromstefanie/Stefanies-VR-Sports-Gym`.
- Platform: VRChat Worlds.
- Unity: 2022.3 LTS.
- Scripting: UdonSharp.
- The sports hall is nearly complete.
- The release scoreboard scope is Soccer and Soccer Hockey.
- Both use the same football and the same two goals.
- The shared ball and goals remain active while switching between Soccer and Soccer Hockey.
- Switching sport mode does not reset the match.
- Players deliberately reset the match with a separate Reset Game action.
- Existing scripts are under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Existing screenshots are under `CURRENT_UNITY_STATE/SCREENSHOTS/`.

## Existing source-of-truth systems

### `SportsModeManager.cs`

- Manually synchronized active sport mode.
- Activates sport-specific and shared objects.
- Changes the floor material.
- Refreshes sport-mode button visuals.
- Remains the truth for Soccer versus Soccer Hockey.

### `SportsModeButton.cs`

- Sends a mode request to `SportsModeManager`.
- Reads manager state for its visual.
- Does not store an independent mode.

Do not replace these scripts without a separate accepted decision.

## Protected basketball reference

- Package: `REFERENCE_PACKAGES/Basketball/Basketball_TestScene_Inspection.unitypackage`
- Analysis: `REFERENCE_PACKAGES/Basketball/ANALYSIS.md`
- Inspector notes: `REFERENCE_PACKAGES/Basketball/INSPECTOR_REFERENCE.md`
- Treat the original basketball scene, prefabs and scripts as read-only reference material.

Reusable proven patterns include:

- registered players and synchronized teams;
- player names in UI;
- network-time countdown;
- late-join reconstruction;
- manager as truth;
- buttons as input;
- visuals as display;
- goal detectors reporting points to a manager;
- one configurable button script with an Inspector action dropdown.

Do not copy the original random team assignment. The Sports Gym uses explicit `Join Red` and `Join Blue` actions.

## Accepted first-release match experience

### Match scope

- Soccer and Soccer Hockey share one Red-versus-Blue match.
- One football and two goals are used in both modes.
- A ten-minute match starts through `Start Game`.
- Ball in Red goal gives Blue one point.
- Ball in Blue goal gives Red one point.
- Goals count only while the match is active.
- Accepted score changes are synchronized.
- Late joiners and rejoining players reconstruct the current score, timer, teams, lock state and match status.
- At time expiry with unequal scores, the leading team wins immediately.
- At time expiry with equal scores, the timer stays at `00:00` and the match enters sudden death.
- In sudden death, the next valid goal immediately determines the winner.
- Winner presentation uses text, particles and cheering audio.

### Registration, permissions and teams

- Players choose their own team with `Join Red` or `Join Blue`.
- A player belongs to at most one team.
- Player names are available for team lists and the result.
- Before match start, players may choose or change teams.
- `Start Game` requires at least one registered player in Red and at least one in Blue.
- If either team is empty, start is refused and `Both teams need a player` is shown.
- A successful start locks both teams.
- While locked, new players cannot join and registered players cannot switch.
- `Allow Team Switching` temporarily opens joining and switching during an active match.
- Turning it off locks teams again.
- The open/locked state is synchronized and visible to everyone.
- Only a player already registered in Red or Blue may toggle `Allow Team Switching`.
- When players are registered, only a registered Red or Blue player may use `Start Game` or `Reset Game`.
- When no players are registered at all, any visitor may reset or prepare the abandoned board for a new match.
- `Start Game` still cannot succeed until both teams contain a player.
- Reset needs a deliberate confirmation or hold interaction to prevent accidents.

### Existing scoreboard UI

Stef has already created the main layout:

- Red Team player list;
- Blue Team player list;
- shared game timer;
- Red and Blue scores;
- time increase/decrease and No Limit controls;
- Start Game;
- Join Red;
- Join Blue;
- Leave Game;
- Reset Game;
- manual score correction controls;
- one large extra announcement text field above the scoreboard.

The UI should display manager state and should not store independent game state.

### Announcement panel and end-of-time flow

The large upper text field becomes the central shared announcement panel.

It may display:

- short countdown or phase messages;
- start refusal messages;
- team-lock or team-open messages;
- sudden-death status;
- winner text.

Accepted end-of-time behaviour:

1. At `00:00`, play a buzzer sound.
2. If one team leads, finish the match and show the winner.
3. If the score is tied, keep the timer at `00:00`.
4. Set the shared match state to sudden death.
5. Show `NEXT GOAL WINS` in the announcement panel.
6. The message may blink or pulse in a restrained way.
7. Keep the message visible until the next valid goal ends the match.
8. Late joiners during sudden death immediately see the same message and state.

## Likely smallest architecture

Proposed, not yet approved for implementation:

- `SportsModeManager` remains the sport-mode truth.
- One generic match manager becomes truth for registered players, teams, team lock, timer, score, announcement state, sudden-death state and result.
- Two goal detectors report only which goal the football entered.
- One configurable action-button behaviour requests actions such as Join Red, Join Blue, Leave, Start, Reset and Allow Team Switching.
- Scoreboard visuals and the announcement panel only render manager state.

## Current design question

Decide this next:

**What should happen to the football immediately after a valid goal?**

Small options:

- automatically teleport the ball to the centre spot and briefly block further scoring;
- leave the ball where it is and only use a short goal-detector cooldown;
- require players to press a separate Respawn Ball button.

Recommended first-release rule:

- automatically return the football to the centre after every accepted goal;
- ignore further goal triggers until that reset is complete;
- then resume play.

Discuss only this question next.

## Remaining open questions

After the current question is resolved, discuss one at a time:

1. Exact goal anti-double-score implementation.
2. Exact winner text and player-name presentation.
3. Whether team membership persists after a completed match.
4. Exact Reset Game confirmation interaction.
5. How the UI visually shows locked and open teams.
6. Whether No Limit remains in the first release.
7. Exact announcement-panel messages and timing.

## Risks

- Damaging or coupling tightly to the protected basketball prefab.
- Creating a second source of truth.
- Goal detectors keeping independent scores.
- Double-scoring while the ball remains in a goal trigger.
- Outsiders disrupting a running match.
- Networking failures for late join, rejoin or ownership transfer.
- Accidental resets.
- The announcement panel showing a local message that disagrees with shared match state.
- Expanding the reusable button idea into an oversized framework before release.

## Exact next step for Stef

Answer only:

Should a valid goal automatically return the football to the centre spot?

No Unity changes are needed yet.

## Do not do yet

- Do not create the generic match manager.
- Do not create goal triggers.
- Do not edit basketball scripts or prefabs.
- Do not create the universal button prefab.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.