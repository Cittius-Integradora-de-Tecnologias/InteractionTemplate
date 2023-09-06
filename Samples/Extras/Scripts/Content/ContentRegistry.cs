using System.Collections.Generic;

namespace Cittius.Interaction.Extras
{

    public static class ContentRegistry
    {
        public static List<ContentData> contentList = new List<ContentData>();
        public static List<MixData> mixList = new List<MixData>();

        public static bool TryMix(ContentData[] contentsToMix, out MixData result)
        {
            foreach (var mix in mixList)
            {
                foreach (var mixContents in mix.contents)
                {
                    foreach (var content in contentsToMix)
                    {
                        if (mixContents != content)
                        {
                            break;
                        }
                    }
                }
                result = mix;
                return true;
            }
            result = new MixData();
            return false;
        }
    }

}