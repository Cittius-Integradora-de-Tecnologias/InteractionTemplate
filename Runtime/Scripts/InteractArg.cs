using UnityEngine;

namespace Cittius.Interaction
{
    public class InteractionArg
    {
        public Interactor interactor;
        public IInteract interacted;

        public InteractionArg(Interactor interactor, IInteract interacted)
        {
            this.interactor = interactor;
            this.interacted = interacted;
        }
    }
}
