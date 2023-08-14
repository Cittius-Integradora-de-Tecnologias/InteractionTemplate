using UnityEngine;

namespace Cittius.Interaction
{

    public class Interactor : MonoBehaviour
    {
        [SerializeField] private bool interactOnCollision;

        //Attach references
        [SerializeField] private Transform m_attach;
        public Transform attach { get { return m_attach; } }

        //Activate

        public void Activate(IActivate activate)
        {
            InteractionManager.Activate(new ActivateArg(this, activate));
        }

        public void CancelActivate()
        {
            InteractionManager.Deactivate(this);
        }

        //Interaction
        public void Interact(Interactable interact)
        {
            InteractionArg arg = new InteractionArg(this, interact);
            InteractionManager.RegisterInteraction(arg);

        }

        public void CancelInteraction()
        {
            InteractionManager.RemoveInteraction(this);
        }

        //Interaction By Collision

        private void OnCollisionEnter(Collision collision)
        {
            InteractByCollision(collision.collider);
        }

        private void OnCollisionExit(Collision collision)
        {
            CancelInteractByCollision(collision.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            InteractByCollision(other);
        }

        private void OnTriggerExit(Collider other)
        {
            CancelInteractByCollision(other);
        }

        public void InteractByCollision(Collider collision)
        {
            if (!interactOnCollision) { return; }

            if (collision.attachedRigidbody && collision.attachedRigidbody.TryGetComponent<Interactable>(out Interactable interactBase)
                || collision.TryGetComponent<Interactable>(out interactBase))
            {
                Interact(interactBase);
            }
        }

        public void CancelInteractByCollision(Collider collision)
        {
            if (!interactOnCollision) { return; }
            if (collision.attachedRigidbody && collision.attachedRigidbody.TryGetComponent<Interactable>(out Interactable interactBase)
                || collision.TryGetComponent<Interactable>(out interactBase))
            {
                CancelInteraction();
            }
        }
    }
}