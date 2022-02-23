Standing on an incline in VR causes motion sickness. This is because a player's horizontal movements within their playspace translate to vertical movements in VR. This prefab solves the problem by replacing the inclined collider with a flat one while the player's only motion is in playspace.
---

HOW TO USE:
1. Drag prefab into scene anywhere and unpack
2. Find the "Ramp" field in the "program" object, and drag in the ramp collider you want to replace
3. To toggle the comfort functionality at runtime, simply deactivate/activate the "program" object

If desired, delete:
- example ramp mesh
- example collider ramp
- Demo UI panel
- debug object & children

- You may use multiple instances of the prefab in your scene with no issue.
- You may combine multiple ramp colliders into a single mesh collider to be used in one instance of the prefab, subject to warnings below.

WARNINGS:
- Beware of unforseen physics simulation consequences! While the local player stands still, the "Ramp" collider you specify will be disabled. This means rigidbodies on that will fall down. There are other possible edge cases like raycasts.
- Players may use the flat collider to pass through your ramp collider completely, or to hover above the ground and climb on top of things unexpectedly. Be cautious using this prefab in any game or puzzle world where cheating is a concern.