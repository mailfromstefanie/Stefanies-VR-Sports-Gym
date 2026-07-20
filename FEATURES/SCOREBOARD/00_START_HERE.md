# Scoreboard — Start Here

Status: Active feature

## Current phase

Implementation in small verified microsteps.

The first-release interaction design and technical architecture are already approved. Do not reopen or expand the design unless a test exposes a real problem.

## Current source of truth

Read `CURRENT_WORK.md` before doing anything. It contains:

- the current microstep;
- what is already completed;
- what may be changed now;
- what must not be changed yet;
- the pass condition.

## Feature purpose

Build a simple, reliable, synchronized scoreboard for Soccer and Soccer Hockey that works with the existing sports-mode system and remains correct for:

- multiple players;
- ownership transfer;
- late joiners;
- player departure;
- inactive scoreboard roots;
- PC and Quest.

## Approved architecture

- `SportsMatchManager` owns synchronized match truth.
- `SportsMatchButton` sends actions and owns no match state.
- `SportsScoreboardView` renders manager state.
- `SportsModeManager` remains the truth for the selected sport mode.
- Scoreboard refresh is event-driven rather than per-frame.
- Existing SoccerBox football systems must be preserved.

## Working rule

Implementation still means one microstep at a time.

Before giving an instruction:

1. Read the exact current microstep and pass condition in `CURRENT_WORK.md`.
2. Explain briefly what the step will prove.
3. Give one small action.
4. Stop and wait for Stef's real result.
5. Do not proceed merely because the expected result seems obvious.

## Current microstep

The active microstep is defined only in `CURRENT_WORK.md`. Do not duplicate or guess it here, because this file is the stable feature entry point.

## Read when needed

- `CURRENT_WORK.md` — current execution truth
- `SOURCE_OF_TRUTH_RULES.md` — conflict resolution and status language
- `COLLABORATION_RULES.md` — how to guide Stef
- `BUILD_JOURNAL.md` — completed tests and historical progress
- `DECISION_LOG.md` — accepted project-wide decisions
- `CURRENT_UNITY_STATE/SCRIPTS/` — inspected script copies
- `CURRENT_UNITY_STATE/SCREENSHOTS/` — inspected Unity-state evidence

## Guardrails

Do not add timer flow, goals, score permissions, reset confirmation, particles, audio, or football changes unless `CURRENT_WORK.md` explicitly makes one of them the active microstep.

Do not treat a successful compile as proof of correct networking.

Do not treat a local test as proof of correct late-joiner or ownership behaviour.
