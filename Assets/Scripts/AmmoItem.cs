using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ammo Item")]
public class AmmoItem : Item
{
    public AmmoType ammoType;
    public int ammoRemaining;
    public int maxAmmoStackCapacity = 54;
}
