using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    PlayerManager playerManager;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public void StartGame()
    {
        playerManager.saveManager.SaveGame();
        SceneManager.LoadSceneAsync("Game City");
    }

    public void LoadGame()
    {
        if (playerManager.saveManager.SaveFileExists())
        {
            playerManager.saveManager.LoadGame();
        }
        else
        {
            StartGame();
        }
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
