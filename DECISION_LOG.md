# Decision Log

## D-001 — Separate repository
**Status:** Accepted

Stefanie's VR Sports Gym uses its own repository and is not mixed with `SPIN-Table-Tennis` or `Stefanies-Art-House-Cinema`.

## D-002 — Start with inventory
**Status:** Accepted

Inspect existing scripts and UI before writing replacement architecture.

## D-003 — Protect the first public release
**Status:** Accepted

Build the smallest reliable scoreboard needed for publication.

## D-004 — Registered-player match experience
**Status:** Accepted

Players can Join, Leave, belong to Red or Blue, and appear in the team lists.

## D-005 — Configurable action-button pattern
**Status:** Accepted as a design pattern

Unity UI calls one `Interact()` entry point while an Inspector enum chooses the action.

## D-006 — Manual Red or Blue team choice
**Status:** Accepted

Players choose `Join Red` or `Join Blue`; there is no random team assignment.

## D-007 — Release scope is Soccer and Soccer Hockey
**Status:** Accepted

Both modes share one football, two goals and one Red-versus-Blue match.

## D-008 — Goals score for the opposing team
**Status:** Accepted

Ball in Red goal gives Blue one point; ball in Blue goal gives Red one point.

## D-009 — Shared match state supports late join and rejoin
**Status:** Accepted

Teams, score, timer, lock state and match state are reconstructed from synchronized data.

## D-010 — Sport-mode switching does not reset the match
**Status:** Accepted

Changing between Soccer and Soccer Hockey leaves the active match intact.

## D-011 — Teams lock at match start
**Status:** Accepted

New players cannot join and existing players cannot switch unless `Allow Team Switching` is enabled.

## D-012 — Only registered players may reopen teams
**Status:** Accepted

Only a current Red or Blue player may toggle `Allow Team Switching`.

## D-013 — Both teams require a player before start
**Status:** Accepted

`Start Game` requires at least one registered player in each team. Preferred refusal text: `Both teams need a player`.

## D-014 — Equal score enters sudden death
**Status:** Accepted

At `00:00` with equal scores, the timer stays at zero and the next valid goal wins.

## D-015 — Shared announcement panel
**Status:** Accepted

The large upper text field shows shared match messages, including `NEXT GOAL WINS`.

## D-016 — Registered players control Start and Reset
**Status:** Accepted

When players are registered, only a current Red or Blue player may Start or Reset. When nobody is registered, any visitor may restore an abandoned board. Reset still needs a deliberate confirmation or hold interaction.

## D-017 — Valid goals reset the football to centre
**Status:** Accepted

After every accepted goal, the football returns to the centre and further scoring is blocked until reset is complete.

## D-018 — Goal announcement, sound and restart pause
**Status:** Accepted

After a normal goal, show `GOAL FOR RED TEAM` or `GOAL FOR BLUE TEAM`, play a goal sound, reset the ball and pause for about two seconds. A sudden-death winner skips the normal restart.

## D-019 — Winner announcement contains only the team
**Status:** Accepted

The final shared announcement is only `RED TEAM WINS` or `BLUE TEAM WINS`. Do not append player names.

**Reason:** Stef wants the ending bold, immediate and uncluttered. Registered names remain visible in the normal Red and Blue team lists.

**Presentation:** Play cheering audio and the winning team's particles. Keep the winner message visible until Reset Game.

## D-020 — Team membership persists after the result
**Status:** Accepted

Players remain registered in Red or Blue after a match finishes. Preparing a rematch clears the score, timer, sudden-death state, announcements and result, but keeps both team lists intact.

Players leave through `Leave Game` or change team only while team switching is allowed. A separate full cleanup may clear all registrations when the group deliberately wants new teams or when an abandoned board must be restored.

**Reason:** The same group can start a rematch without everyone having to join again.

## D-021 — Reset Game requires a second confirmation press
**Status:** Accepted

The first valid press of `Reset Game` shows `PRESS RESET AGAIN TO CONFIRM` and starts a confirmation window of about five seconds. The same player must press the button again within that window to perform the reset. Otherwise the pending confirmation expires without changing the match.

When nobody is registered, any visitor may use the same two-press confirmation to restore an abandoned board. A different player cannot complete another player's pending confirmation.

**Reason:** This prevents accidental resets while remaining reliable with ordinary Unity UI buttons in both VR and desktop mode.

## D-022 — Team status UI must explain itself in words
**Status:** Accepted

The team-switching control automatically displays its current shared state as `TEAM SWITCHING: LOCKED` or `TEAM SWITCHING: OPEN`, with short helper text explaining what pressing it will do. Colour may support the state but may never be the only indicator.

When someone attempts to join or switch while teams are locked, the shared announcement panel briefly shows `TEAMS ARE LOCKED` and `ASK A PLAYER TO OPEN TEAM SWITCHING`.

**Reason:** The scoreboard must be intuitive for visitors using VR for the first time. The manager owns the state; UI text and colours update automatically from that same state.

## D-023 — No Limit is excluded from the first release
**Status:** Accepted

The new first-release match manager will not support a `No Limit` match mode. Official matches are timed and use the normal buzzer, winner and sudden-death flow.

Visitors who only want to kick the ball around casually do not need to start an official match. They may use the existing manual score controls to keep an informal score without activating the timed match system.

**Reason:** A separate unlimited mode adds unnecessary state and UI complexity while casual play already works without it.

## D-024 — Short status messages clear after about two seconds
**Status:** Accepted

