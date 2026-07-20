# Collaboration Rules

These rules define how a new chat should work with Stef on this repository.

## One step at a time

During implementation, give one concrete Unity or scripting step and then wait.

A single step may be:

- creating or selecting one GameObject;
- adding one component;
- assigning one Inspector reference;
- replacing one clearly identified script;
- running one compile check;
- performing one small local or network test.

Do not bundle several of these unless they are inseparable and still easy to verify.

## Explain before instructing

Before the action, explain briefly:

- what problem the step solves;
- why it is the smallest useful step;
- what should remain untouched.

## Verification

Never assume success.

After a step, wait for one of:

- Stef's observed result;
- a screenshot;
- a Console error;
- a ClientSim or VRChat test result;
- an uploaded script or log.

Then compare the observation with the pass condition in `CURRENT_WORK.md`.

## No accidental phase changes

Do not move from design to implementation, or from implementation to a broader feature, without explicit approval from Stef and an update to `CURRENT_WORK.md`.

A clear technical idea is not permission to build it.

## Preserve working systems

This is an existing Unity project. Do not redesign unrelated working systems during a small scoreboard task.

In particular:

- inspect before changing;
- preserve SoccerBox football behaviour and synchronization unless explicitly working on it;
- keep `SportsModeManager` as sport-mode truth;
- keep manager objects active when required for network state;
- avoid per-frame UI refresh when event-driven refresh is sufficient;
- do not introduce a large framework for a small release-critical feature.

## Language and teaching style

Use plain Dutch when speaking with Stef unless she asks otherwise.

Avoid unexplained jargon. When a technical term matters, explain it in one sentence.

Stef should always know:

- what to click or change;
- where to find it;
- what she should expect to see;
- when to stop and report back.

## Error handling

When something fails:

1. preserve the exact error or observation;
2. do not make several speculative changes at once;
3. identify the smallest likely cause;
4. test one correction;
5. record the result.

## GitHub discipline

GitHub is the project memory, not a place to pretend progress happened.

Record:

- what was actually built;
- what compiled;
- what was tested locally;
- what was tested with multiple clients;
- what remains uncertain;
- the exact next microstep.

Do not mark a proposal as accepted or tested without Stef's confirmation.
