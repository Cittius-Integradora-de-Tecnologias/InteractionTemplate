using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Cittius.Interaction
{
    [RequireComponent(typeof(InteractBase))]

    public class Recipient : MonoBehaviour
    {
        [System.Serializable]
        public struct RecipientContent
        {
            public ContentData content;
            public int quantity;

            public RecipientContent(ContentData content, int quantity)
            {
                this.content = content;
                this.quantity = quantity;
            }
        }

        [SerializeField] private List<RecipientContent> m_storedContents;
        public List<RecipientContent> storedContents { get { return m_storedContents; } }
        private InteractBase interact;

        [Header("Configuration")]
        public bool canBeAdded = false;
        public bool isInfinite = false;
        public bool havaLimit = true;
        public int maxLimite = 1000; //1000 == 1L


        [Header("Recipient Events")]
        public UnityEvent<RecipientContent> onAdded;

        private void Start()
        {
            interact = this.gameObject.GetComponent<InteractBase>();
            interact.activated += Transfer;
        }

        /// <summary>
        /// By defualt is called when this Object is grabbed by a interactor and interacted with another recipient, 
        /// this method will try to transfer it first content to all activated recipients
        /// </summary>
        /// <param name="args"></param>
        public void Transfer(ActivateArg args)
        {
            IInteract interact = InteractionManager.FindInteraction(args.interactor);
            if (
                interact != null
                && interact.transform.TryGetComponent<Recipient>(out Recipient recipient)
                && recipient.storedContents.Count > 0
                )
            {

                RecipientContent repContent = recipient.m_storedContents.Last();
                if (recipient.TryRemoveContent(repContent.content, false, 10))
                {
                    TryAddContent(repContent);
                }
                return;
            }
        }

        [Obsolete("Use TryAddContent instead", true)]
        public void Fill(string contentName)
        {
            //TryAddContent(null);
        }

        /// <summary>
        /// It will make a attempt to add content to this recipient,
        /// return false if the attempt fail
        /// return true if the attempt succed
        /// </summary>
        /// <param name="recipientData"></param>
        /// <returns></returns>
        public bool TryAddContent(RecipientContent recipientData)
        {
            if (canBeAdded && GetQuantity() + recipientData.quantity < maxLimite)
            {
                AddContent(recipientData);
                onAdded?.Invoke(recipientData);
                Debug.Log(this.transform.name + " Filled with " + recipientData.content.name);
                Debug.Log(this.transform.name + " have " + GetQuantity() + "ml");
                return true;
            }
            else
            {
                return false;
            }

            //ContentRegistry.TryMix(GetContents(), out MixData result);

        }

        /// <summary>
        /// This method will remove or reduce the registered content in this recipient,
        /// if the data is not registered this method will return null.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clear"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool TryRemoveContent(ContentData data, bool clear = true, int quantity = 20)
        {
            if (data == null) { return false; }
            RecipientContent[] contents = this.m_storedContents.ToArray();
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i].content.name == data.name)// contains 
                {
                    if (clear || contents[i].quantity - quantity <= 0)
                    {
                        m_storedContents.RemoveAt(i);

                    }
                    else
                    {
                        contents[i].quantity -= quantity;
                        m_storedContents = contents.ToList();
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This method will add new content if it is not regitered in this recipient,
        /// If registered the quanity will me added.
        /// </summary>
        /// <param name="content"></param>
        private void AddContent(RecipientContent content)
        {
            RecipientContent[] contents = this.m_storedContents.ToArray();
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i].content.name == content.content.name)// contains 
                {
                    contents[i] = new RecipientContent(contents[i].content, contents[i].quantity += content.quantity);
                    m_storedContents = contents.ToList();
                    return;
                }
            }
            m_storedContents.Add(content);
        }

        /// <summary>
        /// Return the sum of all contents present on this recipient
        /// </summary>
        /// <returns></returns>
        public int GetQuantity()
        {
            int sum = 0;
            foreach (var item in m_storedContents)
            {
                sum += item.quantity;
            }
            return sum;
        }

        /// <summary>
        /// Return all the contents present in this recipient without quantity
        /// </summary>
        /// <returns></returns>
        public ContentData[] GetContents()
        {
            ContentData[] stored = new ContentData[0];
            foreach (var item in m_storedContents)
            {
                stored.Append(item.content);
            }
            return stored;
        }
    }
}
