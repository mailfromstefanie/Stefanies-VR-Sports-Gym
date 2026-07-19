# Source of Truth Rules

Use this order when information conflicts:

1. A result Stef has just reproduced in the current Unity project or uploaded VRChat build.
2. Current scripts and Inspector values that have been directly inspected.
3. `CURRENT_WORK.md` for the active phase, goal, risks and next step.
4. The active feature documents under `FEATURES/`.
5. `DECISION_LOG.md` for accepted project-wide choices.
6. `BUILD_JOURNAL.md` for historical progress.
7. `ROADMAP.md` for future priorities.
8. Chat memory, assumptions and old experiments.

## Status language
Always distinguish between:
- reported;
- inspected;
- proposed;
- built;
- tested in Unity;
- tested in VRChat;
- accepted for release.

## Safety rule
Never present an untested script or proposed hierarchy as though it is already active in the Unity project.

## Phase rule
Reading the repository is not permission to build. Stay in the phase recorded in `CURRENT_WORK.md` until Stef explicitly approves moving forward.
