using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private AudioMixer audioMixer;
    private GameObject player;
    private MouseLook playerMouseLook;
    private Camera mainCamera;
    public static event Action s_Resume;

    private void Start()
    {
        Invoke("Setup", 1f);
    }

    private void Setup()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMouseLook = player.GetComponent<MouseLook>();
        mainCamera = Camera.main;
    }
    public void Pause() => transform.GetChild(0).gameObject.SetActive(true);
    public void Resume()
    { 
        transform.GetChild(0).gameObject.SetActive(false);
        s_Resume?.Invoke();
    }
    public void OpenPanel(GameObject panel) => panel.SetActive(true);
    public void Back(GameObject panel) => panel.SetActive(false);
    public void Options() => optionsPanel.SetActive(true);
    public void QuitToMainMenu() => SceneManager.LoadScene(0);
    public void QuitGame() => Application.Quit();
    public void UpdateSensitivity(Slider s)
    {
        playerMouseLook.mouseSensitivityX = s.value;
        playerMouseLook.mouseSensitivityY = s.value;
    }
    public void UpdateScreenMode(Toggle t) => Screen.fullScreenMode = t.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    
    public void SetMasterVolume(Slider volume){
        audioMixer.SetFloat ("master", volume.value);
    }
    
    public void SetMusicVolume(Slider volume){
        audioMixer.SetFloat ("music", volume.value);
    }
    
    public void SetSFXVolume(Slider volume){
        audioMixer.SetFloat ("sfx", volume.value);
    }
    public void UpdateFOV(Slider s) => mainCamera.fieldOfView = s.value;

    private void PauseGame()
    {
        Pause();
    }

    private void ResumeGame()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Resume();
    }

    private void OnEnable()
    {
        GameControl.s_PauseGame += PauseGame;
        GameControl.s_ResumeGame += ResumeGame;
    }

    private void OnDisable()
    {
        GameControl.s_PauseGame -= PauseGame;
        GameControl.s_ResumeGame -= ResumeGame;
    }

}
