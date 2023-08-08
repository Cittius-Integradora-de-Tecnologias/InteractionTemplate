using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HotQueen.Interaction
{
    [RequireComponent(typeof(InteractBase))]
    public class Recipient : MonoBehaviour
    {
        public Dictionary<ContentData, int> contents { get; private set; }
        private InteractBase interact;

        [Header("Configuration")]
        public bool canBeAdded = false;
        public bool isInfinite = false;
        public bool havaLimit = true;
        public int maxLimite = 1000; //1000 == 1L


        [Header("Recipient Events")]
        public UnityEvent<ContentData> onAdded;

        private void Start()
        {
            interact.activated += Transfer;
        }

        private void Transfer(ActivateArg args)
        {
            IActivate activate = InteractionManager.FindActivity(args.interactor);
            if (activate != null && activate.transform.TryGetComponent<Recipient>(out Recipient recipient))
            {
                //TryAddContent();
                return;
            }
        }

        [Obsolete("Use TryAddContent instead", true)]
        public void Fill(string contentName)
        {
            //TryAddContent(null);
        }

        public bool TryAddContent(ContentData contentData, int quantity)
        {
            if (canBeAdded && GetQuantity() + quantity > maxLimite)
            {
                if (!contents.ContainsKey(contentData))
                {
                    contents.Add(contentData, quantity);
                }
                else
                {
                    foreach (var item in contents)
                    {
                        if (item.Key.name == contentData.name)
                        {
                            contents[item.Key] += quantity;
                        }
                    }
                }

            }

            onAdded?.Invoke(contentData);
            Debug.Log(this.transform.name + " Filled with " + contents);
            Debug.Log(this.transform.name + " have " + GetQuantity() + "ml");
            return true;
        }

        public int GetQuantity()
        {
            int sum = 0;
            foreach (var item in contents)
            {
                sum += item.Value;
            }
            return sum;
        }
    }
}
