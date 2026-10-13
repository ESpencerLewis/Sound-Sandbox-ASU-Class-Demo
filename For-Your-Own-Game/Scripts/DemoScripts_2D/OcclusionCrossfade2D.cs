using System.Collections.Generic;
using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// The 2D version of OcclusionCrossfade, for 2D games (it uses Unity's 2D physics and ignores the camera's depth).
    /// Two copies of the same sound play together: a normal one ("Clean") and one darkened in your audio editor
    /// ("Muffled"). When a Collider 2D (a wall) is between this object and the listener, it crossfades from Clean
    /// to Muffled. Both sources must start in the same moment and be the same length.
    /// Set "Listener" to your Player (not the camera) if your camera sits back on the Z axis.
    /// Works on Unity Web: it only changes volume.
    /// </summary>
    public class OcclusionCrossfade2D : MonoBehaviour
    {
        public AudioSource clean;
        public AudioSource muffled;
        public Transform listener;
        public LayerMask blockers = ~0;
        [Range(0f, 1f)] public float blockedLevel = 0.8f;
        public float seconds = 0.3f;

        float cleanBase, muffledBase, amount; // amount: 0 = open, 1 = blocked
        readonly List<RaycastHit2D> hits = new List<RaycastHit2D>();
        ContactFilter2D filter;

        void Awake()
        {
            if (clean != null) cleanBase = clean.volume;
            if (muffled != null) muffledBase = muffled.volume;
            filter = new ContactFilter2D { useTriggers = false, useLayerMask = true, layerMask = blockers };
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

            float level = Mathf.Lerp(1f, blockedLevel, amount);
            clean.volume = cleanBase * Mathf.Cos(amount * Mathf.PI * 0.5f) * level;
            muffled.volume = muffledBase * Mathf.Sin(amount * Mathf.PI * 0.5f) * level;
        }

        bool IsBlocked()
        {
            Vector2 from = transform.position;
            Vector2 to = listener.position;
            Vector2 dir = to - from;
            float dist = dir.magnitude;
            if (dist < 0.01f) return false;
            Physics2D.Raycast(from, dir / dist, filter, hits, dist);
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
