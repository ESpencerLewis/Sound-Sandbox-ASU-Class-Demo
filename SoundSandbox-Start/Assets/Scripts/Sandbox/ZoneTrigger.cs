using UnityEngine;
using UnityEngine.Audio;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// When the player walks into this box:
    ///  1) the ZoneMixer eases the room volume to roomGain (works on desktop AND Unity Web builds), and
    ///  2) optionally blends to an Audio Mixer snapshot (desktop/editor only if the snapshot changes reverb,
    ///     low-pass or other effects, because Unity Web only supports mixer volume).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ZoneTrigger : MonoBehaviour
    {
        public ZoneMixer zoneMixer;
        [Range(0f, 1f)] public float roomGain = 1f;
        public float transitionSeconds = 1.5f;
        [Tooltip("Optional. Effect snapshots (reverb, low-pass) work in the Editor and desktop builds, not on Web.")]
        public AudioMixerSnapshot snapshot;
        public string playerTag = "Player";

        void Reset() { GetComponent<Collider>().isTrigger = true; }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag)) return;
            if (zoneMixer != null) zoneMixer.SetTarget(roomGain, transitionSeconds);
            if (snapshot != null) snapshot.TransitionTo(transitionSeconds);
        }
    }
}
