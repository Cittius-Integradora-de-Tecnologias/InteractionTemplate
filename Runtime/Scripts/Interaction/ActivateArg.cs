using UnityEngine;

namespace Cittius.Interaction
{
    public struct ActivateArg
    {
        public Interactor interactor;
        public IActivate activated;

        public ActivateArg(Interactor interactor, IActivate activated)
        {
            this.interactor = interactor;
            this.activated = activated;
        }
    }
}
