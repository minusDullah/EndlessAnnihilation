using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkHandler : MonoBehaviour
{
    [SerializeField] private List<PerkMachineScriptable> perkMachineScriptable;
    [SerializeField] private Dictionary<string, bool> My_dict;
    
    private void Awake()
    {
        My_dict = new Dictionary<string, bool>();

        My_dict.Clear();

        foreach (PerkMachineScriptable perkMachineScriptable in perkMachineScriptable)
        {
            My_dict.Add(perkMachineScriptable.perkName, false);
        }
    }

    public void BuyPerk(string perkName)
    {
        My_dict.Remove(perkName);
        My_dict.Add(perkName, true);
    }

    public bool AlreadyBought(string perkName)
    {
        My_dict.TryGetValue(perkName, out bool alreadyBought);
        return alreadyBought;
    }
}
