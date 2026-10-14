using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Put this on a cube whose collider has Is Trigger ON (for example the PICK UP cube).
    /// When the Player walks into it, health goes up by Amount. It can sit next to Trigger One Shot on the same cube.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HealOnTrigger : MonoBehaviour
    {
        public Health health;
        [Range(0f, 1f)] public float amount = 0.3f;
        public string playerTag = "Player";

        void OnTriggerEnter(Collider other)
        {
            if (health != null && other.CompareTag(playerTag)) health.Heal(amount);
        }
    }
}
