using System;
using UnityEngine;

namespace Cittius.Interaction
{
    public interface IInteract
    {
        public event Action<InteractionArg> interacted;
        public event Action<InteractionArg> stoppedInteraction;
        public Transform transform { get; }

        protected void Interact(InteractionArg args);
        protected void StopInteraction(InteractionArg args);

    }
}
