using UnityEngine;

public enum AmmoType
{
    arrow,
    rock,
    sleepDart
}

public class AmmoStation
{
    static int[] storeAmmoCount = { 0, 0, 0 };

    static public void Store(AmmoType _type, int _amount)
    {
        storeAmmoCount[(int)_type] += _amount;
        Debug.Log("The store has " + storeAmmoCount[(int)_type] + " of type " + _type.ToString());
    }
    static public int TakeAll(AmmoType _type)
    {
        int total = Count(_type);
        storeAmmoCount[(int)_type] = 0;
        return total;
    }
    static public int[] TakeAll()
    {
        int[] totals = { 0, 0, 0 };
        for (int i = 0; i < storeAmmoCount.Length; i++)
        {
            totals[i] = storeAmmoCount[i];
            storeAmmoCount[i] = 0;
        }
        return totals;
    }
    static public int Count(AmmoType _type) { return storeAmmoCount[(int)_type]; }
}
