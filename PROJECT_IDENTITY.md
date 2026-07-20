# Project Identity

## Name

Stefanie's VR Sports Gym

## Repository

`mailfromstefanie/Stefanies-VR-Sports-Gym`

## Platform

- VRChat Worlds
- Unity 2022.3 LTS
- UdonSharp
- PC and Quest support

## Product goal

Finish and publish an existing VR sports hall containing several ball-game modes.

The remaining release-critical feature is a simple, reliable shared scoreboard that works with the existing sport-mode switching system.

## Known existing elements

- Basketball mode
- Soccer mode
- Volleyball mode
- Soccer Hockey mode
- Existing synchronized sport-mode switching
- Existing scoreboard UI
- Existing balls, fields and world environment
- SoccerBox football system with its own behaviour and synchronization

The exact inspected state is documented in `CURRENT_WORK.md`, `BUILD_JOURNAL.md`, and `CURRENT_UNITY_STATE/`.

## Quality goal

Use the smallest dependable solution needed for a public release. Do not expand the project into a large tournament platform before the first release.

## Working principles

- Inspect before changing.
- Manager = truth.
- Button = input.
- Visual = display.
- Clearly separate local and global behaviour.
- Test PC, Quest, networking, ownership, late joiners and player departure.
- Build and test one small step at a time.
- Preserve working third-party and existing systems unless the active microstep explicitly requires a change.

## New-chat handoff

A new development chat must read:

1. `CURRENT_WORK.md`
2. `SOURCE_OF_TRUTH_RULES.md`
3. `COLLABORATION_RULES.md`
4. the active feature folder named in `CURRENT_WORK.md`

Use `START_PROMPT.md` or the mobile copy at `FOR_STEF/Startprompt_Stefanies_VR_Sports_Gym.txt`.

The current phase in `CURRENT_WORK.md` controls whether the chat should design, build, test, or stop. Reading the repository is not permission to skip phases or provide several steps at once.
