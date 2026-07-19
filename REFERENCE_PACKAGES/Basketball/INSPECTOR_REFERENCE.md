# Basketball Inspector Reference

Status: read-only reference. Do not modify the protected basketball prefab from this document.

## Grouping manager Inspector

The original `BasketBallGrouping` component exposes Inspector fields for:

- maximum players;
- world capacity;
- maximum match minutes;
- Red and Blue team names and colours;
- finish sound;
- total-score TextMeshPro fields;
- team player-name TextMeshPro fields;
- individual player-score TextMeshPro fields;
- timer minute, second and selected-duration TextMeshPro fields;
- overhead tag parent.

This confirms that the original package uses one manager with direct references to all scoreboard visuals.

## Configurable button Inspector

Each button uses the same `BasketBallButtons` behaviour.

The button has:

- a `Button Type` enum/dropdown;
- an optional Ball Settings reference;
- a Group Settings reference.

The Unity UI Button `On Click` invokes `UdonBehaviour.Interact()` on the same object. The selected enum then routes the action to the referenced settings object.

Available original actions are:

- Respawn Ball;
- Join Game;
- Leave Game;
- Start Game;
- Increase Game Minutes;
- Decrease Game Minutes;
- Game Minutes No Limit.

## Important differences from the Sports Gym design

Do not copy the original `StartGame()` behaviour unchanged.

The protected basketball manager:

- registers players first as unassigned;
- randomly distributes registered players between Red and Blue when Start Game is pressed;
- resets individual scores at match start.

The Sports Gym design instead requires:

- explicit `Join Red` and `Join Blue` actions;
- team choice before match start;
- teams locking at match start;
- deliberate temporary reopening through `Allow Team Switching`;
- no random redistribution on Start Game.

## Reusable parts

Safe patterns to reproduce in a new independent Sports Gym implementation:

- one manager as shared truth;
- serialized player/team state;
- network-time-based countdown;
- Inspector references to TextMeshPro visuals;
- one enum-driven button behaviour using `Interact()`;
- UI visuals reading manager state instead of owning it.

The original basketball scripts and prefabs remain protected reference material.