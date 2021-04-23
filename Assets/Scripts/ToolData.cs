using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolData
{
    public int rockAmmo;
    public int dartAmmo;
    public int bowAmmo;

    public ToolData(Tools_Abilities toolbelt)
    {
        rockAmmo = 1;
        dartAmmo = 1;
        bowAmmo = 1;
    
    }


}
