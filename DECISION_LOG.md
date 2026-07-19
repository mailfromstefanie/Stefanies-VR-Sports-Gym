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

**Boundary:** Trigger details, anti-double-score protection and ball reset behaviour are not yet final and must be prototyped later.

## D-009 — Shared match state supports late join and rejoin
**Status:** Accepted at architecture-intent level

The future match manager must synchronize state changes such as registration, teams, match start and accepted goals. Late joiners and rejoining players must reconstruct the current match from synchronized state rather than relying only on local UI history.

**Reason:** Every player must see the same score, teams, remaining time and match status.

**Boundary:** Exact synced fields and ownership handling will be chosen during architecture design, not during the current interaction-design step.

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

**Reason:** The people already playing control whether the running match admits or moves players. This keeps the system social without allowing an outsider to disrupt the match.

**Boundary:** Any registered player may operate the control; there is no separate captain role in the first release.

## D-013 — Both teams require a player before start
**Status:** Accepted

`Start Game` succeeds only when Red has at least one registered player and Blue has at least one registered player. If either team is empty, the manager refuses to start the match and shows a short status message.

**Reason:** This prevents accidental empty or one-sided matches and makes the start condition clear to everyone.

**Preferred status text:** `Both teams need a player`.