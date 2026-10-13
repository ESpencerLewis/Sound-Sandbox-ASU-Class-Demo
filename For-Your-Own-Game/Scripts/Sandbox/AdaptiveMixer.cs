using UnityEngine;
using UnityEngine.Audio;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// One intensity value (0 to 1) drives several mixer groups at once. Each layer has an AnimationCurve that
    /// says how loud it is at each intensity, so students can SEE and edit the arrangement logic in the inspector.
    /// Works on Unity Web too (mixer volume is the one mixer property Web supports). Setup: on the Audio Mixer, right-click each group's Volume, choose "Expose to script", then rename the
    /// exposed parameter in the mixer's Exposed Parameters list to match exposedParam below.
    /// </summary>
    public class AdaptiveMixer : MonoBehaviour
    {
        [System.Serializable]
        public class Layer
        {
            public string name = "Pad";
            [Tooltip("Exact name of the exposed volume parameter on the mixer, for example PadVol")]
            public string exposedParam = "PadVol";
            [Tooltip("X: intensity 0 to 1. Y: loudness 0 (silent) to 1 (full).")]
            public AnimationCurve gainByIntensity = AnimationCurve.Linear(0, 0, 1, 1);
        }

        public AudioMixer mixer;
        public Layer[] layers;
        [Range(0f, 1f)] public float intensity = 0.2f;
        [Tooltip("Seconds the mix takes to follow the intensity.")]
        public float smoothing = 0.6f;
        public bool showSlider = true;

        float current;

        void Update()
        {
            if (mixer == null) return;
            float k = 1f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.01f, smoothing));
            current = Mathf.Lerp(current, intensity, k);
            foreach (var l in layers)
            {
                float lin = Mathf.Clamp(l.gainByIntensity.Evaluate(current), 0.0001f, 1f);
                mixer.SetFloat(l.exposedParam, Mathf.Log10(lin) * 20f); // linear gain to decibels
            }
        }

        // Public so other scripts (and the student exercise) can change it.
        public void SetIntensity(float value) { intensity = Mathf.Clamp01(value); }
        public void AddIntensity(float delta) { SetIntensity(intensity + delta); }

        void OnGUI()
        {
            if (!showSlider) return;
            GUILayout.BeginArea(new Rect(12, 12, 320, 70), GUI.skin.box);
            GUILayout.Label($"Intensity {intensity:0.00}");
            intensity = GUILayout.HorizontalSlider(intensity, 0f, 1f);
            GUILayout.EndArea();
        }
    }
}
