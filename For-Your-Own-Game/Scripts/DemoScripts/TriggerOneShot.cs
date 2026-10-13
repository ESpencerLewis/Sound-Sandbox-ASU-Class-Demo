using System.Collections;
using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Put this on a cube whose Box Collider has Is Trigger ON. When the Player walks into it, the clip plays once.
    /// Use it for the Pick Up cube (hide it afterwards) and the Win cube (optionally fade other loops out).
    /// Needs the Player tagged "Player" and a CharacterController on it (the Sandbox player already has both).
    /// Works on Unity Web: it is a plain one-shot clip.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerOneShot : MonoBehaviour
    {
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public string playerTag = "Player";
        [Tooltip("Hide the cube after it is touched (good for a pick up).")]
        public bool hideAfter = true;
        [Tooltip("Loops to fade out when this is touched (for example the Ambient and Danger sounds at the Win cube).")]
        public AudioSource[] fadeOut;
        public float fadeSeconds = 2f;

        AudioSource src;
        bool done;

        void Awake()
        {
            src = GetComponent<AudioSource>();
            if (src == null) src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f; // 2D: heard clearly wherever it is
        }

        void OnTriggerEnter(Collider other)
        {
            if (done || !other.CompareTag(playerTag)) return;
            done = true;
            if (clip != null) src.PlayOneShot(clip, volume);
            if (hideAfter)
            {
                foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
                GetComponent<Collider>().enabled = false;
            }
            if (fadeOut != null && fadeOut.Length > 0) StartCoroutine(Fade());
        }

        IEnumerator Fade()
        {
            float[] start = new float[fadeOut.Length];
            for (int i = 0; i < fadeOut.Length; i++) start[i] = fadeOut[i] != null ? fadeOut[i].volume : 0f;
            for (float t = 0f; t < fadeSeconds; t += Time.deltaTime)
            {
                float k = 1f - t / Mathf.Max(0.01f, fadeSeconds);
                for (int i = 0; i < fadeOut.Length; i++) if (fadeOut[i] != null) fadeOut[i].volume = start[i] * k;
                yield return null;
            }
            foreach (var s in fadeOut) if (s != null) { s.volume = 0f; s.Stop(); }
        }
    }
}
