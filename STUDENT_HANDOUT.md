# Sound in Unity: student handout (DRAFT, grows as the demo is built)

Unity **6000.0.64f1** (the lab version). Built-in Unity audio only. The game builds to **Web**, where only volume works
(no reverb or filters), so every effect here is done with volume, placement and pre-made sound files.
You are given: the **Sandbox scripts**, the **audio files**, and the **Greybox builder** script. Follow the steps in order.

## What you are building
A walk-around room with four coloured cubes, each with its own sound:
- **Ambient**: a loop that starts with the game and plays everywhere.
- **PICK UP** (yellow): a short sound when you walk into it.
- **DANGER** (red): a loop that gets louder as you walk toward it.
- **DIALOGUE** (blue, behind a wall): a loop that gets louder as you approach, and gets muffled when the wall is between you and it.
- **WIN** (green): a sound when you walk into it.
- **Footsteps**: six different steps while you walk.

Controls: **W / S** forward and back, **A / D** strafe (move sideways), **Q / E** turn. Click the Game window first.

---

## Step 1. Make the project
1. Open **Unity Hub**, click **New project**.
2. Set **Editor version** to **6000.0.64f1**. Choose the plain **3D** template.
3. Name it **SoundSandbox**. Leave **Unity Version Control** off (not needed).
4. Click **Create project** and wait for it to open.

## Step 2. Add the scripts
1. In the **Project** window, click **Assets**, right-click, **Create, Folder**, name it `Scripts`.
2. In Finder, drag the provided **Sandbox** folder into `Assets/Scripts`.
3. Wait for the spinner at the bottom right to stop.
4. Open the **Console** (Window, General, Console). There should be **no red errors**.

## Step 3. Add the greybox builder, then build the room
1. In the Project window, create a folder `Assets/Editor`. Put the provided `BuildGreyboxScene.cs` in it.
2. Wait for the spinner. A new menu called **Sandbox** appears at the top of the screen.
3. Click **Sandbox, Build Greybox Scene**. (If it asks about saving the open scene, click either.)
4. You should see a gray room with four coloured, labelled cubes, and a Player capsule. The scene is saved as `Assets/Scenes/Greybox.unity`.
5. Press **Play**, click in the Game window, and walk around. This is the layout the sounds will go into. No sound yet.

## Step 4. Bring in the audio
1. In the Project window, right-click **Assets**, **Create, Folder**, name it `Audio`.
2. In Finder, select the 12 provided audio files and drag them onto the **Audio** folder:
   `AmbientLoop`, `DangerLoop`, `Dialogue`, `DialogueFilter`, `Pickup`, `Win`, and `Footsteps1` to `Footsteps6`.
3. Click the **Audio** folder, click one file, press **Command-A** (Windows: Ctrl-A) to select all 12.
4. In the **Inspector**, set **Load Type** to **Compressed In Memory**, then click **Apply**.
   This avoids a silent-mode problem on iPhones and works on Web.

## Step 5. The Ambient sound
1. Open the **Greybox** scene (double-click it in `Assets/Scenes`).
2. In the **Hierarchy**, click **Ambient (will play everywhere)**.
3. In the Inspector, click **Add Component**, search for **Audio Source**, click it.
4. Drag **AmbientLoop** from the Audio folder into the **Audio Resource** slot.
5. Set: **Play On Awake ON** (it starts when you press Play), **Loop ON**,
   **Spatial Blend 0** (2D: it plays the same everywhere), **Volume 0.6**.
