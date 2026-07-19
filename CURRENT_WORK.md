# Current Work

## Active feature

`FEATURES/SCOREBOARD`

## Current phase

Architecture design.

The first-release interaction design is approved and complete.

Do not start Unity implementation or write scripts yet.

## Current milestone

M2 — Approve the smallest reliable technical architecture for the shared Soccer and Soccer Hockey match system.

## Confirmed project setup

- Platform: VRChat Worlds, Unity 2022.3 LTS, UdonSharp.
- Soccer and Soccer Hockey share one football and two goals.
- Switching between those modes does not reset the match.
- Existing scripts are under `CURRENT_UNITY_STATE/SCRIPTS/`.
- Existing screenshots are under `CURRENT_UNITY_STATE/SCREENSHOTS/`.
- The protected basketball reference is under `REFERENCE_PACKAGES/Basketball/`.
- `SportsModeManager.cs` remains the synchronized sport-mode truth and is not replaced by the scoreboard system.

## Approved first-release experience

The interaction design is complete and remains the specification for architecture work.

Key accepted rules:

- Players manually join Red or Blue.
- Start requires at least one player in both teams.
- A timed match locks teams unless a registered player enables team switching.
- Goals, score, timer, teams, match state and lock state are shared and reconstruct correctly for late joiners.
- At equal score and `00:00`, sudden death begins and the next physical goal wins.
- A normal goal shows the scoring team, plays a sound, blocks duplicate scoring, resets the ball to centre and pauses for about two seconds.
- The winner message is only `RED TEAM WINS` or `BLUE TEAM WINS`.
- Normal Reset Game preserves both team lists and uses a same-player, two-press confirmation.
- `CLEAR ALL PLAYERS` is separate, works only when no match is active and confirms on the same button.
- `No Limit` is excluded from the first release.
- Manual score correction is blocked during sudden death.
- Short status messages clear after about two seconds.
- UI text and colours update automatically from shared manager state and never own independent match state.

## Architecture principles already fixed

- Manager = truth.
- Buttons = input requests.
- Visuals = display only.
- Goal detectors report events and never keep score.
- `SportsModeManager` remains independent sport-mode truth.
- The protected basketball package remains read-only reference material.
- The first release must stay small, understandable and testable.

## Approved architecture decision

### One central `SportsMatchManager`

One manually synchronized UdonSharp behaviour is the only source of truth for the complete match.

It owns:

- Red and Blue registered player IDs;
- Red and Blue scores;
- configured match duration;
- match phase;
- network start timestamp;
- team-switching lock state;
- winner state;
- shared announcement state;
- goal/reset lock state needed to prevent duplicate scoring.

Other components do not store competing synchronized copies of this truth.

### Small helper behaviours

- `SportsMatchButton`: sends one Inspector-selected action request to `SportsMatchManager`.
- `SportsGoalDetector`: reports whether the Red or Blue goal was entered.
- `SportsScoreboardView`: reads manager state and refreshes TMP texts, button labels, colours and visibility.
- Ball reset remains commanded by the manager through the configured football Rigidbody, VRC Object Sync and centre anchor; whether a tiny dedicated helper is needed will be decided from the actual ball setup.

### Why this is approved

- One ownership target for match changes.
- One serialization point after accepted state changes.
- Late joiners reconstruct one coherent snapshot.
- Less chance that timer, teams, score and announcement disagree.
- Easier to debug and explain in noob-friendly Unity steps.
- Input, detection and display still remain cleanly separated.

This is architecture approval only and is not permission to create scripts yet.

## Current architecture question

**Which explicit match phases should `SportsMatchManager` use?**

Recommended first-release state machine:

1. `READY` — no official match is running; players may join and manual score controls may be used.
2. `PLAYING` — timer is running and valid goals count.
3. `GOAL_PAUSE` — a normal goal has counted; duplicate goals are blocked while the ball resets for about two seconds.
4. `SUDDEN_DEATH` — timer is at zero, score is tied and only the next physical goal may decide the match.
5. `FINISHED` — winner is fixed; scoring and match controls remain blocked until Reset Game.

The reset-confirmation and clear-player-confirmation windows should be temporary confirmation data, not separate match phases.

Discuss only this decision next.

## Architecture topics after this decision

Review one at a time:

1. exact synchronized fields;
2. ownership and serialization rules for button actions;
3. team-registration storage and player-leave cleanup;
4. network-time countdown and late-join reconstruction;
5. goal validation and anti-double-score flow;
6. football ownership, reset and Rigidbody handling;
7. announcements, audio and particle event reconstruction;
8. configurable button actions and automatic view refresh;
9. smallest build-and-test order.

## Do not do yet

- Do not create `SportsMatchManager`.
- Do not create goal triggers.
- Do not alter the football.
- Do not edit basketball scripts or prefabs.
- Do not create or connect action buttons.
- Do not add synchronized variables in Unity.
- Do not alter the Unity hierarchy.