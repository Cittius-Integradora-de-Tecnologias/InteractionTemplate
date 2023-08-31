using UnityEngine;
using UnityEngine.InputSystem;

namespace Cittius.Interaction.Extras
{
    [RequireComponent(typeof(Rigidbody))]
    public class KeyboardPlayer : MonoBehaviour
    {
        [Header("Interaction")] public float distance = 1f;
        public Interactor interactor;

        [Header("Control")] public float moveSpeed = 1;
        public float rotationSpeed = 1;
        [SerializeField] private Camera playerCamera;
        private Rigidbody rb;

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1f);

            Move();
            Rotation();


            if (Input.GetMouseButtonDown(0))
            {
                interactor.InteractRaycast(Camera.main.ScreenPointToRay(Input.mousePosition), distance,
                    out InteractionArg arg);
            }
            else if (Input.GetMouseButtonUp(0))

            {
                interactor.CancelInteraction();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                interactor.ActivateRaycast(Camera.main.ScreenPointToRay(Input.mousePosition), distance,
                    out ActivateArg activate);
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                interactor.CancelActivate();
            }
        }

        private void Move()
        {
            Vector3 direction = new Vector3();
            direction.x = Input.GetAxis("Horizontal");
            direction.z = Input.GetAxis("Vertical");
            direction.y = Input.mouseScrollDelta.y;

            direction *= moveSpeed * Time.fixedDeltaTime;
            Vector3 movement = (this.transform.forward * direction.z) + (this.transform.right * direction.x);
            movement.y = rb.velocity.y;
            rb.velocity = movement;
        }

        Vector3 currentRotation = new Vector3();

        private void Rotation()
        {
            if (Input.mousePosition.magnitude > 0.1f)
            {
                currentRotation += GetMouseDelta() * rotationSpeed * Time.fixedDeltaTime;
                currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
                currentRotation.z = 0;
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, currentRotation.y, 0);
                playerCamera.transform.eulerAngles =
                    new Vector3(currentRotation.x, playerCamera.transform.eulerAngles.y, 0);
            }
        }

        private Vector3 GetMouseDelta()
        {
            Vector2 mousePosition_Delta = Mouse.current.delta.ReadValue();
            return new Vector3(-mousePosition_Delta.y, mousePosition_Delta.x, 0);
        }
    }
}