using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotQueen.Interaction
{
    public interface IActivate
    {
        public event Action<ActivateArg> activated;
        public event Action<ActivateArg> deactivated;
        public Transform transform { get; }

        public void Activate(ActivateArg args);
        public void Deactivate(ActivateArg args);
    }
}
