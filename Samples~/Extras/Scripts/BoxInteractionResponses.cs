using UnityEngine;


namespace HotQueen.Interaction
{
    public class BoxInteractionResponses : MonoBehaviour
    {
        [SerializeField] private MeshRenderer m_renderer;
        public void ChangeColor(Color color)
        {
            Material mat = m_renderer.materials[0];
            mat.color = color;
        }
        public void ChangeColor(string hex)
        {
            Material mat = m_renderer.materials[0];
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                mat.color = color;
            }
        }
    }
}
