using UnityEngine;

namespace Cittius.Interaction.Extras
{
    public class MouseInteractor : MonoBehaviour
    {
        public float distance = 1f;
        public Interactor interactor;
        public float moveSpeed = 1;
        public float rotationSpeed = 1;

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, distance);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1f);

            Move();
            Rotation();
            InputInteraction(hitInfo);
            InputActivate(hitInfo);
        }

        private void InputActivate(RaycastHit hitInfo)
        {
            //Interact
            if (Input.GetKeyDown(KeyCode.E))
            {

                if (hitInfo.collider.attachedRigidbody
                    && hitInfo.collider.attachedRigidbody.TryGetComponent<InteractBase>(out InteractBase interact))
                {
                    interactor.Activate(interact);
                }
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                interactor.CancelActivate();
            }
        }

        private void InputInteraction(RaycastHit hitInfo)
        {
            //Interact
            if (Input.GetMouseButtonDown(0))
            {

                if (hitInfo.collider.attachedRigidbody
                    && hitInfo.collider.attachedRigidbody.TryGetComponent<InteractBase>(out InteractBase interact))
                {
                    interactor.Interact(interact);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                interactor.CancelInteraction();
            }
        }

        private void Move()
        {
            Vector3 direction = new Vector3();
            direction.x = Input.GetAxis("Horizontal");
            direction.z = Input.GetAxis("Vertical");
            direction.y = Input.mouseScrollDelta.y;

            this.transform.position += direction * moveSpeed;
        }

        Vector3 currentRotation = new Vector3();
        private void Rotation()
        {

            currentRotation += GetMouseDelta();
            //currentRotation.x = Mathf.Clamp(currentRotation.z, -90f, 90f);
            //currentRotation.z = 0;
            this.transform.eulerAngles = currentRotation;
        }

        private Vector2 mousePosition_old;
        private Vector3 GetMouseDelta()
        {
            Vector2 mousePosition_New = Input.mousePosition;
            Vector2 mousePosition_Delta = mousePosition_old - mousePosition_New;
            mousePosition_Delta = mousePosition_Delta.normalized;
            mousePosition_old = mousePosition_New;
            return new Vector3(mousePosition_Delta.y, -mousePosition_Delta.x, 0);

        }
    }
}
