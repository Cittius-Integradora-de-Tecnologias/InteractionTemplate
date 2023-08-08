using System.Collections.Generic;

namespace HotQueen.Interaction
{
    public static class InteractionManager
    {

        public delegate void RegistryEvent<T>(T value) where T : struct;
        //Interaction
        private static List<InteractionArg> m_interactionArgs = new List<InteractionArg>();
        public static event RegistryEvent<InteractionArg> OnRegistered; // Create delegate to registry
        public static event RegistryEvent<InteractionArg> OnRemoved;

        //Activate
        private static List<ActivateArg> m_activateArgs = new List<ActivateArg>();
        private static event RegistryEvent<ActivateArg> OnActivate;
        private static event RegistryEvent<ActivateArg> OnDeactivate;

        /// <summary>
        /// Handle interaction for the interactor and interactable
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool RegisterInteraction(InteractionArg arg)
        {
            if (
               m_interactionArgs.Contains(arg)
               || arg.interacted.transform.TryGetComponent<Interactor>(out Interactor interactor)
               && arg.interactor.transform.TryGetComponent<InteractBase>(out InteractBase interactBase)
               && m_interactionArgs.Contains(new InteractionArg(interactor, interactBase)
               )) { return false; }

            m_interactionArgs.Add(arg);
            OnRegistered?.Invoke(arg);
            arg.interacted.Interact(arg);
            return true;
        }

        /// <summary>
        /// Perform the cancelation of the previous interactions made by the interactor, It will return true if finished successfully or false otherwise.
        /// When called the <paramref name="interactables"/> can be informed, this means that only this exact interactables will be cancelled.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool RemoveInteraction(Interactor interactor, List<IInteract> interactables = null)
        {
            List<InteractionArg> args = new List<InteractionArg>();
            foreach (var item in m_interactionArgs)
            {
                if (item.interactor == interactor)
                {
                    if (interactables != null && !interactables.Contains(item.interacted))
                    {
                        continue;
                    }
                    InteractionArg arg = new InteractionArg(item.interactor, item.interacted);
                    arg.interacted.StopInteraction(arg);
                    OnRemoved?.Invoke(arg);
                    args.Add(arg);
                }
            }

            foreach (var item in args)
            {
                m_interactionArgs.Remove(item);
            }

            return true;
        }

        public static void Activate(ActivateArg arg)
        {
            arg.activated.Activate(arg);
            m_activateArgs.Add(arg);
            OnActivate?.Invoke(arg);
        }

        public static bool Deactivate(Interactor interactor)
        {
            foreach (var item in m_activateArgs)
            {
                if (item.interactor == interactor)
                {
                    ActivateArg arg = new ActivateArg(interactor, item.activated);
                    item.activated.Deactivate(arg);
                    m_activateArgs.Remove(arg);
                    OnDeactivate?.Invoke(arg);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return all the <param name="interactor"></param> interactions
        /// </summary>
        public static IInteract FindInteraction(Interactor interactor)
        {
            foreach (var item in m_interactionArgs)
            {
                if (item.interactor == interactor)
                {
                    return item.interacted;
                }
            }
            return null;
        }

        /// <summary>
        /// Return all the <param name="interactor"></param> interactions
        /// </summary>
        public static IActivate FindActivity(Interactor interactor)
        {
            foreach (var item in m_activateArgs)
            {
                if (item.interactor == interactor)
                {
                    return item.activated;
                }
            }
            return null;
        }

        /// <summary>
        /// Return all the <param name="interactor"></param> interactions
        /// </summary>
        public static Interactor FindInteractor(InteractBase interactBase)
        {
            foreach (var item in m_activateArgs)
            {
                if (item.activated == interactBase.GetComponent<IActivate>())
                {
                    return item.interactor;
                }
            }
            foreach (var item in m_interactionArgs)
            {
                if (item.interacted == interactBase.GetComponent<IInteract>())
                {
                    return item.interactor;
                }
            }
            return null;
        }
    }
}
