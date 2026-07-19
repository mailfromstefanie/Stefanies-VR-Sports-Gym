# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Interaction design complete; architecture review is next.

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
- `No Limit` is not part of the first release.
- Visitors who only want to play casually can leave the official match unstarted and use the manual score controls to keep their own score.
- The timer, end buzzer and sudden death belong only to a formally started timed match.
- Manual score correction remains available before a match and during normal timed play.
- Manual score correction is disabled during sudden death; only a valid physical goal may decide the winner.

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

### Reset confirmation

- The first valid press of `Reset Game` does not reset immediately.
- It shows `PRESS RESET AGAIN TO CONFIRM` in the shared announcement panel.
- The same player must press `Reset Game` again within about five seconds.
- The second press performs the reset.
- If five seconds pass, the confirmation expires and nothing changes.
- When nobody is registered, any visitor may use the same two-press confirmation to restore an abandoned board.
- A different player cannot complete another player's pending confirmation.

### Clear All Players

- `CLEAR ALL PLAYERS` is a separate action from `Reset Game`.
- It is available only while no match is active.
- The first valid press changes that same button to `PRESS AGAIN TO CONFIRM` for about five seconds.
- The same player must press the same button again within that window.
- The second press removes everyone from Red and Blue.
- If the window expires, the button automatically returns to `CLEAR ALL PLAYERS` and nothing changes.
- The announcement panel may simultaneously show `CLEAR ALL PLAYERS?`.
- Normal `Reset Game` never clears the team lists.

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

### Announcement timing

- Short status confirmations such as `GAME STARTED`, `TEAMS LOCKED` and `TEAM SWITCHING OPEN` remain visible for about two seconds.
- They then clear automatically so the announcement panel is ready for goals, warnings and other important match states.
- Persistent information belongs on the relevant button or control itself.
- Important states such as `NEXT GOAL WINS`, reset confirmation and the winner message follow their own longer lifetime rules.

## Existing scoreboard UI

Stef has already created:

- Red and Blue player lists;
- timer and team scores;
- time increase and decrease controls;
- Start Game, Join Red, Join Blue, Leave Game and Reset Game;
- manual score correction controls;
- a large shared announcement panel.

The existing `No Limit` control is outside the accepted first-release scope and should not be connected to the new manager.

A separate `CLEAR ALL PLAYERS` button still needs to be added later during the approved Unity build phase.

UI objects display manager state and do not store independent match state.

## Likely smallest architecture

Proposed, not yet approved for implementation:

- `SportsModeManager` remains sport-mode truth.
- One generic match manager stores players, teams, permissions, timer, scores, sudden death, announcements and result.
- Two goal detectors report only which goal was entered.
- One ball-reset reference returns the football to a centre anchor.
- One configurable action-button behaviour routes Inspector-selected actions.
- Scoreboard visuals only render manager state.

## Next design step

The first-release interaction design is now complete.

Next, review and approve the smallest technical architecture before any Unity hierarchy or script changes are made. That architecture review must cover:

- the exact synchronized fields and match states;
- ownership rules for manager actions and the football;
- goal anti-double-score protection;
- ball reset and Rigidbody handling;
- late-join reconstruction;
- how the configurable action buttons and automatic visuals connect to the manager;
- a microstep build-and-test order.

## Exact next step for Stef

Approve moving from interaction design into architecture design. This is not yet permission to write scripts or change Unity.

## Do not do yet

- Do not create the generic match manager.
- Do not create goal triggers.
- Do not alter the ball.
- Do not edit basketball scripts or prefabs.
- Do not create the universal button prefab.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.