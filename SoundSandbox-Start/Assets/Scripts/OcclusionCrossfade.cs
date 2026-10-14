using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Occlusion without a filter (a filter does not work on Unity Web). Two copies of the same sound play together:
    /// a normal one ("Clean") and one that was darkened in your audio editor ("Muffled"). When a wall is between this
    /// object and the listener, the sound crossfades from Clean to Muffled, and gets a little quieter overall.
    /// Put this on the cube, with the two AudioSources on children of the cube. Both sources must start in the
    /// same moment (list them both in Click To Start) and be the same length.
    /// Works on Unity Web: it only changes volume.
    /// </summary>
    public class OcclusionCrossfade : MonoBehaviour
    {
        public AudioSource clean;
        public AudioSource muffled;
        public Transform listener;
        public LayerMask blockers = ~0;
        [Range(0f, 1f)] public float blockedLevel = 0.8f;
        public float seconds = 0.3f;
        [Tooltip("Overall level for both copies. Other scripts (for example Health Audio) can lower it.")]
        [Range(0f, 1f)] public float masterLevel = 1f;

        float cleanBase, muffledBase, amount; // amount: 0 = open, 1 = blocked

        void Awake()
        {
            if (clean != null) cleanBase = clean.volume;
            if (muffled != null) muffledBase = muffled.volume;
        }

        void Update()
        {
            if (clean == null || muffled == null) return;
            if (listener == null)
            {
                if (Camera.main == null) return;
                listener = Camera.main.transform;
            }

            bool blocked = IsBlocked();
            float k = 1f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.01f, seconds));
            amount = Mathf.Lerp(amount, blocked ? 1f : 0f, k);

            // Equal-power crossfade so the level does not dip in the middle
            float level = Mathf.Lerp(1f, blockedLevel, amount) * masterLevel;
            clean.volume = cleanBase * Mathf.Cos(amount * Mathf.PI * 0.5f) * level;
            muffled.volume = muffledBase * Mathf.Sin(amount * Mathf.PI * 0.5f) * level;
        }

        bool IsBlocked()
        {
            Vector3 from = transform.position;
            Vector3 to = listener.position;
            Vector3 dir = to - from;
            float dist = dir.magnitude;
            if (dist < 0.01f) return false;
            // Ignore trigger zones, the player's own body, and this object's own colliders
            var hits = Physics.RaycastAll(from, dir / dist, dist, blockers, QueryTriggerInteraction.Ignore);
            foreach (var h in hits)
            {
                var t = h.collider.transform;
                if (t.IsChildOf(listener.root) || t.IsChildOf(transform)) continue;
                return true;
            }
            return false;
        }
    }
}
