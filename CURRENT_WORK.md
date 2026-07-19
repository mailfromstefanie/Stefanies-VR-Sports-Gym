# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Inventory and interaction design

Do not start Unity implementation yet.

## Current milestone

M1 — Define the smallest reliable shared match system for Soccer and Soccer Hockey.

## Confirmed project setup

- Platform: VRChat Worlds, Unity 2022.3 LTS, UdonSharp.
- Soccer and Soccer Hockey share one football and two goals.
- Switching between those modes does not reset the match.
- Existing scripts are under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Existing screenshots are under `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The protected basketball reference is under `REFERENCE_PACKAGES/Basketball/`.
- `SportsModeManager.cs` remains the synchronized sport-mode truth.

## Accepted match experience

### Match and scoring

- Default duration is ten minutes.
- Ball in Red goal gives Blue one point.
- Ball in Blue goal gives Red one point.
- Goals count only while the match is active.
- Shared state supports late join and rejoin.
- Sport-mode switching leaves the match intact.
- Reset Game is separate and deliberate.

### Players, teams and permissions

- Players choose `Join Red` or `Join Blue`.
- A player belongs to at most one team.
- Start requires at least one player in both teams.
- Start locks both teams.
- `Allow Team Switching` temporarily opens joining and switching.
- Only a registered Red or Blue player may operate that control.
- When players are registered, only registered players may Start or Reset.
- When nobody is registered, any visitor may restore an abandoned board.
- Reset needs a deliberate confirmation or hold interaction.

### End of time

- At `00:00`, play a buzzer.
- If one team leads, that team wins.
- If tied, timer remains at `00:00` and sudden death begins.
- Announcement panel shows `NEXT GOAL WINS`.
- The next valid goal ends the match.

### Goal sequence

After a normal accepted goal:

1. Add and synchronize the point.
2. Block further goal detection.
3. Show `GOAL FOR RED TEAM` or `GOAL FOR BLUE TEAM`.
4. Play a shared goal sound.
5. Stop the ball, clear velocity and angular velocity, and move it to centre.
6. Hold it unavailable for about two seconds.
7. Clear the message and resume play.

A deciding sudden-death goal skips the normal restart.

### Winner presentation

Accepted final announcement:

- `RED TEAM WINS`, or
- `BLUE TEAM WINS`.

Do not add player names to the winner message. Player names remain visible only in the normal team lists.

The winner presentation also uses:

- cheering audio;
- particles for the winning team;
- a message that remains visible until Reset Game.

## Existing scoreboard UI

Stef has already created:

- Red and Blue player lists;
- timer and team scores;
- time controls and No Limit;
- Start Game, Join Red, Join Blue, Leave Game and Reset Game;
- manual score correction controls;
- a large shared announcement panel.

UI objects display manager state and do not store independent match state.

## Likely smallest architecture

Proposed, not yet approved for implementation:

- `SportsModeManager` remains sport-mode truth.
- One generic match manager stores players, teams, permissions, timer, scores, sudden death, announcements and result.
- Two goal detectors report only which goal was entered.
- One ball-reset reference returns the football to a centre anchor.
- One configurable action-button behaviour routes Inspector-selected actions.
- Scoreboard visuals only render manager state.

## Current design question

**Should team membership remain after a match finishes, or should the players be removed from Red and Blue automatically?**

Recommended first-release rule:

- keep the players registered after the result;
- Reset Game clears score, timer and result but leaves the teams in place;
- players can use Leave Game or switch teams before the next start.

Discuss only this question next.

## Remaining open questions

1. Exact Reset Game confirmation interaction.
2. How the UI shows locked and open teams.
3. Whether No Limit remains in the first release.
4. Exact announcement timing for start and lock messages.
5. Whether manual score correction is allowed during sudden death.
6. Exact goal anti-double-score and ball-ownership implementation.

## Do not do yet

- Do not create the generic match manager.
- Do not create goal triggers.
- Do not alter the ball.
- Do not edit basketball scripts or prefabs.
- Do not create the universal button prefab.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.