using Cittius.Interaction.Data;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Cittius.Interaction
{
    public class Interactable : MonoBehaviour, IInteract, IActivate
    {
        [SerializeField] private InteractableData m_data;

        public InteractableData data
        {
            get { return m_data; }
        }

        //unity event
        [SerializeField] private UnityEvent<InteractionArg> onInteracted;
        [SerializeField] private UnityEvent<InteractionArg> onStoppedInteraction;
        [SerializeField] private UnityEvent<ActivateArg> onActivated;
        [SerializeField] private UnityEvent<ActivateArg> onDeactivated;

        public Transform transform
        {
            get { return base.transform; }
        }

        void IActivate.Activate(ActivateArg args)
        {
            onActivated?.Invoke(args);
        }

        void IActivate.Deactivate(ActivateArg args)
        {
            onDeactivated?.Invoke(args);
        }

        void IInteract.Interact(InteractionArg args)
        {
            onInteracted?.Invoke(args);
        }

        void IInteract.StopInteraction(InteractionArg args)
        {
            onStoppedInteraction?.Invoke(args);
        }

        //action
        public event Action<InteractionArg> interacted;
        public event Action<InteractionArg> stoppedInteraction;
        public event Action<ActivateArg> activated;
        public event Action<ActivateArg> deactivated;

        private void Awake()
        {
            interacted += (ctx) => onInteracted?.Invoke(ctx);
            stoppedInteraction += (ctx) => onStoppedInteraction?.Invoke(ctx);
            activated += (ctx) => onActivated?.Invoke(ctx);
            deactivated += (ctx) => onDeactivated?.Invoke(ctx);
            
            
        }


        // /// <summary>
        // /// On colide with another Iactivate object, this component will try to trigger interactor to initiate a activation.
        // /// Else will do nothing
        // /// </summary>
        // /// <param name="collision"></param>
        // private void OnCollisionEnter(Collision collision)
        // {
        //     if (collision.collider.attachedRigidbody
        //         && collision.collider.attachedRigidbody.TryGetComponent<IActivate>(out IActivate activate))
        //     {
        //         Interactor interactor = InteractionManager.FindInteractor(this);
        //         if (interactor)
        //         {
        //             interactor.Activate(activate);
        //         }
        //     }
        // }
        //
        // private void OnCollisionExit(Collision collision)
        // {
        //     if (collision.collider.attachedRigidbody
        //         && collision.collider.attachedRigidbody.TryGetComponent<IActivate>(out IActivate activate))
        //     {
        //         Interactor interactor = InteractionManager.FindInteractor(this);
        //         if (interactor)
        //         {
        //             interactor.CancelActivate();
        //         }
        //     }
        // }
    }
}