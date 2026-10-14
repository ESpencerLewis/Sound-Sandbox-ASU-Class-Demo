# Solutions and troubleshooting

If something doesn't work, **open `SoundSandbox-Finished`** (Unity Hub, Add, Add project from disk) and compare its Inspector with yours, object by object.
Use the table to see what each object should look like, then the checklist below it.

## What each object should look like (the finished demo)
| Object | Components and settings |
|---|---|
| **Player** (capsule) | Tag **Player**. Character Controller, **Sandbox Mover**, **Footstep Player** (Clips: Footsteps1 to 6, Step Distance 1.8), and an Audio Source. The **Main Camera** is its child. |
| **Main Camera** | The only **Audio Listener** in the scene, tag **MainCamera**. |
| **Ambient (will play everywhere)** | Audio Source: **AmbientLoop**, Play On Awake ON, Loop ON, Spatial Blend **0**, Volume 0.6. |
| **LevelSound** | **Click To Start**, Sources (size 4): Ambient, Danger Sound, Dialogue Clean, Dialogue Muffled. |
| **DANGER, child "Danger Sound"** | Audio Source: **DangerLoop**, Play On Awake ON, Loop ON, Spatial Blend **1**, Linear Rolloff, Min 2, Max 14. |
| **PICK UP** | Box Collider, **Is Trigger ON**. **Trigger One Shot**: Clip **Pickup**, Hide After ON. |
| **WIN** | Box Collider, **Is Trigger ON**. **Trigger One Shot**: Clip **Win**, Hide After OFF, Fade Out size 2 (Ambient and Danger Sound). |
| **DIALOGUE** | **Occlusion Crossfade**: Clean = Dialogue Clean, Muffled = Dialogue Muffled. |
| **DIALOGUE, children "Dialogue Clean" and "Dialogue Muffled"** | Audio Sources: **Dialogue** and **DialogueFilter**. Play On Awake ON, Loop ON, Spatial Blend **1**, Linear Rolloff, Min 2, Max 14. |
| **Inner Wall** | A normal (not trigger) Box Collider between the player's route and the DIALOGUE cube. |

## If something goes wrong
**No sound at all**
- There must be exactly **one Audio Listener** (on the Main Camera). Two listeners, or none, gives a warning or silence.
- Click in the **Game window** first so the keys work. In a Web build click the page, then **Click to start**.
- Check the audio clips are assigned in the **Audio Resource** slot, and the Audio Source's **Mute** box is off.
- All audio files imported with **Load Type: Compressed In Memory** (select them, Inspector, Apply).

**The Console shows red errors**
- Scripts must be in `Assets/Scripts` and the Console must show **no red** before anything works. A red error from one script stops the rest.
- Only keep one copy of each script. Two scripts with the same name give a "duplicate" error.

**Pick up or Win does nothing**
- The cube's Box Collider needs **Is Trigger ticked**.
- The Player needs the tag **Player** (spelled exactly) and a Character Controller (or a Rigidbody).
- **Trigger One Shot** must be on the cube itself, with a clip in the **Clip** slot.

**Danger or Dialogue is silent, or always loud**
- Spatial Blend must be **1** and **Rolloff Linear**. The default rolloff never reaches silence.
- It only becomes audible inside **Max Distance** (14). Walk toward the cube.
- Always loud: Spatial Blend is 0 (2D).

**The loops are out of time with each other**
- Every looping source must be listed in **Click To Start**, so they all start together. Add the missing ones.

**Dialogue never muffles**
- **Occlusion Crossfade** needs both **Clean** and **Muffled** filled in.
- You must be on the far side of the **Inner Wall** (a solid wall between you and the cube). Wall colliders must not be triggers.
- Both dialogue sources must be playing at the same time, so both are in Click To Start.

**No footsteps**
- **Footstep Player** is on the Player, and **Clips** has all six files in it. Walk with W; turning on the spot makes no steps.

**Works in the Editor but not in the Web build**
- On Web only **volume** works: no reverb and no low-pass filter. That's why the muffling uses two pre-made files and a crossfade.
- The browser may block sound until you click the page. Click it, then press **Click to start**.
- Build for the **Web** platform (File, Build Profiles, Web). Keep **Greybox** ticked in the scene list.

## The older C# exercise (optional)
`StudentExercise.cs` in the `Sandbox` folder is an exercise for the **Adaptive Mixer** (a Tense zone raises "intensity" while you stand in it).
It is separate from the demo above. Its finished version is `StudentExercise_Solution.txt` in the same folder.
