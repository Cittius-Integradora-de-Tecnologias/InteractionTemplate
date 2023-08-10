using System;
using System.Collections;
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
        [SerializeField] private bool m_isInfinite = false;
        public bool isInfinite { get { return m_isInfinite; } }
        public int transferenceAmount = 10;
        public int maxLimite = 1000; //1000 == 1L


        [Header("Recipient Events")]
        public UnityEvent<RecipientContent> onAdded;


        private Coroutine tranferenceCoroutine;
        private void Start()
        {
            interact = this.gameObject.GetComponent<InteractBase>();
            interact.activated += (arg) =>
            {
                if (canBeAdded)
                {
                    tranferenceCoroutine = StartCoroutine(InitTransference(arg, 1f));
                }
            };
            interact.deactivated += StopTransference;
        }

        private IEnumerator InitTransference(ActivateArg args, float delay)
        {
            IInteract interact = InteractionManager.FindInteraction(args.interactor);
            if (interact != null
                && interact.transform.TryGetComponent<Recipient>(out Recipient recipient)
                && recipient.storedContents.Count > 0)
            {
                while (true)
                {
                    ReceiveTransference(recipient, transferenceAmount);
                    yield return new WaitForSeconds(delay);
                }
            }
            yield return null;
        }

        private void StopTransference(ActivateArg obj)
        {
            if (tranferenceCoroutine != null)
            {
                StopCoroutine(tranferenceCoroutine);
                tranferenceCoroutine = null;
            }
        }

        /// <summary>
        /// By defualt is called when this Object is grabbed by a interactor and interacted with another recipient, 
        /// this method will try to transfer it first content to all activated recipients
        /// </summary>
        /// <param name="args"></param>
        public void ReceiveTransference(Recipient other, int transferenceAmount)
        {
            if (other.m_storedContents.Count() > 0)
            {
                RecipientContent repContent = other.m_storedContents.Last();
                if (other.isInfinite || other.TryRemoveContent(repContent.content, false, transferenceAmount))
                {
                    TryAddContent(repContent.content, transferenceAmount);
                }
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
        public bool TryAddContent(ContentData recipientData, int quantity = 20)
        {
            if (canBeAdded && GetQuantity() + quantity < maxLimite)
            {
                AddContent(recipientData, quantity);

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
        private void AddContent(ContentData content, int quantity)
        {

            RecipientContent recipientContent = new RecipientContent(content, quantity);
            if (!TryIncrement(recipientContent))
            {
                m_storedContents.Add(recipientContent);
            }

            onAdded?.Invoke(recipientContent);
            Debug.Log(this.transform.name + " Filled with " + recipientContent.content.name);
            Debug.Log(this.transform.name + " have " + GetQuantity() + "ml");

        }

        public bool TryIncrement(RecipientContent n_content)
        {
            RecipientContent[] contents = this.m_storedContents.ToArray();
            RecipientContent l_content = new RecipientContent();
            for (int i = 0; i < contents.Length; i++)
            {
                l_content = contents.ElementAt(i);
                if (l_content.content.name == n_content.content.name)// contains 
                {
                    l_content = new RecipientContent(l_content.content, l_content.quantity += n_content.quantity);
                    contents[i] = l_content;
                    m_storedContents = contents.ToList();
                    return true;
                }
            }
            return false;
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
