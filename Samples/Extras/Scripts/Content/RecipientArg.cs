namespace Cittius.Interaction.Extras
{
    public struct RecipientArg
    {
        public Recipient drained;
        public Recipient filled;
        public int quantity;

        public RecipientArg(Recipient drained, Recipient filled, int quantity)
        {
            this.drained = drained;
            this.filled = filled;
            this.quantity = quantity;
        }
    }
}