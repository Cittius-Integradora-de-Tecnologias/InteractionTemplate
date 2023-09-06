using UnityEngine;

namespace Cittius.Interaction.Data
{
    [CreateAssetMenu(fileName = "newItemRegistry", menuName = "Cittius/Item/RegisterItem")]
    public class InteractableScriptable : ScriptableObject
    {
        [SerializeField] private InteractableData m_data;
        public InteractableData data { get { return m_data; } }

        [SerializeField] private Interactable m_prefab;
        public Interactable prefab { get { return m_prefab; } }
    }
}
