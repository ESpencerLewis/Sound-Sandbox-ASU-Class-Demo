using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Synapsody.Sandbox
{
    /// <summary>
    /// Minimal walk-around controller for the class. W/S move, A/D strafe, Q/E turn.
    /// Works with either Unity input system. Needs a CharacterController on the same object,
    /// and the object tagged "Player" so ZoneTrigger can detect it. Put the Audio Listener on the camera
    /// (a child of this object).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class SandboxMover : MonoBehaviour
    {
        public float speed = 3f;
        public float turnDegreesPerSecond = 90f;
        CharacterController cc;

        void Awake() { cc = GetComponent<CharacterController>(); }

        void Update()
        {
            ReadInput(out float x, out float z, out float turn);
            transform.Rotate(0f, turn * turnDegreesPerSecond * Time.deltaTime, 0f);
            Vector3 move = (transform.right * x + transform.forward * z) * speed;
            move.y = -2f; // keep grounded
            cc.Move(move * Time.deltaTime);
        }

        static void ReadInput(out float x, out float z, out float turn)
        {
#if ENABLE_INPUT_SYSTEM
            var k = Keyboard.current;
            if (k == null) { x = z = turn = 0f; return; }
            x = (k.dKey.isPressed ? 1f : 0f) - (k.aKey.isPressed ? 1f : 0f);
            z = (k.wKey.isPressed ? 1f : 0f) - (k.sKey.isPressed ? 1f : 0f);
            turn = (k.eKey.isPressed ? 1f : 0f) - (k.qKey.isPressed ? 1f : 0f);
#else
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            turn = (Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f);
#endif
        }
    }
}
