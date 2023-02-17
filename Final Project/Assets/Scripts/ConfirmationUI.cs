using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ConfirmationUI : MonoBehaviour
{
    public GameObject ConfirmUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            DontWantToExitGame();
    }
    public void ExitGameFromPause()
    {
        SceneManager.LoadScene(GameManager.SCENE_MENU);
    }

    public void ExitGameFromGameOver()
    {
        SceneManager.LoadScene(GameManager.SCENE_MENU);
    }

    public void OpenConfirmation()
    {
        ConfirmUI.SetActive(true);
    }

    public void DontWantToExitGame()
    {
        ConfirmUI.SetActive(false);
    }

    public void ExitGameFromMenu()
    {
        QuitGame();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}
