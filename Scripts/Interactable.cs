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

        protected void Start()
        {
            InteractionManager.registeredInteraction += (arg) =>
            {
                if (arg.interacted == (IInteract)this)
                {
                    interacted?.Invoke(arg);
                }
            };

            InteractionManager.removedInteraction += (arg) =>
            {
                if (arg.interacted == (IInteract)this)
                {
                    stoppedInteraction?.Invoke(arg);
                }
            };

            InteractionManager.registeredActivation += (arg) =>
            {
                if (arg.activated == (IActivate)this)
                {
                    activated?.Invoke(arg);
                }
            };

            InteractionManager.removedActivation += (arg) =>
            {
                if (arg.activated == (IActivate)this)
                {
                    deactivated?.Invoke(arg);
                }
            };
        }
    }
}