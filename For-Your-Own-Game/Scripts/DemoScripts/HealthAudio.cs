using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Turns health into sound. As health falls:
    ///  - a looping heartbeat gets louder and faster (volume and pitch follow the curves below), and
    ///  - the other sounds you list are ducked (turned down), because a low-pass filter does not work on Web.
    /// Works on Unity Web: it only changes volume and pitch.
    /// Heartbeat: an Audio Source with a looping heartbeat clip, Loop ON, Spatial Blend 0. List it in Click To Start.
    /// </summary>
    public class HealthAudio : MonoBehaviour
    {
        public Health health;
        public AudioSource heartbeat;
        [Tooltip("X: damage (0 = full health, 1 = no health). Y: heartbeat volume.")]
        public AnimationCurve heartbeatVolume = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.3f, 0f), new Keyframe(1f, 1f));
        public float highestPitch = 1.4f;

        [Header("Duck the other sounds")]
        public AudioSource[] duckSources;
        [Tooltip("Dialogue pairs controlled by Occlusion Crossfade (it manages their volumes itself).")]
        public OcclusionCrossfade[] duckOcclusion;
        [Range(0f, 1f)] public float lowestLevel = 0.3f;

        float[] baseVolumes;
        float lastLevel = -1f;

        void Start()
        {
            baseVolumes = new float[duckSources.Length];
            for (int i = 0; i < duckSources.Length; i++)
                if (duckSources[i] != null) baseVolumes[i] = duckSources[i].volume;
        }

        void Update()
        {
            if (health == null) return;
            float damage = 1f - health.value;

            if (heartbeat != null)
            {
                heartbeat.volume = Mathf.Clamp01(heartbeatVolume.Evaluate(damage));
                heartbeat.pitch = Mathf.Lerp(1f, highestPitch, damage);
            }

            // Only write the ducked volumes when health changes, so fades from other scripts (Win) still work
            float level = Mathf.Lerp(1f, lowestLevel, damage);
            if (Mathf.Approximately(level, lastLevel)) return;
            lastLevel = level;
            for (int i = 0; i < duckSources.Length; i++)
                if (duckSources[i] != null) duckSources[i].volume = baseVolumes[i] * level;
            foreach (var o in duckOcclusion)
                if (o != null) o.masterLevel = level;
        }
    }
}
