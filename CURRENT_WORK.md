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
- At equal score and `00:00`, sudden death begins and the scoreboard shows `NEXT GOAL WINS`.
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

## Approved architecture decisions

### One central `SportsMatchManager`

One manually synchronized UdonSharp behaviour is the only source of truth for the complete match.

Small helper behaviours remain separate:

- `SportsMatchButton` sends one Inspector-selected action request.
- `SportsGoalDetector` reports which goal was entered.
- `SportsScoreboardView` refreshes TMP texts, labels, colours and visibility from manager state.
- The manager commands the football reset through its configured Rigidbody, VRC Object Sync and centre anchor.

### Explicit match phases

`SportsMatchManager` uses five internal phases:

1. `READY` — no official match is running.
2. `PLAYING` — timer runs and valid goals count.
3. `GOAL_PAUSE` — a normal goal has counted and duplicate goals are blocked while the ball resets.
4. `SUDDEN_DEATH` — internal technical name only; visitors see `NEXT GOAL WINS`.
5. `FINISHED` — winner is fixed until Reset Game.

Reset and Clear All Players confirmations are temporary confirmation data, not match phases.

### Approved synchronized snapshot

The manager synchronizes only the shared values needed to reconstruct one coherent match:

- match phase;
- Red score;
- Blue score;
- configured match duration in seconds;
- network end timestamp for the active countdown;
- team-switching open/locked state;
- winner team;
- Red registered player IDs;
- Blue registered player IDs;
- current persistent announcement type;
- announcement sequence number so one-shot presentation can be recognized;
- goal-pause end timestamp while the football is blocked.

The visible countdown is calculated locally from the shared network end timestamp. The timer therefore does not need to serialize every second.

Temporary hover visuals and ordinary local redraw data are not synchronized. Confirmation authority is still validated by the manager.

## Current architecture question

**How should button actions safely change the synchronized manager?**

Recommended first-release ownership rule:

1. A player presses a `SportsMatchButton`.
2. The button sends the selected action and the local player identity to `SportsMatchManager`.
3. The manager first checks permission and current match phase.
4. For an allowed action, the local player takes ownership of the manager object.
5. Only after ownership is confirmed does the manager change its fields.
6. The manager calls `RequestSerialization()` once after the complete accepted change.
7. Rejected actions do not take ownership and do not serialize match state; they only show the appropriate feedback.

This keeps one clear ownership target and avoids partially synchronized changes.

Discuss only this decision next.

## Architecture topics after this decision

Review one at a time:

1. team-registration storage and player-leave cleanup;
2. network-time countdown and late-join reconstruction;
3. goal validation and anti-double-score flow;
4. football ownership, reset and Rigidbody handling;
5. announcements, audio and particle event reconstruction;
6. configurable button actions and automatic view refresh;
7. smallest build-and-test order.

## Do not do yet

- Do not create `SportsMatchManager`.
- Do not create goal triggers.
- Do not alter the football.
- Do not edit basketball scripts or prefabs.
- Do not create or connect action buttons.
- Do not add synchronized variables in Unity.
- Do not alter the Unity hierarchy.