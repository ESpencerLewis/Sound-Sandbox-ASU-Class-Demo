using UnityEngine;
using UnityEngine.Audio;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// OPTIONAL. Draws one volume slider per mixer group in the corner of the Game window and applies it to an
    /// exposed Audio Mixer volume (for example MusicVol). Slider values are 0 to 1; the mixer works in decibels,
    /// so this converts with 20 * log10(value). Mixer volume works on Unity Web (effects like reverb do not).
    /// </summary>
    public class MixerVolumeSliders : MonoBehaviour
    {
        [System.Serializable]
        public class Channel
        {
            public string label = "Music";
            [Tooltip("The exact name of the exposed parameter in the mixer, for example MusicVol.")]
            public string exposedParam = "MusicVol";
            [Range(0f, 1f)] public float value = 1f;
        }

        public AudioMixer mixer;
        public Channel[] channels;
        public bool show = true;

        void Start()
        {
            foreach (var c in channels) Apply(c);
        }

        void OnGUI()
        {
            if (!show || channels == null) return;
            GUILayout.BeginArea(new Rect(12, 12, 280, 20 + channels.Length * 28), GUI.skin.box);
            foreach (var c in channels)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(c.label, GUILayout.Width(80));
                float v = GUILayout.HorizontalSlider(c.value, 0f, 1f);
                GUILayout.EndHorizontal();
                if (!Mathf.Approximately(v, c.value))
                {
                    c.value = v;
                    Apply(c);
                }
            }
            GUILayout.EndArea();
        }

        void Apply(Channel c)
        {
            if (mixer == null) return;
            float db = Mathf.Log10(Mathf.Max(0.0001f, c.value)) * 20f;
            if (!mixer.SetFloat(c.exposedParam, db))
                Debug.LogWarning("Mixer has no exposed parameter named '" + c.exposedParam + "'. Check the spelling in Exposed Parameters.");
        }
    }
}
