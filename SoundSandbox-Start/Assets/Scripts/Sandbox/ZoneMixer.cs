using UnityEngine;
using UnityEngine.Audio;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// WebGL-safe zone mixing. On Unity Web the mixer's VOLUME is the only property that works (no reverb, no
    /// low-pass, no other effects), so zones change a volume through script instead of relying on snapshot effects.
    /// Put one on a manager object. Expose the volume of a parent group (for example "Room") as RoomVol.
    /// ZoneTrigger tells this component what gain to move to; it eases there and sets the mixer.
    /// </summary>
    public class ZoneMixer : MonoBehaviour
    {
        public AudioMixer mixer;
        public string exposedParam = "RoomVol";
        [Range(0f, 1f)] public float startGain = 1f;
        public float seconds = 1.2f;

        float target, current;

        void Awake() { target = current = startGain; }

        public void SetTarget(float gain, float transitionSeconds = -1f)
        {
            target = Mathf.Clamp01(gain);
            if (transitionSeconds > 0f) seconds = transitionSeconds;
        }

        void Update()
        {
            if (mixer == null) return;
            float k = 1f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.01f, seconds * 0.4f));
            current = Mathf.Lerp(current, target, k);
            mixer.SetFloat(exposedParam, Mathf.Log10(Mathf.Max(0.0001f, current)) * 20f);
        }
    }
}
