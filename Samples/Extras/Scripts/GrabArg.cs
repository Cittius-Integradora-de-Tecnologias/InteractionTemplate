namespace HotQueen.Interaction
{
    public struct GrabArg
    {
        public Interactor grabber;
        public Grabbable grabbable;

        public GrabArg(Interactor grabber, Grabbable grabbable)
        {
            this.grabber = grabber;
            this.grabbable = grabbable;
        }
    }
}
