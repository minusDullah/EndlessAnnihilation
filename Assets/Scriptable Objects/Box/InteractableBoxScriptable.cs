using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interactable Box Scriptable Object", menuName = ("ScriptableObjects/Interactable Box"))]
public class InteractableBoxScriptable : ScriptableObject
{
    public string prompt;
    public int cost;
}
