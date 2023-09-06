using Codice.Client.BaseCommands;
using UnityEngine;

namespace Cittius.Interaction
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private bool interactOnCollision;
        
        //Activate
        public void Activate(ActivateArg arg)
        {
            InteractionManager.Add(arg);
        }

        public virtual bool ActivateRaycast(Vector3 origin, Vector3 direction, float distance, out ActivateArg arg)
        {
            if (ValidateRaycast(origin, direction, distance, out IActivate activate))
            {
                ActivateArg n_arg = new ActivateArg(this, activate);
                Activate(n_arg);
                arg = n_arg;
            }


            arg = null;
            return false;
        }

        public virtual bool ActivateRaycast(Ray ray, float distance, out ActivateArg arg)
        {
            return ActivateRaycast(ray.origin, ray.direction, distance, out arg);
        }


        public void CancelActivate()
        {
            InteractionManager.Clear(this, 2);
        }

        //Interaction
        public void Interact(InteractionArg arg)
        {
            InteractionManager.Add(arg);
        }

        public virtual bool InteractRaycast(Vector3 origin, Vector3 direction, float distance, out InteractionArg arg)
        {
            if (ValidateRaycast(origin, direction, distance, out IInteract interact))
            {
                InteractionArg n_arg = new InteractionArg(this, interact);
                Interact(n_arg);
                arg = n_arg;
                return true;
            }


            arg = null;
            return false;
        }

        public bool InteractRaycast(Ray ray, float distance, out InteractionArg arg)
        {
            return InteractRaycast(ray.origin, ray.direction, distance, out arg);
        }

        public void CancelInteraction()
        {
            InteractionManager.Clear(this, 1);
        }

        //Interaction By Collision

        #region Collision

        // private void OnCollisionEnter(Collision collision)
        // {
        //     InteractCollision(collision.collider);
        // }
        //
        // private void OnCollisionExit(Collision collision)
        // {
        //     CancelInteractByCollision(collision.collider);
        // }
        //
        // private void OnTriggerEnter(Collider other)
        // {
        //     InteractCollision(other);
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     CancelInteractByCollision(other);
        // }
        //
        // public void InteractCollision(Collider collision)
        // {
        //     if (ValidateCollision<IInteract>(collision, out IInteract interact))
        //     {
        //         Interact(new InteractionArg(this, interact));
        //     }
        // }
        //
        // public void CancelInteractByCollision(Collider collision)
        // {
        //     if (ValidateCollision<IInteract>(collision, out IInteract interact))
        //     {
        //         CancelInteraction();
        //     }
        // }

        private bool ValidateCollision<T>(Collider collision, out T component)
        {
            if (interactOnCollision && collision.attachedRigidbody &&
                collision.attachedRigidbody.TryGetComponent<T>(out T interactBase)
                || collision.TryGetComponent<T>(out interactBase))
            {
                component = interactBase;
                return true;
            }

            component = default(T);
            return false;
        }

        private bool ValidateRaycast<T>(Vector3 origin, Vector3 direction, float distance, out T component)
        {
            Debug.DrawRay(origin, direction * distance, Color.red, 1f);
            if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, distance))
            {
                if (hitInfo.collider.attachedRigidbody
                    && hitInfo.collider.attachedRigidbody.TryGetComponent<T>(out T interact))
                {
                    component = interact;
                    return true;
                }
            }

            component = default(T);
            return false;
        }

        #endregion
    }
}