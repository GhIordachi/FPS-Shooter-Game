using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillCount : MonoBehaviour
{
    DisplayWinScreen winScreen;

    public int count = 0;
    public int killsNeededToWin = 4;

    private void Awake()
    {
        winScreen = FindObjectOfType<DisplayWinScreen>();
        count = 0;
        SoundManager.instance.PlayMusic("GameMusic");
    }

    private void Update()
    {
        if(count >= killsNeededToWin)
        {
            winScreen.DisplayWinPopUp();
            count = 0;
            StartCoroutine(WaitToGoToMainMenu());
        }
    }

    IEnumerator WaitToGoToMainMenu()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
