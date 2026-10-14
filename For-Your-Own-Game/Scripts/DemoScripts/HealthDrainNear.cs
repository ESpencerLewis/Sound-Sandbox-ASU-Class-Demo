using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Put this on a dangerous object (for example the DANGER cube). While the Player is within Radius metres,
    /// health drains; farther away, it slowly recovers. Works in any 3D game.
    /// </summary>
    public class HealthDrainNear : MonoBehaviour
    {
        public Health health;
        public Transform target;                 // leave empty to find the object tagged Player
        public string playerTag = "Player";
        public float radius = 6f;
        public float drainPerSecond = 0.1f;
        public float regenPerSecond = 0.03f;

        void Update()
        {
            if (health == null) return;
            if (target == null)
            {
                var p = GameObject.FindWithTag(playerTag);
                if (p == null) return;
                target = p.transform;
            }
            float d = Vector3.Distance(target.position, transform.position);
            if (d <= radius) health.Damage(drainPerSecond * Time.deltaTime);
            else health.Heal(regenPerSecond * Time.deltaTime);
        }
    }
}
