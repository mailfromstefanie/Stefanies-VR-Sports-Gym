# Scoreboard Design

## Design status
Not yet approved. This document records questions before architecture is chosen.

## Desired experience
Players should immediately understand:
- which sport is active;
- which side or team owns each score;
- how to add, remove or reset points;
- whether they are allowed to operate the controls.

The scoreboard should remain simple during play and should not require technical knowledge.

## Open design questions
1. Does every sport use exactly two scores?
2. Does each sport keep its own score, or does switching modes reset the board?
3. May every player operate it, or only trusted users?
4. Are points manual only for the first release?
5. Can a score ever increase by more than one?
6. Should reset require confirmation?
7. What labels are already present in the UI?

## Architecture constraint
The active sport mode and scores must each have one clear source of truth. UI components display state but do not independently own it.
