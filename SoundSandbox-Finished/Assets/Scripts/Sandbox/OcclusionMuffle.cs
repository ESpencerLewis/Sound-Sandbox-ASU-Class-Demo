using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Put this on a sound emitter. If a wall is between the emitter and the listener, the sound gets quieter
    /// (works everywhere, including Unity Web) and, if an AudioLowPassFilter is present, also darker
    /// (Editor and desktop only; Unity Web ignores it).
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class OcclusionMuffle : MonoBehaviour
    {
        public Transform listener;
        public LayerMask blockers = ~0;
        [Range(0f, 1f)] public float blockedVolumeMultiplier = 0.35f;
        public float openCutoff = 22000f;
        public float blockedCutoff = 900f;
        public float seconds = 0.25f;

        AudioSource src;
        AudioLowPassFilter lp;
        float baseVolume;
        float amount; // 0 = open, 1 = blocked

        void Awake()
        {
            src = GetComponent<AudioSource>();
            lp = GetComponent<AudioLowPassFilter>();
            baseVolume = src.volume;
            if (listener == null && Camera.main != null) listener = Camera.main.transform;
        }

        void Update()
        {
            if (listener == null) return;
            Vector3 dir = listener.position - transform.position;
            bool blocked = Physics.Raycast(transform.position, dir.normalized, out var hit, dir.magnitude, blockers)
                           && hit.transform != listener && !hit.transform.IsChildOf(listener);
            float k = 1f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.01f, seconds));
            amount = Mathf.Lerp(amount, blocked ? 1f : 0f, k);
            src.volume = Mathf.Lerp(baseVolume, baseVolume * blockedVolumeMultiplier, amount);
            if (lp != null) lp.cutoffFrequency = Mathf.Lerp(openCutoff, blockedCutoff, amount);
        }
    }
}
