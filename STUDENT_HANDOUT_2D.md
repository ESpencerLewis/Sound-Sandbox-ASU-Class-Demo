# Sound in Unity: handout for 2D games (DRAFT, not yet tested in a 2D project)

For students whose own game is **2D**. The main handout (STUDENT_HANDOUT.md) is the 3D class demo; read its steps first, because the sounds and most of the scripts are the same.
You are given the audio files, the main **Sandbox** and **DemoScripts** scripts, and the two 2D scripts in **DemoScripts_2D**.
Unity **6000.0.64f1**. Everything here works with volume, placement and plain sound files, so it also works on Web.

## What changes in 2D
Unity's 2D physics is separate from its 3D physics. The 3D trigger and wall-check scripts do not see 2D colliders, so use these instead:
- `TriggerOneShot2D` instead of `TriggerOneShot` (pick ups, win).
- `OcclusionCrossfade2D` instead of `OcclusionCrossfade` (sound muffled behind a wall).
- `FootstepPlayer`, `ClickToStart` and the Audio Source settings are the same.

## Set up first
1. Make your **2D** project (Unity Hub, 2D template). Create `Assets/Scripts`; copy the scripts in: `ClickToStart`, `FootstepPlayer`, `TriggerOneShot2D`, `OcclusionCrossfade2D`. Wait for the spinner; no red errors in the Console.
2. Import the audio into `Assets/Audio`. Select all the files and set **Load Type: Compressed In Memory**, **Apply**.
3. Your **Player** needs the tag **Player**, a **Collider 2D** (for example Box Collider 2D) and a **Rigidbody 2D**. Unity only reports a 2D trigger when one of the two objects has a Rigidbody 2D.
4. One **Audio Listener**, on the camera.

## The big 2D catch: distance
A 2D camera usually sits 10 units back on the Z axis, and a 3D sound counts that depth as distance.
So a sound with Max Distance 14 is only really audible within about 4 units on screen.
Either **add about 10 to every distance** (for example Min 12, Max 24), or **move the Audio Listener from the camera onto the Player** and leave the distances as they are.

## The sounds
Set every loop to **Loop ON**, **Spatial Blend 1**, **Linear Rolloff** if you want it to fade with distance, and list it in **Click To Start** (create an empty object, add **Click To Start**, set **Sources** to the loops).
- **Ambient:** an Audio Source on an empty object, **Spatial Blend 0** (plays everywhere).
- **Danger loop:** an Audio Source on the danger object, **Spatial Blend 1**, **Linear Rolloff**, distances as above. It plays from the start and gets louder as you get close.
- **Footsteps:** on the Player, add **Footstep Player**, set **Clips** size to 6, drag the six footsteps in.
  For a **side-scroller** leave **Ignore Vertical Movement** ON (jumping and falling make no steps). For a **top-down** game turn it **OFF**.
- **Pick up:** the object needs a **Collider 2D with Is Trigger ON**. Add **Trigger One Shot 2D**, drag **Pickup** into **Clip**, leave **Hide After** ticked.
- **Win:** the same, with **Win**, **Hide After** unticked, and **Fade Out** set to the Ambient and Danger sources.
- **Dialogue behind a wall:** two Audio Sources, one with **Dialogue** and one with **DialogueFilter** (both Loop ON, Spatial Blend 1, same distances), both listed in **Click To Start**.
  On the object that holds them add **Occlusion Crossfade 2D**, set **Clean** and **Muffled**, and set **Listener** to your **Player** (not the camera).
  Your wall needs a **Collider 2D** (not a trigger).

## Check
Press Play, click the start button, then: walk (steps), walk toward the danger object (louder), touch the pick up and win objects (sounds), and walk behind the wall (muffled).
Common problems: no pick up sound means a missing Rigidbody 2D or Player tag; no muffling means the wall has no Collider 2D or is set as a trigger.
