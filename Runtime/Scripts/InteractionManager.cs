using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using Codice.Client.BaseCommands.Merge;
using UnityEditor;

namespace Cittius.Interaction
{
    public static class InteractionManager
    {
        public struct RegistryData
        {
            public List<InteractionArg> interactionArgs;
            public List<ActivateArg> activateArgs;

            public RegistryData(List<InteractionArg> interactionArgs, List<ActivateArg> activateArgs)
            {
                this.interactionArgs = interactionArgs;
                this.activateArgs = activateArgs;
            }
        }

        public delegate void RegistryEvent<T>(T value);

        public static Dictionary<Interactor, RegistryData> InteractionRegistry =
            new Dictionary<Interactor, RegistryData>();

        public static event Action<InteractionArg> registeredInteraction;
        public static event Action<ActivateArg> registeredActivation;

        public static void Add<T>(T data)
        {
            RegistryData regData = new RegistryData();
            switch (data)
            {
                case ActivateArg:
                    ActivateArg activate = data as ActivateArg;
                    if (InteractionRegistry.Keys.Contains(activate.interactor))
                    {
                        InteractionRegistry[activate.interactor].activateArgs.Add(activate);
                    }
                    else
                    {
                        List<ActivateArg> activateArgs = new List<ActivateArg>();
                        activateArgs.Add(activate);
                        InteractionRegistry.Add(activate.interactor,
                            new RegistryData(new List<InteractionArg>(), activateArgs));
                    }

                    registeredActivation?.Invoke(activate);
                    break;
                case InteractionArg:
                    InteractionArg interaction = data as InteractionArg;
                    if (InteractionRegistry.Keys.Contains(interaction.interactor))
                    {
                        InteractionRegistry[interaction.interactor].interactionArgs.Add(interaction);
                    }
                    else
                    {
                        List<InteractionArg> interactionArgs = new List<InteractionArg>();
                        interactionArgs.Add(interaction);
                        InteractionRegistry.Add(interaction.interactor,
                            new RegistryData(interactionArgs, new List<ActivateArg>()));
                    }

                    registeredInteraction?.Invoke(interaction);
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        ///  Clear all <param name="interactor"></param> interactions
        /// </summary>
        public static void Clear(Interactor interactor)
        {
            if (InteractionRegistry.Keys.Contains(interactor))
            {
                RegistryData registry = InteractionRegistry[interactor];

                List<ActivateArg> activateRegistry = new List<ActivateArg>(registry.activateArgs);
                foreach (var activate in activateRegistry)
                {
                    Remove(interactor, activate);
                }

                activateRegistry.Clear();

                List<InteractionArg> interactionRegistry = new List<InteractionArg>(registry.interactionArgs);
                foreach (var interaction in interactionRegistry)
                {
                    Remove(interactor, interaction);
                }

                interactionRegistry.Clear();
            }
        }

        public static event Action<InteractionArg> removedInteraction;
        public static event Action<ActivateArg> removedActivation;

        /// <summary>
        /// Clear the exact <param name="data"></param> interaction
        /// </summary>
        public static void Remove<T>(Interactor interactor, T data)
        {
            if (InteractionRegistry.Keys.Contains(interactor))
            {
                RegistryData registry = InteractionRegistry[interactor];
                switch (data)
                {
                    case ActivateArg:
                        foreach (var activate in registry.activateArgs)
                        {
                            if (activate.interactor == interactor)
                            {
                                registry.activateArgs.Remove(activate);
                                removedActivation?.Invoke(activate);
                                return;
                            }
                        }

                        break;
                    case InteractionArg:
                        foreach (var interaction in registry.interactionArgs)
                        {
                            if (interaction.interactor == interactor)
                            {
                                registry.interactionArgs.Remove(interaction);
                                removedInteraction?.Invoke(interaction);
                                return;
                            }
                        }

                        break;
                    default:
                        return;
                }
            }
        }

        public static bool FindInteraction(Interactor interactor, out InteractionArg[] interacts)
        {
            interacts = InteractionRegistry.Keys.Contains(interactor)
                ? InteractionRegistry[interactor].interactionArgs.ToArray()
                : null;
            return interacts == null;
        }

        /// <summary>
        /// Return all the <param name="interactBase"></param> interactors
        /// </summary>
        public static Interactor FindInteractor(Interactable interactBase)
        {
            // foreach (var item in m_activateArgs)
            // {
            //     if (item.activated == interactBase.GetComponent<IActivate>())
            //     {
            //         return item.interactor;
            //     }
            // }
            //
            // foreach (var item in m_interactionArgs)
            // {
            //     if (item.interacted == interactBase.GetComponent<IInteract>())
            //     {
            //         return item.interactor;
            //     }
            // }
            //
            return null;
        }

        /// <summary>
        /// Return all the <param name="interactor"></param> activations
        /// </summary>
        public static bool FindActivity(Interactor interactor, out ActivateArg[] arg)
        {
            arg = InteractionRegistry.Keys.Contains(interactor)
                ? InteractionRegistry[interactor].activateArgs.ToArray()
                : null;
            return arg == null;
        }
    }
}