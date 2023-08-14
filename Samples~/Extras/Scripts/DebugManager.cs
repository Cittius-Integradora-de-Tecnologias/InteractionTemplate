using Cittius.Interaction.Data;
using UnityEngine;

namespace Cittius.Interaction.Extras
{
    public class DebugManager : MonoBehaviour
    {
        private void Start()
        {
            InteractableRegistry.instance.InitializeItemRegistry();
        }
    }
}
