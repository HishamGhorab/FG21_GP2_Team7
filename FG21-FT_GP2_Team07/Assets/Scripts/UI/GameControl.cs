using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameControl : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject optionsButton;
    public static event Action s_PauseGame;
    public static event Action s_ResumeGame;
    bool paused;

    private void Start()
    {
        paused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                PauseGame();
            }
            else
            {
                s_ResumeGame?.Invoke();
                ResumeGame();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Options()
    {
        
    }

    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        s_PauseGame?.Invoke();
        //PauseMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //CloseMenu();
    }

    //private void PauseMenu()
    //{
    //    quitButton.SetActive(true);
    //    mainMenuButton.SetActive(true);
    //    optionsButton.SetActive(true);
    //}

    //private void CloseMenu()
    //{
    //    quitButton.SetActive(false);
    //    mainMenuButton.SetActive(false);
    //    optionsButton.SetActive(false);
    //}

    private void OnEnable()
    {
        PauseMenu.s_Resume += ResumeGame;
    }

    private void OnDisable()
    {
        PauseMenu.s_Resume -= ResumeGame;
    }
}
