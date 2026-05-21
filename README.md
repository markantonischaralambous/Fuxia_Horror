# Aigia Fouxia - Game Notes

Updated: 2026-05-21

## Overview

Aigia Fouxia is a first-person horror quest game set around a cursed village and cemetery. The player wakes up, speaks with strange local NPCs, explores the area, collects missing grave crosses, returns them to their places, survives ghost encounters, and escapes through the gate.

The game mixes horror exploration with character dialogue. Pelomalou acts as an early flavor NPC who points the player toward the church and helps establish the setting's odd tone.

## Core Player Goal

1. Wake up and explore the village.
2. Talk to NPCs to understand the curse.
3. Start the cross quest.
4. Find all 3 missing grave crosses.
5. Return the crosses to the correct placement zones.
6. Avoid the ghost while completing the objective.
7. Unlock the exit gate.
8. Escape and reach the final memorial screen.

## Controls

| Action | Input |
| --- | --- |
| Move | `WASD` |
| Look | Mouse |
| Sprint | `Left Shift` |
| Jump | `Space` |
| Interact / Continue dialogue | `E` |
| Continue dialogue | Left mouse button |
| Toggle lantern | `F` |
| Unlock cursor | `Escape` |

## Quest State Flow

Progress is controlled by `GameManager.storyState`.

| State | Current Meaning |
| --- | --- |
| `0` | Intro state before the cross quest |
| `1` | Cross quest active |
| `2` | Cross quest complete / escape phase |
| `3+` | Reserved for later dialogue or extended story states |

## Cross Quest

The cross quest asks the player to collect and return 3 grave crosses.

Collectible crosses use:

- `Assets/Scripts/UI/QuestObjective.cs`

Placement zones use:

- `Assets/Scripts/UI/QuestPlacementZone.cs`

The live inventory count is stored in:

```csharp
QuestObjective.playerInventoryCount
```

During the active cross quest, the top-right HUD displays:

```text
Crosses: 0/3
```

The counter is created at runtime by:

- `Assets/Scripts/UI/GameManager.cs`

It appears only during story state `1` and hides once the quest advances to state `2`.

## Ending Flow

The ending is handled by:

- `Assets/Scripts/UI/ExitEscapeController.cs`

When the exit gate is unlocked and the player presses `E` near it:

1. The player freezes in place.
2. The text `YOU ESCAPED` appears.
3. The game waits 3 seconds.
4. Enemy and ghost objects are disabled so they cannot continue chasing the player.
5. A black full-screen ending appears.

The ending screen displays:

```text
THANKS FOR PLAYING
In memory of Dora Kakouratou
```

The memorial image is:

- `Assets/Npcs/IN_MEMORY.jpg`

A build-safe copy is also stored at:

- `Assets/StreamingAssets/IN_MEMORY.jpg`

## NPCs

### Pelomalou

Pelomalou is a roaming NPC with intro/flavor dialogue. She uses:

- `Assets/Scripts/NPCRoam.cs`
- `Assets/Scripts/UI/DialogueTrigger.cs`

Her roaming has been hardened so she avoids choosing paths through non-trigger colliders. The roam script now checks for complete NavMesh paths and re-picks destinations if the current route is blocked.

### Quest NPCs

Quest NPC dialogue can advance the story state after dialogue ends. Dialogue is managed by:

- `Assets/Scripts/UI/DialogueTrigger.cs`
- `Assets/Scripts/UI/DialogueUI.cs`
- `Assets/Scripts/UI/DialogueData.cs`

## Ghost / Enemy

The main ghost behavior uses:

- `Assets/Scripts/Enemy/EnemyAI.cs`
- `Assets/Scripts/Enemy/DeathManager.cs`
- `Assets/Scripts/Enemy/JumpScare.cs`
- `Assets/Scripts/Enemy/CameraShake.cs`

The enemy can patrol, detect, chase, attack, play audio, and trigger the death sequence. During the ending sequence, enemy and ghost objects are disabled by `ExitEscapeController` so the ending screen remains safe and uninterrupted.

## Runtime UI

Several UI elements are created by scripts at runtime:

| UI | Script |
| --- | --- |
| Dialogue panel | `DialogueUI.cs` |
| Cross counter | `GameManager.cs` |
| Exit prompt | `ExitEscapeController.cs` |
| Exit unlocked / escaped text | `ExitEscapeController.cs` |
| Final memorial screen | `ExitEscapeController.cs` |

## Important Scene Objects

| Object | Purpose |
| --- | --- |
| `Player` | First-person playable character |
| `GameManager` | Story state, ghost spawning, cross counter |
| `Pelomalou` | Roaming intro/flavor NPC |
| `Hidden_Cross1` | Collectible cross |
| `Hidden_Cross2` | Collectible cross |
| `Hidden_Cross3` | Collectible cross |
| `LostCrossStones` / placement zones | Cross return locations |
| `Ghost Chase` / enemy object | Main hostile ghost |
| `Fence_Gate` object | Escape gate target |

## Main Scripts

| Script | Role |
| --- | --- |
| `FirstPersonController.cs` | Player movement, jumping, mouse look |
| `LanternToggle.cs` | Lantern flame/light toggle |
| `GameManager.cs` | Story state, ghost spawn state, cross counter |
| `DialogueTrigger.cs` | NPC interaction and story progression |
| `DialogueUI.cs` | Dialogue display and typing effect |
| `QuestObjective.cs` | Quest pickup logic and cross inventory |
| `QuestPlacementZone.cs` | Cross return logic and exit unlock |
| `ExitEscapeController.cs` | Gate escape and final ending screen |
| `NPCRoam.cs` | Roaming NPC navigation |
| `EnemyAI.cs` | Main ghost patrol/chase/attack behavior |
| `DeathManager.cs` | Death sequence coordination |
| `JumpScare.cs` | Jumpscare visuals/audio |
| `CameraShake.cs` | Camera shake effect |

## Development Notes

- Unity version: `2021.3.45f2`.
- Text rendering uses TextMesh Pro.
- Navigation uses Unity NavMesh and `NavMeshAgent`.
- The NavMesh should be rebaked after major level geometry changes.
- If an NPC or ghost can walk through geometry, check both the baked NavMesh and the collider setup.
- Memorial image loading checks `Assets/Npcs/IN_MEMORY.jpg` first, then `Assets/StreamingAssets/IN_MEMORY.jpg`.

## Recent Updates

- Added the `Crosses: 0/3` HUD counter.
- Fixed the counter to appear during the real active cross quest state.
- Improved Pelomalou's roaming so she avoids collider-blocked paths.
- Added a 3-second escape pause with `YOU ESCAPED`.
- Added the final black memorial screen.
- Corrected the memorial name to `Dora Kakouratou`.
- Disabled ghost/enemy objects when the ending sequence starts.
