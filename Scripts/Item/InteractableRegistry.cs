using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cittius.Interaction.Data
{
    public class InteractableRegistry
    {
        public struct ItemInstanceRegistry
        {
            public InteractableData data;
            public List<Interactable> instances;

            public ItemInstanceRegistry(InteractableData data, List<Interactable> interactBase)
            {
                this.data = data;
                instances = interactBase;
            }
        }

        //Registry of itens
        //[SerializeField] private List<ItemScriptable> m_itemScriptables = new List<ItemScriptable>();
        private const string Scriptables_Path = "Prefab/Itens";
        public List<Interactable> interactableLoaded { get; private set; }//{ get { return m_itemScriptables; } }
        //Registry of instances
        public List<ItemInstanceRegistry> itemInstances { get; private set; }


        private static InteractableRegistry m_instance;
        public static InteractableRegistry instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new InteractableRegistry();
                }
                return m_instance;
            }
        }

        public void InitializeItemRegistry()
        {
            itemInstances = new List<ItemInstanceRegistry>();
            LoadResoureces();
            UpdateItemInstanceList();

            SceneManager.sceneLoaded += (scn, md) => { UpdateItemInstanceList(); };
        }


        public void LoadResoureces()
        {
            interactableLoaded = Resources.LoadAll<Interactable>(Scriptables_Path).ToList();
            Debug.Log("Loaded:" + interactableLoaded.Count + " Items");
        }

        public void UpdateItemInstanceList()
        {

            Interactable[] instances = GetAllItemInstances();

            foreach (var item in instances)
            {
                foreach (var other in interactableLoaded)
                {
                    if (other.data.numericID == item.data.numericID)
                    {
                        AddInstances(item);
                    }
                }
            }
        }

        private void AddInstances(Interactable instance)
        {
            foreach (var item in itemInstances)
            {
                if (item.data.numericID == instance.data.numericID)
                {
                    item.instances.Add(instance);
                    return;
                }
            }
            List<Interactable> interactables = new List<Interactable>();
            interactables.Add(instance);
            itemInstances.Add(new ItemInstanceRegistry(instance.data, interactables));
        }

        private Interactable[] GetAllItemInstances()
        {
            return GameObject.FindObjectsOfType<Interactable>();
        }
    }
}
