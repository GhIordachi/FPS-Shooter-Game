using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/HealthKit")]
public class HealthKitItem : Item
{
    public int maxHealthKitCount = 3;
    public int healthKitCount = 0;
    public int healthToBeRestored = 70;
}
