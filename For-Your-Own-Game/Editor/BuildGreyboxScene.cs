using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Synapsody.Sandbox;

// Menu: Sandbox > Build Greybox Scene
// Builds a silent walk-around room so you can see the layout before any audio is added.
// Floor, walls, a Player you move with W/S/A/D and turn with Q/E, and four coloured cubes:
//   PICK UP (yellow), DANGER (red), DIALOGUE (blue, behind an inner wall), WIN (green).
public static class BuildGreyboxScene
{
    [MenuItem("Sandbox/Build Greybox Scene")]
    public static void Build()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        var root = new GameObject("Room").transform;

        // Floor (30 x 30) and four outer walls
        Block("Floor", new Vector3(0, -0.05f, 0), new Vector3(30, 0.1f, 30), Mat("Floor", new Color(0.55f, 0.55f, 0.55f)), root);
        var wallMat = Mat("Wall", new Color(0.8f, 0.78f, 0.72f));
        Block("Wall North", new Vector3(0, 1.5f, 15), new Vector3(30, 3, 0.3f), wallMat, root);
        Block("Wall South", new Vector3(0, 1.5f, -15), new Vector3(30, 3, 0.3f), wallMat, root);
        Block("Wall East", new Vector3(15, 1.5f, 0), new Vector3(0.3f, 3, 30), wallMat, root);
        Block("Wall West", new Vector3(-15, 1.5f, 0), new Vector3(0.3f, 3, 30), wallMat, root);
        // Inner wall that sits between the walking route and the Dialogue cube (for the occlusion demo)
        Block("Inner Wall (hides Dialogue)", new Vector3(-8, 1.5f, 6), new Vector3(9, 3, 0.3f), wallMat, root);

        // The four sound cubes
        var things = new GameObject("Sound Cubes").transform;
        Cube("PICK UP", new Vector3(-8, 0.5f, -4), new Color(1f, 0.85f, 0.1f), true, things);
        Cube("DANGER", new Vector3(9, 0.5f, 4), new Color(0.9f, 0.1f, 0.1f), false, things);
        Cube("DIALOGUE", new Vector3(-8, 0.5f, 10), new Color(0.2f, 0.4f, 0.95f), false, things);
        Cube("WIN", new Vector3(9, 0.5f, 12), new Color(0.15f, 0.8f, 0.3f), true, things);

        // Where the ambient sound will live (no visual; it is 2D and plays everywhere)
        new GameObject("Ambient (will play everywhere)").transform.position = new Vector3(0, 1, 0);

        // Player: capsule + CharacterController + SandboxMover, camera as a child at head height
        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
        player.AddComponent<CharacterController>();
        player.AddComponent<SandboxMover>();
        player.transform.position = new Vector3(0, 1, -12);
        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.SetParent(player.transform, false);
            cam.transform.localPosition = new Vector3(0, 0.7f, 0);
            cam.transform.localRotation = Quaternion.identity;
        }

        Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Greybox.unity");
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Scenes/Greybox.unity", true) };
        Debug.Log("Greybox scene built and saved as Assets/Scenes/Greybox.unity. Press Play. W/S move, A/D strafe, Q/E turn.");
    }

    static GameObject Block(string name, Vector3 pos, Vector3 scale, Material m, Transform parent)
    {
        var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = name;
        g.transform.SetParent(parent, false);
        g.transform.position = pos;
        g.transform.localScale = scale;
        g.GetComponent<Renderer>().sharedMaterial = m;
        return g;
    }

    static void Cube(string label, Vector3 pos, Color c, bool isTrigger, Transform parent)
    {
        var g = Block(label, pos, Vector3.one, Mat(label, c), parent);
        g.GetComponent<BoxCollider>().isTrigger = isTrigger;

        var t = new GameObject(label + " label");
        t.transform.SetParent(g.transform, false);
        t.transform.localPosition = new Vector3(0, 1.2f, 0);
        var tm = t.AddComponent<TextMesh>();
        tm.text = label;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.characterSize = 0.12f;
        tm.fontSize = 60;
        tm.color = Color.white;
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font != null)
        {
            tm.font = font;
            t.GetComponent<MeshRenderer>().sharedMaterial = font.material;
        }
    }

    static Material Mat(string name, Color c)
    {
        Directory.CreateDirectory("Assets/GreyboxMaterials");
        string path = "Assets/GreyboxMaterials/" + name + ".mat";
        var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null) { existing.color = c; return existing; }
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        var m = new Material(shader) { color = c };
        AssetDatabase.CreateAsset(m, path);
        return m;
    }
}
