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
