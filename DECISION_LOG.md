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