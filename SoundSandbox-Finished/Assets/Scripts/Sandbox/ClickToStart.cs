using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Browsers block audio until the user clicks the page. Turn Play On Awake OFF on your AudioSources, list them
    /// here, and this shows a Click to start button; the sources start together when it's pressed.
    /// Also fine on desktop, where it acts as a simple start screen.
    /// </summary>
    public class ClickToStart : MonoBehaviour
    {
        public AudioSource[] sources;
        public string label = "Click to start";
        bool started;

        void OnGUI()
        {
            if (started) return;
            float w = 260, h = 70;
            if (GUI.Button(new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h), label))
            {
                started = true;
                foreach (var s in sources) if (s != null) s.Play();
            }
        }
    }
}
