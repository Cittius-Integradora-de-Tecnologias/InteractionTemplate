using UnityEngine;

namespace HotQueen.Interaction
{
    public struct InteractionArg
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
