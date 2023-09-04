using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cittius.Interaction
{
    public interface IActivate
    {
        public event Action<ActivateArg> activated;
        public event Action<ActivateArg> deactivated;
        public Transform transform { get; }

        protected void Activate(ActivateArg args);
        protected void Deactivate(ActivateArg args);
    }
}
