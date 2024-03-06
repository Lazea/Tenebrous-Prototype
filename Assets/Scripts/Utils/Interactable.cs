using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        Submarine,
        Mineral,
    }
    [HideInInspector]
    public InteractableType interactableType;

    public virtual void Interact()
    {
        throw new NotImplementedException();
    }
}
