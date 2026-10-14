# Sound in Unity: an adaptive-audio sandbox

A guest lecture for ASU AME 494 (Wed Oct 14, 2026), by Elle Spencer Lewis.
Unity **6000.0.64f1**. Built-in Unity audio only, so it also works in **Web** builds (where only volume works: no reverb or filters).

## What is here
| Folder | What it is |
|---|---|
| `SoundSandbox-Start` | A ready-made Unity project: the room with four coloured cubes, the scripts, **no audio**. Start here in class. |
| `SoundSandbox-Finished` | The completed demo, with all the sounds working. Open it to compare with yours. |
| `For-Your-Own-Game` | Just the scripts and the audio files, to copy into your own project. |
| `STUDENT_HANDOUT.md` | The step-by-step for the 3D demo. |
| `SOLUTIONS_AND_TROUBLESHOOTING.md` | What each object should look like, and fixes for common problems. |
| `STUDENT_HANDOUT_2D.md` | The same ideas for a 2D game (draft, not yet tested in a 2D project). |

## How to open a project
1. Install **Unity 6000.0.64f1** with **Web Build Support** (Unity Hub, Installs, Install Editor, Archive).
2. Download this repository (green **Code** button, **Download ZIP**) and unzip it.
3. In Unity Hub click **Add**, **Add project from disk**, and choose `SoundSandbox-Start` (or `SoundSandbox-Finished`).
   The first open takes a few minutes while Unity rebuilds its cache. A message about the Editor version means you are not on 6000.0.64f1.
4. In the Project window, open `Assets/Scenes/Greybox`, press **Play**, click in the Game window. W/S move, A/D sideways, Q/E turn.

If the room is missing, use the menu **Sandbox, Build Greybox Scene**.

## Licenses
Scripts and project files: MIT (`LICENSE`). Audio: CC BY 4.0 (`LICENSE-AUDIO.md`), credit Elle Spencer Lewis.
