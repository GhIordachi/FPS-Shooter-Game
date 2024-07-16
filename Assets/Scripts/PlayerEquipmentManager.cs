using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    PlayerManager playerManager;
    WeaponLoaderSlot weaponLoaderSlot;

    [Header("Current Equipment")]
    public WeaponItem weapon;
    public WeaponAnimatorManager weaponAnimator;
    RightHandTarget rightHandIK;
    LeftHandTarget leftHandIK;
    //ammo etc.

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        LoadWeaponLoaderSlots();
    }

    private void Start()
    {
        LoadCurrentWeapon();
    }

    private void LoadWeaponLoaderSlots()
    {
        weaponLoaderSlot = GetComponentInChildren<WeaponLoaderSlot>();
    }

    public void LoadCurrentWeapon()
    {
        weaponLoaderSlot.LoadWeaponModel(weapon);
        playerManager.playerAnimatorManager.animator.runtimeAnimatorController = weapon.weaponAnimator;
        weaponAnimator = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<WeaponAnimatorManager>();
        rightHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<RightHandTarget>();
        leftHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<LeftHandTarget>();
        playerManager.playerAnimatorManager.AssignHandIK(rightHandIK, leftHandIK);
        playerManager.playerUIManager.currentAmmoCountText.text = weapon.remainingAmmo.ToString();
        playerManager.playerUIManager.weaponIcon.sprite = weapon.weaponIcon;

        if (playerManager.playerInventoryManager.currentAmmoInInventory != null)
        {
            if (playerManager.playerInventoryManager.currentAmmoInInventory.ammoType == weapon.ammoType)
            {
                playerManager.playerUIManager.reservedAmmoCountText.text = playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining.ToString();
            }
            else
            {
                // Find and assign the correct AmmoItem from the inventory
                AmmoItem matchingAmmoItem = FindAmmoItemByType(weapon.ammoType);
                if (matchingAmmoItem != null)
                {
                    playerManager.playerInventoryManager.currentAmmoInInventory = matchingAmmoItem;
                    playerManager.playerUIManager.reservedAmmoCountText.text = matchingAmmoItem.ammoRemaining.ToString();
                }
                else
                {
                    // Handle the case where no matching ammo item is found, e.g., set reserved ammo to 0
                    playerManager.playerUIManager.reservedAmmoCountText.text = "0";
                }
            }
        }
        else
        {
            // Find and assign the correct AmmoItem from the inventory
            AmmoItem matchingAmmoItem = FindAmmoItemByType(weapon.ammoType);
            if (matchingAmmoItem != null)
            {
                playerManager.playerInventoryManager.currentAmmoInInventory = matchingAmmoItem;
                playerManager.playerUIManager.reservedAmmoCountText.text = matchingAmmoItem.ammoRemaining.ToString();
            }
            else
            {
                // Handle the case where no matching ammo item is found, e.g., set reserved ammo to 0
                playerManager.playerUIManager.reservedAmmoCountText.text = "0";
            }
        }
    }

    // Method to find the AmmoItem by ammoType in the inventory
    private AmmoItem FindAmmoItemByType(AmmoType ammoType)
    {
        foreach (AmmoItem ammoItem in playerManager.playerInventoryManager.ammosInventory)
        {
            if (ammoItem.ammoType == ammoType)
            {
                return ammoItem;
            }
        }
        return null; // Return null if no matching ammo item is found
    }
}

