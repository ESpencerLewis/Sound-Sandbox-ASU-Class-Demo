# Sound in Unity: student handout (DRAFT, grows as the demo is built)

**Try the finished demo in your browser first:** https://synapsody.itch.io/sound-sandbox (click the game, then **Click to start**).

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

## Step 12. Build for the Web and play it in a browser
A Web build runs as a page in the browser, with no Unity needed. Remember that on Web only **volume** works (no reverb or filters), and the browser may wait for a click before it plays sound.
1. Save the scene (**Command-S**, Windows: Ctrl-S).
2. Go to **File, Build Profiles**. Click **Web** in the platform list, then **Switch Platform**. (If Web is missing, the **Web Build Support** module is not installed: add it in Unity Hub, Installs, the gear icon, Add modules.)
3. Check that **Greybox** is in the **Scenes** list and **ticked**. If it is not, click **Add Open Scenes**.
4. Click **Build And Run**. Make a new folder called `WebBuild` **outside** your project folder, and choose it. The first build can take several minutes.
5. When it finishes, Unity opens the game in your browser. Click the game, then click **Click to start**. Walk with W, S, A, D, Q and E, and check each sound:
   ambient, footsteps, Danger getting louder near the red cube, Pick Up, Win, and the Dialogue muffling behind the wall.
   Write down anything that sounds different from the Editor. That is the lesson of this step.
6. **To share it on itch.io:** zip the **contents** of the `WebBuild` folder so `index.html` is at the top of the zip, not inside another folder. On itch.io choose **New project**, kind **HTML**, upload the zip, and tick **This file will be played in the browser**.
   If it will not load there, rebuild with **Compression Format: Disabled** (Edit, Project Settings, Player, Publishing Settings) and zip it again.

## Step 13 (optional). A mixer for your volumes
A **mixer** groups your sounds so one fader controls many sources, and lets a script or a volume menu change them while the game runs.
Mixer **volume works on Web**; mixer effects such as reverb and low-pass do not.
1. In the Project window, right-click `Assets`, **Create, Audio, Audio Mixer**. Name it **GameMixer**. Double-click it to open the Audio Mixer window.
2. In the **Groups** section, click the **+**, **Add child group to Master**. Make three: **Music**, **Dialogue**, **Effects**.
3. Route each Audio Source to a group: select the source, and drag the group into its **Output** slot (or pick it from the Output menu).
   Ambient and Danger Sound go to **Music**. Dialogue Clean and Dialogue Muffled go to **Dialogue**. Footsteps, Pick Up and Win go to **Effects**.
   The Footstep Player, Pick Up and Win scripts add their own Audio Source while the game runs. To route them, first add an **Audio Source** component yourself
   (untick **Play On Awake**) on the Player, PICK UP and WIN, and set its Output.
4. **Expose each volume:** click a group, right-click **Volume** in the Inspector, **Expose 'Volume (of Music)' to script**. Do this for all three groups.
5. At the top right of the mixer window click **Exposed Parameters**, and rename them **exactly**: `MusicVol`, `DialogueVol`, `EffectsVol` (no spaces).
6. Put the provided `MixerVolumeSliders.cs` in `Assets/Scripts`. Create an empty object called **Sliders**, add **Mixer Volume Sliders**,
   drag **GameMixer** into **Mixer**, set **Channels** to size **3**, and fill in each one: Label `Music` / Param `MusicVol`, Label `Dialogue` / Param `DialogueVol`, Label `Effects` / Param `EffectsVol`.
7. Press **Play**. Three sliders appear at the top left of the Game window and change each group's volume.
Mixers work in decibels, so the script converts a 0 to 1 slider value with `20 * log10(value)`.

<!-- ELLE: Step 13 is written from how Unity's mixer works, but not yet tested in a Web build. Test before covering it in class. -->

## Bonus exercise (optional): make the music respond to a zone
For students who finish early. In the `Sandbox` folder, `StudentExercise.cs` has three **TODOs** for you to fill in, so that standing inside a "Tense" zone makes an **intensity** value rise, and leaving it lets it fall back.
Its finished version is `StudentExercise_Solution.txt` in the same folder: look only after you have tried.
What you need first:
- The **mixer from Step 13**, with a group for the layer you want to bring in, and its volume exposed (for example `TensionVol`).
- An extra **looping layer** of your own, the same length and tempo as the Ambient loop, set as a 2D sound (**Spatial Blend 0**) and routed to that group.
- An empty object with the provided `AdaptiveMixer.cs`. Set **Mixer**, add one **Layer** with your exposed name, and edit its curve so the layer is silent at low intensity and loud at high intensity. Press Play and drag the slider to hear it.
- A cube named **Tense** with **Is Trigger** ticked and the `StudentExercise` script. Drag the AdaptiveMixer object into its **Adaptive** slot.
Then complete the three TODOs, press Play and walk into the cube.

