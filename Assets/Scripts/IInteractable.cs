using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string InteractionPrompt { get; }

    public bool Interact(EAInteractor interactor);
    public bool Close(EAInteractor interactor);
}
