using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// CLASS EXERCISE. Goal: the longer the player stays inside the "Tense" zone, the higher the intensity climbs,
    /// and when they leave it settles back down. Fill in the TODOs. A finished version is in
    /// StudentExercise_Solution.txt, so look only after trying.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class StudentExercise : MonoBehaviour
    {
        public AdaptiveMixer adaptive;
        public string playerTag = "Player";
        public float risePerSecond = 0.15f;
        public float fallPerSecond = 0.10f;

        bool playerInside;

        void Reset() { GetComponent<Collider>().isTrigger = true; }

        void OnTriggerEnter(Collider other)
        {
            // TODO 1: if the other object has the player tag, set playerInside to true.
        }

        void OnTriggerExit(Collider other)
        {
            // TODO 2: if the other object has the player tag, set playerInside to false.
        }

        void Update()
        {
            // TODO 3: if playerInside, call adaptive.AddIntensity(risePerSecond * Time.deltaTime).
            //         Otherwise call adaptive.AddIntensity(-fallPerSecond * Time.deltaTime).
        }
    }
}