## Bonus: health, a heartbeat, and ducking the other sounds
Tie sound to a character's health. As health falls, a heartbeat gets louder and faster, and the other sounds get quieter. Standing near the DANGER cube drains health, and the PICK UP cube heals a little.
It only changes **volume and pitch**, so it works on Web. (A low-pass filter would muffle the other sounds more realistically, but it does not work on Web, so here they are turned down instead.)
A looping heartbeat is provided: `Heartbeat.wav` in `For-Your-Own-Game/Audio` (about 40 beats a minute, four beats, loops cleanly). Import it with **Compressed In Memory**.
1. Put the provided `Health.cs`, `HealthAudio.cs`, `HealthDrainNear.cs` and `HealOnTrigger.cs` in `Assets/Scripts`. Replace `OcclusionCrossfade.cs` with the updated copy (it has a new **Master Level** setting). Wait for the spinner; no red errors.
2. In the **Hierarchy**, **Create Empty**, name it **HealthSystem**, and **Add Component, Health**. A health slider appears at the top right when you press Play.
3. Right-click **HealthSystem**, **Create Empty**, name the child **Heartbeat Sound**. **Add Component, Audio Source**: drag **Heartbeat** into **Audio Resource**, **Play On Awake ON**, **Loop ON**, **Spatial Blend 0**, **Volume 1**.
4. Click **LevelSound**. In **Click To Start**, make **Sources** one bigger and drag **Heartbeat Sound** into the new slot.
5. Click **HealthSystem**, **Add Component, Health Audio**. Fill in: **Health** = HealthSystem, **Heartbeat** = Heartbeat Sound.
   Under **Duck Sources** set **Size** to **2** and drag in **Ambient (will play everywhere)** and **Danger Sound**.
   Under **Duck Occlusion** set **Size** to **1** and drag in the **DIALOGUE** cube. Leave **Lowest Level** at 0.3 and **Highest Pitch** at 2.
6. Click the **DANGER** cube. **Add Component, Health Drain Near**. Drag **HealthSystem** into **Health**. Leave **Radius 6** and **Drain Per Second 0.1**.
7. Click the **PICK UP** cube. **Add Component, Heal On Trigger**. Drag **HealthSystem** into **Health**. Leave **Amount 0.3**.
8. Press **Play**, click **Click to start**, and **drag the Health slider down** to hear it: the heartbeat comes in, speeds up, and the ambient, danger and dialogue sounds get quieter.
   Then walk up to the red cube: health drains. Walk away and it slowly recovers, and the yellow cube heals you when you touch it.
9. **Tune it:** edit the **Heartbeat Volume** curve (X is damage from 0 to 1, Y is volume), the pitch, and **Lowest Level** until it feels right.

<!-- ELLE: this section is written but not yet tested in Play mode or a Web build. The scripts compile in 6000.0.64f1. -->

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

## Free music and sounds for your own game
**Check the license of every sound you use**, and keep a note of where each one came from.
- **CC0**: free to use with no credit required.
- **CC BY**: free to use, but you must credit the creator.
- **CC BY-NC**: free, but not in anything you sell.

**Sound libraries**
- [Freesound](https://freesound.org): a huge library of sound effects and recordings. A free account is needed to download. Each sound has its own license (CC0, CC BY or CC BY-NC), so read it on the sound's page.
- [Kenney audio packs](https://kenney.nl/assets/category:Audio): impact, interface, sci-fi, RPG and digital sounds. The Impact Sounds pack is listed as CC0 and free; check each pack's page.
- [OpenGameArt](https://opengameart.org): free music and sounds, with the license listed on each page. For example, [Short Loops Background Music Pack](https://opengameart.org/content/short-loops-background-music-pack) (CC0, 1.5 MB) and [Residual World game audio samples](https://opengameart.org/content/residual-world-4-game-audio-samples-cc0) (CC0, 5 MB).

**Music with layers (stems) that play together, good for adaptive music**
- [Lurking Layers Music Pack](https://imperial-dawn-audio.itch.io/lurking-layers): CC0, three tracks with five or more layers each at one shared tempo (571 MB).
- [Eerie Space Music](https://foozlecc.itch.io/eerie-space-music): CC0, two songs with stems (449 MB).
- [Free 8-bit Music Asset Pack](https://danzmacabre.itch.io/free-8-bit-music-asset-pack): CC BY 4.0, three songs with stems (110 MB).
- More free music on itch.io: [itch.io free game music assets](https://itch.io/game-assets/free/tag-music).

**Learning more**
- [FMOD For Unity - Integration Tutorial](https://www.youtube.com/watch?v=9ehp1aqtDuI) (video): the next step, adding FMOD to a Unity project.

**Tips for loops:** loops that play together must be the **same length** and start on a bar line, so they stay in time. Export them as WAV, import with **Compressed In Memory**, and listen to the loop point.

<!-- NEXT STEPS TO WRITE (for Elle, delete before handing out):
 7b (pick up, next): 3D sound, Linear rolloff, distance fade
 7 Pick up: trigger + one-shot script
 8 Win: trigger + one-shot (+ fade the loops out)
 9 Footsteps script
 10 Dialogue + DialogueFilter: two sources, crossfade script (occlusion without a filter)
 11 Mixer groups + exposed volumes (RoomVol etc., exact names)
 12 Web build and test
 Plus: Common problems box, credits. -->
