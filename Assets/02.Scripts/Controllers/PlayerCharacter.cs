using UnityEngine;
using UnityEngine.InputSystem;

namespace FoodyGo.Controllers
{
    public class PlayerCharacter : MonoBehaviour
    {
        public Vector3 velocity;
        public float speed;

        [SerializeField] InputActionReference _moveInputAction;

        private void OnEnable()
        {
            _moveInputAction.action.performed += OnMovePerformed;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 direction = new Vector3(input.x, 0, input.y).normalized;
            if (direction.magnitude > 0.1f)
            {
                velocity = direction * speed;
            }
            else
            {
                velocity = Vector3.zero;
            }
        }
    }
}
