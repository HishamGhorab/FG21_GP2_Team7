using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject DialoguePrefab;
    [SerializeField] private Image background;
    private SoundComponent soundComponent;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        soundComponent = GetComponent<SoundComponent>();
    }

    public void StartGame()
    {
        PlayIntroduction();
        Time.timeScale = 1f;
    }

    private void PlayIntroduction() => DialoguePrefab.GetComponent<DialogueHandler>().LoadLine();
    private void LoadGameScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void ButtonPress() => soundComponent.PlaySound("Buttonclick");
    public void ExitGame()
    {
        Debug.Log("Game exited");
    }

    private void OnEnable()
    {
        DialogueHandler.s_StartGame += LoadGameScene;
    }

    private void OnDisable()
    {
        DialogueHandler.s_StartGame -= LoadGameScene;
    }
}
