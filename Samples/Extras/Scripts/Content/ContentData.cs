using UnityEngine;

namespace Cittius.Interaction.Extras
{

    [CreateAssetMenu(fileName = "NewContent", menuName = "Cittius/Interaction/Recipient/Content")]
    public class ContentData : ScriptableObject
    {
        public string name;
        public string description;
        public float density;
        public Material material;
    }

}