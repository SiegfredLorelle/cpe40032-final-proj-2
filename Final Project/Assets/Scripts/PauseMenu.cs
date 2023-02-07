using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public AudioSource[] audioSources;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.isGamePaused = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.isGameStopped)
        {
            if (GameManager.Instance.isGamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
    public void Resume ()
    {
        GameManager.Instance.isGamePaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        // Unpause all sound sources
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.UnPause();
        }
    }

    public void Pause ()
    {
        GameManager.Instance.isGamePaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        // Find all audio sources then pause them
        audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Pause();
        }

    }

     public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        
    }
  

    
}
