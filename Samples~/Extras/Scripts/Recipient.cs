using HotQueen.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace HotQueen.Interaction
{
    public class Recipient : MonoBehaviour
    {
        [Header("Recipient Values")]
        [SerializeField] private string m_contentName;
        public string contentName { get { return m_contentName; } }
        //public bool emptyAfterUse = true;
        public bool canBeFilled = false;
        //public bool canTransfer = false;
        public InteractBase interact;

        [Header("Recipient Events")]
        //public UnityEvent onEmptied;
        public UnityEvent<string> onFilled;
        //public UnityEvent<Recipient> onTranfered;

        private void Start()
        {
            interact.activated += Transfer;
        }

        private void Transfer(ActivateArg args)
        {
            IActivate activate = InteractionManager.FindActivity(args.interactor);
            if (activate != null && activate.transform.TryGetComponent<Recipient>(out Recipient recipient))
            {
                Fill(recipient.contentName);
                return;
            }
        }

        public void Fill(string contentName)
        {
            if (canBeFilled && this.m_contentName == "")
            {
                this.m_contentName = contentName;
                //m_content.amount += content.amount;
                onFilled?.Invoke(contentName);
                Debug.Log(this.transform.name + " Filled with " + contentName);
            }

        }
    }
}
