using System.Data.SqlTypes;

namespace Cittius.Interaction
{
    public class ActivateArg
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