Brief confirmations such as `GAME STARTED`, `TEAMS LOCKED` and `TEAM SWITCHING OPEN` remain visible in the shared announcement panel for about two seconds and then clear automatically.

Persistent state stays visible on the relevant control itself. Important messages such as `NEXT GOAL WINS`, reset confirmation and the final winner follow their own longer display rules.

**Reason:** The announcement panel must give clear feedback without becoming occupied when a goal, warning or important match-state message needs to appear.

## D-025 — Manual score correction is blocked during sudden death
**Status:** Accepted

The manual Red and Blue plus/minus score controls cannot change the score while the match is in sudden death. Only an accepted physical goal may end sudden death and determine the winner.

Manual correction remains available before an official match and during normal timed play so players can fix an incorrect score.

**Reason:** Sudden death must be decided on the field, not accidentally or deliberately through the scoreboard.

## D-026 — Clear All Players uses its own self-confirming button
**Status:** Accepted

A separate `CLEAR ALL PLAYERS` action removes every registration from Red and Blue without being confused with the normal rematch reset.

The action is available only while no match is active. On the first valid press, the same button changes its visible text to `PRESS AGAIN TO CONFIRM` for about five seconds. The same player must press that button again within the confirmation window. When the window expires, the button automatically returns to `CLEAR ALL PLAYERS` and nothing is changed.

The shared announcement panel may simultaneously show `CLEAR ALL PLAYERS?` so the action is obvious to first-time VR users.

**Reason:** Confirmation on the button that was already pressed is more intuitive than asking a visitor to find a second confirmation control. Keeping the action separate prevents `Reset Game` from unexpectedly deleting the teams.

## D-027 — One central SportsMatchManager owns all match truth
**Status:** Accepted

One manually synchronized `SportsMatchManager` is the only source of truth for the complete Soccer and Soccer Hockey match.

It owns team registrations, scores, configured duration, match phase, network timing, team-switching state, winner, shared announcement state and duplicate-goal protection. Buttons, goal detectors and scoreboard visuals do not keep competing synchronized copies of that information.

Small helper behaviours remain separate for input, goal reporting and display refresh.

**Reason:** One ownership and serialization point gives late joiners one coherent snapshot, reduces disagreement between timer, teams and score, and stays easier to debug and explain.

## D-028 — Five explicit match phases with plain player-facing wording
**Status:** Accepted

`SportsMatchManager` uses the internal phases `READY`, `PLAYING`, `GOAL_PAUSE`, `SUDDEN_DEATH` and `FINISHED`.

`SUDDEN_DEATH` is only a technical state name in the script. The scoreboard never expects visitors to understand that term and instead displays the direct instruction `NEXT GOAL WINS`.

Reset confirmation and Clear All Players confirmation are temporary confirmation data, not extra match phases.

**Reason:** Explicit internal phases keep scoring, timing and ball-reset logic reliable, while plain visible wording keeps the experience intuitive for first-time VR users.

## D-029 — Synchronize one reconstructable match snapshot
**Status:** Accepted

`SportsMatchManager` synchronizes the match phase, Red and Blue scores, configured duration, network end timestamp, team-switching state, winner team, Red and Blue player IDs, persistent announcement type, announcement sequence number and goal-pause end timestamp.

The countdown display is calculated locally from the shared network end timestamp instead of serializing every second.

**Reason:** Late joiners can rebuild one coherent match while network traffic remains small and predictable.

## D-030 — Validate before ownership and serialize accepted changes once
**Status:** Accepted

The manager checks the requesting player, permissions and current match phase before taking ownership. Rejected actions do not take ownership and do not serialize match state.

For an accepted action, the requesting player takes ownership of the `SportsMatchManager`. After ownership is confirmed, all synchronized values belonging to that action are changed together and `RequestSerialization()` is called once.

**Reason:** This prevents partial updates, avoids needless ownership changes for invalid actions and keeps one clear serialization point.

## D-031 — Team registrations use player-ID slots and clean up departures
**Status:** Accepted

Red and Blue use fixed-size synchronized integer arrays containing VRChat player IDs, with `-1` representing an empty slot. Visible names are rebuilt locally from those IDs.

When a registered player leaves the instance, the manager owner removes that ID and serializes the cleaned lists. The match does not automatically reset or stop, even when one team becomes empty.

## D-032 — Countdown uses one shared network end timestamp
**Status:** Accepted

Starting a match stores one synchronized network end timestamp. Every client and late joiner calculates the displayed remaining time locally.

Only the current manager owner processes the transition at zero and synchronizes either the winner or `SUDDEN_DEATH` state.

## D-033 — Goals have detector-level and manager-level duplicate protection
**Status:** Accepted

Goal detectors react only to the configured football and must observe it leave before reporting another entry. The manager accepts reports only during `PLAYING` or `SUDDEN_DEATH` and immediately changes phase before further processing.

This ensures that bouncing, multiple colliders or repeated trigger callbacks cannot award multiple points for one goal.

## D-034 — Football reset preserves the SoccerBox system
**Status:** Accepted

The scoreboard does not replace or reconfigure the SoccerBox football, `SBBall`, child `BallTrigger`, Rigidbody physics or synchronization path. It does not add `VRC Object Sync` and does not convert the ball into a pickup.

After an accepted goal, the processor takes ownership of the football root, clears linear and angular velocity, moves it to the centre anchor and sends the reset through the existing `SBBall` synchronization path. The goal pause temporarily blocks scoring and interaction without permanently changing the established football physics.

The exact script calls and safest temporary blocking method must be proven in a small isolated test before modifying the working football.