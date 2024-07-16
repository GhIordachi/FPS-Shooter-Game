using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    PlayerManager playerManager;
    DisplayDeathScreen displayDeathScreen;

    [Header("Health")]
    public int playerMaxHealth = 100;
    public int playerCurrentHealth = 100;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        displayDeathScreen = FindObjectOfType<DisplayDeathScreen>();
        playerCurrentHealth = playerMaxHealth;
        if(playerManager.playerUIManager != null )
            playerManager.playerUIManager.healthBar.SetMaxHealth(playerMaxHealth);
    }

    public void GetDamaged(int damage)
    {
        if (playerCurrentHealth > 0)
        {
            playerCurrentHealth -= damage;
            playerManager.playerUIManager.healthBar.SetCurrentHealth(playerCurrentHealth);
            SoundManager.instance.PlaySound("player_hit");
        }
        if(playerCurrentHealth <= 0)
        {
            ManageDeath();
        }
    }

    public void HealPlayer(int healAmount)
    {
        playerCurrentHealth += healAmount;
        if (playerCurrentHealth > playerMaxHealth)
            playerCurrentHealth = playerMaxHealth;
        playerManager.playerUIManager.healthBar.SetCurrentHealth(playerCurrentHealth);
    }

    public void ManageDeath()
    {
        if (playerCurrentHealth <= 0)
        {
            playerCurrentHealth = 0;
            playerManager.playerAnimatorManager.PlayAnimation("Death_01", true);
            playerManager.losesCount++;
            playerManager.saveManager.SaveLosesCount(playerManager.losesCount);
            SoundManager.instance.PlaySound("player_die");
            displayDeathScreen.DisplayDeathPopUp();
            StartCoroutine(WaitToRespawn());
            
        }
    }

    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(4);
        playerManager.saveManager.LoadGame();
    }
}
