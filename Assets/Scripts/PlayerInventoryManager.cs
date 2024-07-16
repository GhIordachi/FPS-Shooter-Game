using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    PlayerManager playerManager;

    public AmmoItem currentAmmoInInventory;
    public HealthKitItem currentHealthKitItem;
    public WeaponItem[] gunsInventory;
    public AmmoItem[] ammosInventory;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        foreach(WeaponItem guns in gunsInventory)
        {
            if (guns != null)
            {
                if (guns.remainingAmmo < guns.maxAmmo)
                {
                    guns.remainingAmmo = guns.maxAmmo;                   
                }
            }
        }
    }

    private void Start()
    {
        UpdateHealthKitOnStart();
    }

    void UpdateHealthKitOnStart()
    {
        playerManager.playerUIManager.currentHealthKitsAvailableText.text = currentHealthKitItem.healthKitCount.ToString();
    }
}
