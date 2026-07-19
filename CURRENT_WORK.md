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
- Soccer and Soccer Hockey share one football and two goals.
- Switching between those modes does not reset the match.
- Existing scripts are under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Existing screenshots are under `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The protected basketball reference is under `REFERENCE_PACKAGES/Basketball/`.

## Existing source-of-truth systems

- `SportsModeManager.cs` remains the synchronized truth for the selected sport mode.
- `SportsModeButton.cs` only sends input and refreshes its visual.
- Do not replace these scripts without a separate accepted decision.

## Accepted match experience

### Match and scoring

- Soccer and Soccer Hockey share one Red-versus-Blue match.
- Default game time is ten minutes.
- Ball in Red goal gives Blue one point.
- Ball in Blue goal gives Red one point.
- Goals count only while the match is active.
- Scores and match state are shared for all players and reconstructed for late joiners.
- Switching sport mode leaves the active match intact.
- Reset Game is a separate deliberate action.

### Players and permissions

- Players choose `Join Red` or `Join Blue`.
- A player belongs to at most one team.
- Start requires at least one registered player in each team.
- Start locks both teams.
- New players cannot join and existing players cannot switch while teams are locked.
- `Allow Team Switching` temporarily opens joining and switching.
- Only a registered Red or Blue player may toggle that control.
- When players are registered, only registered players may Start or Reset.
- When nobody is registered, any visitor may clean up or reset an abandoned board.
- Reset needs a deliberate confirmation or hold interaction.

### Time expiry and sudden death

- At `00:00`, a buzzer sounds.
- If one team leads, that team wins.
- If scores are equal, the timer remains at `00:00` and the match enters sudden death.
- The shared announcement panel displays `NEXT GOAL WINS`.
- The next valid goal ends the match.
- Winner presentation uses shared text, particles and cheering audio.

### Accepted goal sequence

After every accepted goal:

1. Add one point to the correct team and synchronize the score.
2. Block all further goal detection immediately.
3. Show `GOAL FOR RED TEAM` or `GOAL FOR BLUE TEAM` in the shared announcement panel.
4. Play a short goal sound for everyone.
5. Stop the football, clear linear and angular velocity, and move it to the centre spot.
6. Keep the football unavailable or frozen for about two seconds.
7. Clear the goal announcement.
8. Re-enable the football and goal detection for the next kickoff.

If the accepted goal is the deciding sudden-death goal, skip the normal restart and finish the match with the winner presentation instead.

The exact network ownership and Rigidbody reset implementation will be designed and tested later.

## Existing scoreboard UI

Stef has already created:

- Red and Blue player lists;
- game timer;
- Red and Blue scores;
- time controls and No Limit;
- Start Game;
- Join Red;
- Join Blue;
- Leave Game;
- Reset Game;
- manual score correction controls;
- a large shared announcement panel above the scoreboard.

UI objects display manager state and do not store independent match state.

## Likely smallest architecture

Proposed, not yet approved for implementation:

- `SportsModeManager` remains sport-mode truth.
- One generic match manager stores players, teams, permissions, timer, scores, sudden death, announcements and result.
- Two goal detectors report only which goal was entered.
- One ball reset component or manager reference returns the football to its centre anchor.
- One configurable action-button behaviour routes Inspector-selected actions.
- Scoreboard visuals only render manager state.

## Current design question

Decide this next:

**What exact winner message should appear after the match?**

Recommended first-release format:

- first line: `RED TEAM WINS` or `BLUE TEAM WINS`;
- second line: the registered player names from the winning team;
- play cheering audio and the winning team's particles;
- keep the message visible until Reset Game.

Discuss only this question next.

## Remaining open questions

After the current question is resolved, discuss one at a time:

1. Whether team membership persists after a completed match.
2. Exact Reset Game confirmation interaction.
3. How the UI visually shows locked and open teams.
4. Whether No Limit remains in the first release.
5. Exact announcement timing for start and team-lock messages.
6. Whether manual score corrections are allowed during sudden death.

## Risks

- Creating a second source of truth.
- Double-scoring inside a goal.
- Incorrect ownership or Rigidbody state during ball reset.
- Networking failures for late join, rejoin or owner transfer.
- Accidental reset of an active match.
- Expanding the generic button system beyond release needs.

## Exact next step for Stef

Answer only:

Should the final announcement show the winning team name plus all registered player names from that team, and remain visible until Reset Game?

No Unity changes are needed yet.

## Do not do yet

- Do not create the generic match manager.
- Do not create goal triggers.
- Do not alter the ball.
- Do not edit basketball scripts or prefabs.
- Do not create the universal button prefab.
- Do not connect score buttons.
- Do not add synchronized variables.
- Do not alter the Unity hierarchy.