using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDeathScreen : MonoBehaviour
{
    CanvasGroup canvas;
    PlayerManager playerManager;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public void DisplayDeathPopUp()
    {
        playerManager.playerUIManager.hudObject.SetActive(false);
        StartCoroutine(FadeInPopUp());
    }

    IEnumerator FadeInPopUp()
    {
        gameObject.SetActive(true);

        for (float fade = 0.05f; fade < 1; fade = fade + 0.05f)
        {
            canvas.alpha = fade;

            if (fade > 0.9f)
            {
                StartCoroutine(FadeOutPopUp());
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeOutPopUp()
    {
        yield return new WaitForSeconds(2);

        for (float fade = 1f; fade > 0; fade = fade - 0.05f)
        {
            canvas.alpha = fade;

            if (fade <= 0.05f)
            {
                gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
