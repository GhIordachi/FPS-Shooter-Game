using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void CheckpointSaved(PlayerManager player, int checkpointNumber)
    {
        if(player != null)
        {
            switch (checkpointNumber)
            {
                case 1:
                    player.checkpoint1 = true;
                    break;
                case 2:
                    player.checkpoint2 = true;
                    break;
                case 3:
                    player.checkpoint3 = true;
                    break;
                default:
                    break;
            }
            //Resets the ammo and healthKits
            foreach(AmmoItem ammo in player.playerInventoryManager.ammosInventory)
            {
                if(ammo.ammoRemaining < ammo.maxAmmoStackCapacity)
                {
                    ammo.ammoRemaining = ammo.maxAmmoStackCapacity;
                    player.playerUIManager.reservedAmmoCountText.text = ammo.ammoRemaining.ToString();
                }
            }

            if (playerManager.playerInventoryManager.currentAmmoInInventory.ammoType == playerManager.playerEquipmentManager.weapon.ammoType)
            {
                playerManager.playerUIManager.reservedAmmoCountText.text = playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining.ToString();
            }
            else
            {
                // Find and assign the correct AmmoItem from the inventory
                AmmoItem matchingAmmoItem = FindAmmoItemByType(playerManager.playerEquipmentManager.weapon.ammoType);
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

            if (player.playerInventoryManager.currentHealthKitItem.healthKitCount < player.playerInventoryManager.currentHealthKitItem.maxHealthKitCount)
            {
                player.playerInventoryManager.currentHealthKitItem.healthKitCount = player.playerInventoryManager.currentHealthKitItem.maxHealthKitCount;
                player.playerUIManager.currentHealthKitsAvailableText.text = player.playerInventoryManager.currentHealthKitItem.healthKitCount.ToString();
            }

            player.saveManager.SaveGame();
        }
    }

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
