using System;
using UnityEngine;

namespace HotQueen.Interaction
{
    public interface IInteract
    {
        public event Action<InteractionArg> interacted;
        public event Action<InteractionArg> stoppedInteraction;
        public Transform transform { get; }

        public void Interact(InteractionArg args);
        public void StopInteraction(InteractionArg args);

    }
}
