using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IInteractable
    {
        public void Interact(Action onInteractComplete = null);
    }
}
