# Scoreboard — Start Here

## Purpose
Create the smallest reliable shared scoreboard needed to publish Stefanie's VR Sports Gym.

## Current phase
Inventory. No architecture is approved yet.

## Reported existing setup
- A scoreboard UI already exists.
- Two existing scripts switch between sport modes.
- Multiple ball-game modes already exist.

These facts still require inspection before implementation.

## First milestone
Understand the existing sport-mode source of truth and the current UI before adding score logic.

## Minimum release candidate
Likely scope, not yet approved:
- Team A and Team B score;
- increase and decrease controls;
- reset;
- active sport-mode display;
- shared state for all players;
- correct state for late joiners;
- predictable mode-switch behaviour;
- PC and Quest support.

## Deferred unless deliberately approved
- automatic goal detection;
- timers and sets;
- player names;
- statistics and leaderboards;
- persistent results;
- tournament systems;
- elaborate animations.

## Exact next step
Inspect the first existing sport-mode script without editing it.
