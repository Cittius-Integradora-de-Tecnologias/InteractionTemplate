namespace Cittius.Interaction.Extras
{

    public struct MixData
    {
        public ContentData[] contents;
        public ContentData result;

        public MixData(ContentData[] contents, ContentData result)
        {
            this.contents = contents;
            this.result = result;
        }
    }

}