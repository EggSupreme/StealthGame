using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolData
{
    public int[] ammoArray;

    public ToolData(Tools_Abilities toolbelt)
    {
        ammoArray = toolbelt.GetAmmoData();
    
    }


}
