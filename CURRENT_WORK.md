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

### Rematch and team persistence

- Players remain registered in Red or Blue after the match result.
- A normal rematch reset clears score, timer, sudden death, announcement and winner state.
- A normal rematch reset does not clear the Red and Blue player lists.
- Players leave through `Leave Game` or switch teams only while team switching is allowed.
- A separate full cleanup may clear all registrations when the group deliberately wants new teams or an abandoned board must be restored.

### Reset confirmation

- The first valid press of `Reset Game` does not reset immediately.
- It shows `PRESS RESET AGAIN TO CONFIRM` in the shared announcement panel.
- The same player must press `Reset Game` again within about five seconds.
- The second press performs the reset.
- If five seconds pass, the confirmation expires and nothing changes.
- When nobody is registered, any visitor may use the same two-press confirmation to restore an abandoned board.
- A different player cannot complete another player's pending confirmation.

### Beginner-friendly team status UI

The scoreboard must be understandable to someone using VR for the first time.

- Do not rely on colour alone.
- The team-switching control changes its own text automatically from shared manager state.
- Locked state: `TEAM SWITCHING: LOCKED` with helper text such as `Press to allow players to join or switch`.
- Open state: `TEAM SWITCHING: OPEN` with helper text such as `Press to lock teams`.
- If someone tries to join or switch while locked, show `TEAMS ARE LOCKED` and `ASK A PLAYER TO OPEN TEAM SWITCHING` briefly in the announcement panel.
- Colour may support the state, but the words must fully explain it.
- Late joiners, resets and ownership changes rebuild the correct text automatically.
- The central match manager remains the source of truth; buttons and labels only display that state.

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

**Should `No Limit` remain in the first release?**

Recommended rule:

- keep `No Limit`;
- when enabled, the timer shows `NO LIMIT` instead of counting down;
- the match ends only when a registered player deliberately resets or changes the time mode;
- sudden death does not apply because there is no time expiry.

Discuss only this question next.

## Remaining open questions

1. Exact announcement timing for start and lock messages.
2. Whether manual score correction is allowed during sudden death.
3. Exact goal anti-double-score and ball-ownership implementation.
4. Whether full cleanup needs its own separate button or a longer Reset confirmation.

## Do not do yet

- Do not create the generic match manager.
- Do not create goal triggers.
- Do not alter the ball.
- Do not edit basketball scripts or prefabs.
- Do not create the universal button prefab.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.