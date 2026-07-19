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

**Boundary:** The exact team-entry method, result wording and generic architecture are still open. This decision is not permission to copy or modify the protected basketball scripts.

## D-005 — Configurable action-button pattern worth reusing
**Status:** Accepted as a design pattern, not yet approved for implementation

The `BasketBallButtons` pattern is a useful reference: Unity UI invokes one `Interact()` entry point while an Inspector enum selects the button action. References determine which manager or settings object receives the action.

**Reason:** This makes buttons easier to duplicate and configure without wiring a different method manually for every action.

**Boundary:** A generic Sports Gym or Art House button prefab must be separately designed and tested. Do not build a universal framework during the current inventory step.
