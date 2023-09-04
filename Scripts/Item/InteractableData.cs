namespace Cittius.Interaction.Data
{
    [System.Serializable]
    public struct InteractableData
    {
        public string name, nameID, numericID;

        public InteractableData(string name, string nameID, string numericID)
        {
            this.name = name;
            this.nameID = nameID;
            this.numericID = numericID;
        }
    }
}
