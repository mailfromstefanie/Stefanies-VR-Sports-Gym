# Decision Log

## D-001 — Separate repository
**Status:** Accepted

Stefanie's VR Sports Gym uses its own repository and is not mixed with `SPIN-Table-Tennis` or `Stefanies-Art-House-Cinema`.

**Reason:** The projects have different worlds, goals, scripts and release states. Separation prevents old assumptions and documentation from contaminating the sports hall.

## D-002 — Start with inventory
**Status:** Accepted

The first phase is inspection of the existing scripts and scoreboard UI. No replacement architecture will be written before this inspection.

**Reason:** The world already contains working systems. Reusing the actual mode source of truth is safer and simpler than building a competing system.

## D-003 — Protect the first public release
**Status:** Accepted

The first scoreboard should be the smallest reliable solution needed for publication. Advanced game automation is deferred unless later approved.

## D-004 — Registered-player match experience
**Status:** Accepted

The first public-release score system will use registered players rather than only anonymous Left Team and Right Team scores. Players must be able to Join and Leave, be associated with a team and have their names available for score or result presentation.

**Reason:** Stef wants the match ending to identify the actual winners, and the protected basketball reference already proves that synchronized player registration, team assignment and player-name display are feasible.

**Boundary:** The result wording and final architecture remain subject to design. This decision is not permission to copy or modify the protected basketball scripts.

## D-005 — Configurable action-button pattern worth reusing
**Status:** Accepted as a design pattern, not yet approved for implementation

The `BasketBallButtons` pattern is a useful reference: Unity UI invokes one `Interact()` entry point while an Inspector enum selects the button action. References determine which manager or settings object receives the action.

**Reason:** This makes buttons easier to duplicate and configure without wiring a different method manually for every action.

**Boundary:** A generic Sports Gym or Art House button prefab must be separately designed and tested. Do not build a universal framework during the current inventory step.

## D-006 — Manual Red or Blue team choice
**Status:** Accepted

Registered players choose their own team through separate `Join Red` and `Join Blue` actions instead of being randomly assigned when the match starts.

**Reason:** Stef wants players to decide which side they play for. The UI can use the accepted configurable action-button pattern.

## D-007 — Current release match scope is Soccer and Soccer Hockey
**Status:** Accepted

The generic match system currently needs to complete only Soccer and Soccer Hockey. Both modes share one football, two goals and the same Red-versus-Blue scoring model. Soccer Hockey adds hockey sticks and a changed field setup but still uses the football.

**Reason:** Limiting scope to the two unfinished games gives the shortest path to a public release and avoids designing unnecessary adapters for Basketball or Volleyball now.

## D-008 — Goal scores for the opposing team
**Status:** Accepted at interaction-design level

When the football enters Red's goal, Blue receives one point. When it enters Blue's goal, Red receives one point. Each goal will use a detection volume that reports the event to one central match manager.

**Reason:** This matches normal goal-sport expectations and allows Soccer and Soccer Hockey to share the same scoring system.

**Boundary:** Trigger details and anti-double-score protection must still be prototyped.

## D-009 — Shared match state supports late join and rejoin
**Status:** Accepted at architecture-intent level

The future match manager must synchronize state changes such as registration, teams, match start and accepted goals. Late joiners and rejoining players must reconstruct the current match from synchronized state rather than relying only on local UI history.

**Reason:** Every player must see the same score, teams, remaining time and match status.

**Boundary:** Exact synced fields and ownership handling will be chosen during architecture design.

## D-010 — Match remains active across Soccer and Soccer Hockey switching
**Status:** Accepted

Changing between Soccer and Soccer Hockey does not automatically cancel or reset an active match. The football, both goals and scoring model are shared between the two modes. Resetting the match is a separate deliberate action.

**Reason:** Tying the match lifetime to the visual mode switch would add unnecessary coupling and could destroy a valid ongoing game.

## D-011 — Teams lock at match start and require explicit reopening
**Status:** Accepted

When `Start Game` is pressed, Red and Blue lock. While locked, new players cannot enter the active match and registered players cannot switch teams. A shared `Allow Team Switching` control must be enabled before either action becomes possible during the match. Turning it off locks the teams again.

**Reason:** An active match should not silently change because a late visitor presses Join. Reopening teams is a deliberate social decision by the current group.

## D-012 — Only registered players may reopen teams
**Status:** Accepted

Only a player who is already registered in Red or Blue may toggle `Allow Team Switching` during an active match. A spectator or late visitor who is not part of either team cannot open the teams.

**Reason:** The people already playing control whether the running match admits or moves players.

**Boundary:** Any registered player may operate the control; there is no separate captain role in the first release.

## D-013 — Both teams require a player before start
**Status:** Accepted

`Start Game` succeeds only when Red has at least one registered player and Blue has at least one registered player. If either team is empty, the manager refuses to start the match and shows a short status message.

**Preferred status text:** `Both teams need a player`.

## D-014 — Equal score enters sudden death
**Status:** Accepted

When the countdown reaches zero and Red and Blue have equal scores, the timer stops at `00:00` but the match remains active in sudden death. The next valid goal immediately ends the match and determines the winning team.

**Reason:** Stef prefers a decisive and exciting finish instead of a draw.

## D-015 — Shared announcement panel for match messages
**Status:** Accepted

The large extra text field above the scoreboard becomes the central announcement panel for important shared match messages. It can show countdown or phase messages, start warnings, validation errors, team-lock status, sudden death and the final winner.

At normal time expiry, a buzzer plays. If the score is tied, the timer remains at `00:00` and the announcement panel shows `NEXT GOAL WINS`, preferably with a restrained blinking or pulsing effect until the deciding goal is accepted.

## D-016 — Registered players control Start and Reset
**Status:** Accepted

When one or more players are registered, only a player currently registered in Red or Blue may use `Start Game` or `Reset Game`.

When no players are registered at all, any visitor may use Reset Game or other setup controls needed to return the board to a clean waiting state. `Start Game` still cannot begin until both Red and Blue contain at least one registered player.

**Boundary:** Reset Game still needs a deliberate confirmation or hold interaction to reduce accidental resets.

## D-017 — Valid goals automatically reset the football to centre
**Status:** Accepted

After every accepted goal, the football automatically returns to the centre spot for a new kickoff. Further goal detection is temporarily blocked until the reset is complete.

**Reason:** This gives players a clear restart, prevents the ball from bouncing inside the goal and being counted twice, and removes the need for a separate respawn action after every score.

**Boundary:** The exact ownership transfer, Rigidbody reset, delay and goal-detector lockout will be chosen and tested during architecture and prototype work.

## D-018 — Goals use a shared announcement, sound and short restart pause
**Status:** Accepted

After an accepted goal, the shared announcement panel shows `GOAL FOR RED TEAM` or `GOAL FOR BLUE TEAM`, a short goal sound plays for everyone, and the football is reset to the centre with velocity and angular velocity cleared. Goal detection remains blocked and the ball stays unavailable for about two seconds before the next kickoff.

**Reason:** Players immediately understand which team scored, have time to see the updated score, and can reposition before play resumes.

**Boundary:** If the goal decides sudden death, the normal two-second restart is skipped and the match proceeds directly to winner presentation.