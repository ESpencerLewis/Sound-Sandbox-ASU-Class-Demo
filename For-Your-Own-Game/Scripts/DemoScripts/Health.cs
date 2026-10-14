using UnityEngine;

namespace Synapsody.Sandbox
{
    /// <summary>
    /// A health value from 0 (no health) to 1 (full). Other scripts call Damage and Heal.
    /// Draws a slider at the top right of the Game window that you can also drag to test the sound.
    /// </summary>
    public class Health : MonoBehaviour
    {
        [Range(0f, 1f)] public float value = 1f;
        public bool showSlider = true;

        public void Damage(float amount) { value = Mathf.Clamp01(value - amount); }
        public void Heal(float amount) { value = Mathf.Clamp01(value + amount); }

        void OnGUI()
        {
            if (!showSlider) return;
            GUILayout.BeginArea(new Rect(Screen.width - 232, 12, 220, 70), GUI.skin.box);
            GUILayout.Label($"Health {value:0.00} (drag to test)");
            value = GUILayout.HorizontalSlider(value, 0f, 1f);
            GUILayout.EndArea();
        }
    }
}
