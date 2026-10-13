using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Put this on the Player (any controller: it only watches how far the object moves). Plays a random footstep
    /// clip every "Step Distance" metres walked. Turning on the spot makes no steps.
    /// Works on Unity Web: plain one-shot clips.
    /// </summary>
    public class FootstepPlayer : MonoBehaviour
    {
        public AudioClip[] clips;
        public float stepDistance = 1.8f;
        [Range(0f, 0.2f)] public float pitchVariation = 0.05f;
        [Range(0f, 1f)] public float volume = 0.8f;
        [Tooltip("ON for 3D games and side-scrolling 2D games (jumping and falling make no steps). Turn OFF for top-down 2D games.")]
        public bool ignoreVerticalMovement = true;

        AudioSource src;
        Vector3 last;
        float travelled;
        int lastIndex = -1;

        void Awake()
        {
            src = GetComponent<AudioSource>();
            if (src == null) src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f; // 2D: the player's own footsteps
            last = transform.position;
            travelled = stepDistance; // so the first step sounds as soon as you move
        }

        void Update()
        {
            Vector3 p = transform.position;
            Vector3 d = p - last;
            if (ignoreVerticalMovement) d.y = 0f;
            last = p;
            if (d.sqrMagnitude < 1e-6f) { travelled = stepDistance; return; }
            travelled += d.magnitude;
            if (travelled >= stepDistance)
            {
                travelled -= stepDistance;
                Step();
            }
        }

        void Step()
        {
            if (clips == null || clips.Length == 0) return;
            int i = Random.Range(0, clips.Length);
            if (clips.Length > 1 && i == lastIndex) i = (i + 1) % clips.Length; // never the same clip twice in a row
            lastIndex = i;
            src.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
            if (clips[i] != null) src.PlayOneShot(clips[i], volume);
        }
    }
}
