using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    PlayerManager playerManager;
    KillCount killCount;

    [Header("UI Windows")]
    public GameObject hudObject;
    public GameObject pauseMenu;
    public GameObject deathScreen;

    [Header("Crosshair")]
    public GameObject crosshair;

    [Header("Healthbar")]
    public HealthBar healthBar;

    [Header("Ammo")]
    public Text currentAmmoCountText;
    public Text reservedAmmoCountText;

    [Header("Weapon Equipped")]
    public Image weaponIcon;

    [Header("Health Kits")]
    public Text currentHealthKitsAvailableText;

    [Header("Kill Count")]
    public Text zombiesKilledText;
    public Text zombiesNeededToKillText;

    [Header("Total Deaths")]
    public Text totalDeathsText;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        healthBar = FindObjectOfType<HealthBar>();
        killCount = FindObjectOfType<KillCount>();
        if(killCount != null )
            zombiesNeededToKillText.text = killCount.killsNeededToWin.ToString();
        totalDeathsText.text = playerManager.losesCount.ToString();
    }

    public void UpdateKillCount()
    {
        zombiesKilledText.text = killCount.count.ToString();
    }

    public void DeactivatePauseMenu()
    {
        hudObject.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ActivatePauseMenu()
    {
        hudObject.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void LoadLastCheckpoint()
    {
        playerManager.saveManager.LoadGame();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
