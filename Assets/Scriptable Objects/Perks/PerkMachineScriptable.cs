using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Perk Machine Scriptable Object", menuName = ("ScriptableObjects/Perk Machine"))]
public class PerkMachineScriptable : ScriptableObject
{
    public string prompt;
    public string perkName;
    public int perkCost;
}