6. Press **Play**. The ambient loop starts on its own and sounds the same everywhere you walk.
   (Note: in a Web build, browsers decide when sound may start. It may start automatically, or after the player's first click or key press,
   or only when they click the button in Step 6. It depends on the browser, its version and the user's settings, which is why Step 6 adds the button.)

## Step 6. A Click to start button (a safeguard for the browser)
Play On Awake works in the Editor, but a browser blocks sound until the page is clicked. This button makes sure the sound starts.
1. In the **Hierarchy**, right-click an empty area, **Create Empty**, and name it **LevelSound**.
2. With **LevelSound** selected, **Add Component**, search for **Click To Start**, click it.
3. Open **Sources**, set **Size** to **1**, press Return.
4. Drag **Ambient (will play everywhere)** from the Hierarchy onto **Element 0**.
5. Press **Play**. The ambient plays, and a **Click to start** button shows in the middle of the Game window.
   Click it (the sound restarts once from the beginning). In the browser, this click is what lets the sound play.

## Step 7. The Danger loop (gets louder as you walk toward it)
The loop plays from the start, silently, because it is a 3D sound that fades with distance. As you walk toward the red cube it gets louder. No script is needed.
1. In the **Hierarchy**, expand **Sound Cubes** and click **DANGER**. Right-click it, **Create Empty**, name the child **Danger Sound**.
2. Click **Danger Sound**, **Add Component**, **Audio Source**. Drag **DangerLoop** into the **Audio Resource** slot.
3. Set **Play On Awake ON**, **Loop ON**, **Spatial Blend 1** (3D).
4. Open **3D Sound Settings**: **Volume Rolloff: Linear Rolloff** (the default never fades fully to silent), **Min Distance 2**, **Max Distance 14**.
5. Click **LevelSound** in the Hierarchy. In **Click To Start**, set **Sources** to size **2** and drag **Danger Sound** into **Element 1**.
   Now the button restarts both loops together, so they stay in time.
6. Press **Play**, click the button, and walk toward the red cube: silent far away, louder as you get close.

## Step 8. The Pick Up sound
In Unity a pickup is a **trigger**, not a collision. A trigger is a collider you walk *through*; a script plays the sound when you enter it.
A normal solid collision would stop you at the cube.
1. Put the provided `TriggerOneShot.cs` in `Assets/Scripts`. Wait for the spinner; no red errors in the Console.
2. In the **Hierarchy**, expand **Sound Cubes** and click **PICK UP**.
3. In the Inspector, check that the **Box Collider** has **Is Trigger** ticked.
4. **Add Component**, **Trigger One Shot**. Drag **Pickup** into the **Clip** slot. Leave **Hide After** ticked.
5. Press **Play**, click the start button, and walk into the yellow cube: you hear the sound once and the cube disappears.

## Step 9. The Win sound
Same idea as the Pick Up: a trigger. This one stays visible, and fades the two loops out.
1. In the **Hierarchy**, expand **Sound Cubes** and click **WIN**. Check its **Box Collider** has **Is Trigger** ticked.
2. **Add Component**, **Trigger One Shot**. Drag **Win** into the **Clip** slot. **Untick Hide After.**
3. Open **Fade Out**, set **Size** to **2**. Drag **Ambient (will play everywhere)** into **Element 0** and **Danger Sound** into **Element 1**. Leave **Fade Seconds** at 2.
4. Press **Play**, click the start button, and walk into the green cube: the win sound plays once and the two loops fade out and stop.

## Step 10. Footsteps
1. Put the provided `FootstepPlayer.cs` in `Assets/Scripts`. Wait for the spinner; no red errors in the Console.
2. In the **Hierarchy**, click **Player**. **Add Component**, **Footstep Player** (it adds its own Audio Source).
3. Open **Clips**, set **Size** to **6**, and drag **Footsteps1** to **Footsteps6** into the slots (you can select all six and drag them onto the word **Clips**).
4. Leave **Step Distance 1.8**, **Pitch Variation 0.05**, **Volume 0.8**. A bigger Step Distance means slower steps.
5. Press **Play** and walk: a step about every half second, never the same clip twice in a row, each slightly different in pitch. Turning on the spot makes no steps.

## Step 11. The Dialogue, muffled by a wall (occlusion without a filter)
A real low-pass filter does not work on Web. Instead, two copies of the same sound play together: the normal **Dialogue**, and **DialogueFilter**,
a copy darkened in an audio editor. A script crossfades between them when a wall is in the way. It only changes volume, so it works on Web.
1. Put the provided `OcclusionCrossfade.cs` in `Assets/Scripts`. Wait for the spinner; no red errors.
2. In the **Hierarchy**, expand **Sound Cubes**. Right-click **DIALOGUE**, **Create Empty**, name the child **Dialogue Clean**. Repeat and name the second child **Dialogue Muffled**.
3. On **Dialogue Clean**: **Add Component**, **Audio Source**, drag **Dialogue** into **Audio Resource**.
   On **Dialogue Muffled**: **Add Component**, **Audio Source**, drag **DialogueFilter** into **Audio Resource**.
4. On **both** sources: **Play On Awake ON**, **Loop ON**, **Spatial Blend 1**. In **3D Sound Settings**: **Linear Rolloff**, **Min Distance 2**, **Max Distance 14**.
5. Click **LevelSound**. In **Click To Start**, set **Sources** to size **4**. Drag **Dialogue Clean** into Element 2 and **Dialogue Muffled** into Element 3, so both start in the same moment.
6. Click **DIALOGUE** itself. **Add Component**, **Occlusion Crossfade**. Drag **Dialogue Clean** into **Clean** and **Dialogue Muffled** into **Muffled**.
7. Press **Play**, click the button, and approach the blue cube from the south, where the inner wall is in the way: you hear the muffled version.
   Walk around the end of the wall to get a clear line, and it crossfades to the clean version.

## Using this in your own game
The sounds are ordinary WAV files and the scripts only use AudioSources, so they can go into your own **3D** project: copy the scripts into your `Assets/Scripts`,
import the audio with **Load Type: Compressed In Memory**, wait for the Console to show no red errors, then repeat the steps above on your own objects.
Check these first:
1. **One Audio Listener only**, on your camera. The camera should be tagged **MainCamera** (or drag your camera into the **Listener** slot of the occlusion script).
2. **Distances are in your game's world units.** Min 2 and Max 14 suit a human-sized 3D scene. If your world is bigger or smaller, scale them.
3. **For triggers** (pick ups, win): your Player needs the tag **Player** and a collider with a **Rigidbody** (or a **CharacterController**). Unity only reports a trigger when one of the two objects has a Rigidbody.
4. **Footsteps** work with any controller, because they only watch how far the Player moves.
5. **Third-person camera:** if the camera is not a child of the Player, set the **Listener** of the occlusion script to your Player so its body is not counted as a wall.
6. If your game already uses FMOD, Wwise or its own audio manager, these scripts will play alongside it, not inside it.

**Working on a 2D game?** Use the separate **STUDENT_HANDOUT_2D.md** and the scripts in the **DemoScripts_2D** folder.

<!-- NEXT STEPS TO WRITE (for Elle, delete before handing out):
 7b (pick up, next): 3D sound, Linear rolloff, distance fade
 7 Pick up: trigger + one-shot script
 8 Win: trigger + one-shot (+ fade the loops out)
 9 Footsteps script
 10 Dialogue + DialogueFilter: two sources, crossfade script (occlusion without a filter)
 11 Mixer groups + exposed volumes (RoomVol etc., exact names)
 12 Web build and test
 Plus: Common problems box, credits. -->
