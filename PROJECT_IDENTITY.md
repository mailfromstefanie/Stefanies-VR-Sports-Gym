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
These are reported by Stef but are not yet technically inspected:
- Basketball mode
- Soccer mode
- Volleyball mode
- Soccer Hockey mode
- Two scripts related to switching sport modes
- Existing scoreboard UI
- Existing balls, fields and world environment

## Quality goal
Use the smallest dependable solution needed for a public release. Do not expand the project into a large tournament platform before the first release.

## Working principles
- Inspect before changing.
- Manager = truth.
- Button = input.
- Visual = display.
- Clearly separate local and global behaviour.
- Test PC, Quest, networking, ownership and late joiners.
- Build and test one small step at a time.
